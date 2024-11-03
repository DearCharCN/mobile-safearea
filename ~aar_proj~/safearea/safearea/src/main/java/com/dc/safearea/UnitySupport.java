package com.dc.safearea;

import android.util.Log;
import android.app.Activity;
import android.os.Build;
import android.util.DisplayMetrics;
import android.view.DisplayCutout;
import android.view.Window;
import android.view.WindowInsets;
import androidx.annotation.RequiresApi;
import android.view.Surface;
import android.view.WindowManager;
import android.content.res.Configuration;

/** @noinspection LossyEncoding*/
public class UnitySupport {

    //获取刘海高度 单位：像素
    public int getOutsideHeight(Activity activity){
        int result = 0;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
            result = getNotchHeight(activity.getWindow());
        }
        return result;
    }

    //获取刘海占屏幕高度百分比 取值：0.0-1.0
    public float getOutsideUnit(Activity activity)
    {
        float result = 0;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
            int noticSize = 0;
            int fullSize = 0;
            noticSize = getNotchHeight(activity.getWindow());
            fullSize = getHeight(activity.getWindow());
            result = (float)noticSize / (float)fullSize;
        }
        return result;
    }

    //设备是有刘海
    @RequiresApi(api = 28)
    public boolean isNotchScreen(Window window) {
        WindowInsets windowInsets = window.getDecorView().getRootWindowInsets();
        if (windowInsets == null) {
            return false;
        }

        DisplayCutout displayCutout = windowInsets.getDisplayCutout();
        if(displayCutout == null || displayCutout.getBoundingRects() == null){
            return false;
        }

        return true;
    }

    @RequiresApi(api = 28)
    private int getNotchHeight(Window window) {
        int notchHeight = 0;
        WindowInsets windowInsets = window.getDecorView().getRootWindowInsets();
        if (windowInsets == null) {
            return 0;
        }

        DisplayCutout displayCutout = windowInsets.getDisplayCutout();
        if(displayCutout == null || displayCutout.getBoundingRects() == null){
            return 0;
        }

        notchHeight = displayCutout.getSafeInsetTop();

        return notchHeight;
    }

    private int getHeight(Window window) {
        DisplayMetrics metric = new DisplayMetrics();
        window.getWindowManager().getDefaultDisplay().getRealMetrics(metric);
        return  metric.heightPixels;
    }

    private static final String TAG   = "Linear";

    //获取屏幕方向
    //返回屏幕方向
    //1 正竖屏
    //2 倒竖屏
    //3 正横屏
    //4 倒横屏
    public int getScreenDiraction(Activity activity)
    {
        // 获取当前屏幕方向
        int orientation = activity.getResources().getConfiguration().orientation;
        int rotation = ((WindowManager) activity.getSystemService(Activity.WINDOW_SERVICE)).getDefaultDisplay().getRotation();
        //Log.e(TAG, "=====orientation====="+orientation);
        //Log.e(TAG, "=====rotation====="+rotation);
        if (orientation == Configuration.ORIENTATION_PORTRAIT) {
            if (rotation == Surface.ROTATION_0) {
                return  1;
            } else if (rotation == Surface.ROTATION_180) {
                return  2;
            }else if (rotation == Surface.ROTATION_90) {
                return  3;
            } else if (rotation == Surface.ROTATION_270) {
                return  4;
            }
        } else if (orientation == Configuration.ORIENTATION_LANDSCAPE) {
            if (rotation == Surface.ROTATION_0) {
                return  3;
            } else if (rotation == Surface.ROTATION_180) {
                return  4;
            }else if (rotation == Surface.ROTATION_90) {
                return  3;
            } else if (rotation == Surface.ROTATION_270) {
                return  4;
            }
        }

        return  0;
    }

    ScreenResolutionMonitor resolutionChangedHandle;
    //注册 监听屏幕分辨率变化（折叠屏）
    public void addListenResolutionChanged(Activity activity,java.lang.Runnable onResolutionChange)
    {
        removeListenResolutionChanged();
        resolutionChangedHandle = new ScreenResolutionMonitor(activity);
        // 开始监听屏幕分辨率变化
        resolutionChangedHandle.startMonitoring(onResolutionChange);
    }
    //取消 监听屏幕分辨率变化（折叠屏）
    public void removeListenResolutionChanged()
    {
        if (resolutionChangedHandle != null)
        {
            resolutionChangedHandle.stopMonitoring();
            resolutionChangedHandle = null;
        }
    }
}

