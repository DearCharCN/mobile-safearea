package com.dc.safearea;

import android.content.Context;
import android.hardware.display.DisplayManager;
import android.view.Display;

public class ScreenResolutionMonitor {

    private DisplayManager displayManager;
    private DisplayManager.DisplayListener displayListener;

    public ScreenResolutionMonitor(Context context) {
        displayManager = (DisplayManager) context.getSystemService(Context.DISPLAY_SERVICE);
    }

    public void startMonitoring(Runnable onResolutionChange) {
        displayListener = new DisplayManager.DisplayListener() {
            @Override
            public void onDisplayAdded(int displayId) {
                // �µ���ʾ�豸�����
            }

            @Override
            public void onDisplayRemoved(int displayId) {
                // ��ʾ�豸���Ƴ�
            }

            @Override
            public void onDisplayChanged(int displayId) {
                Display display = displayManager.getDisplay(displayId);
                if (display != null) {
                    // �����ص�
                    onResolutionChange.run();
                }
            }
        };

        displayManager.registerDisplayListener(displayListener, null);
    }

    public void stopMonitoring() {
        if (displayListener != null) {
            displayManager.unregisterDisplayListener(displayListener);
            displayListener = null;
        }
    }
}
