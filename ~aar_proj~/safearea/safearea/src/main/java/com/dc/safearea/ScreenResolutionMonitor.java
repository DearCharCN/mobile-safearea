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
                // 新的显示设备被添加
            }

            @Override
            public void onDisplayRemoved(int displayId) {
                // 显示设备被移除
            }

            @Override
            public void onDisplayChanged(int displayId) {
                Display display = displayManager.getDisplay(displayId);
                if (display != null) {
                    // 触发回调
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
