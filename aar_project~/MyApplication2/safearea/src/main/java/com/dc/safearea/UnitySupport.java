package com.dc.safearea;

import android.app.Activity;
import android.os.Build;
import android.util.DisplayMetrics;
import android.view.DisplayCutout;
import android.view.Window;
import android.view.WindowInsets;
import androidx.annotation.RequiresApi;

public class UnitySupport {

    public int getOutsideHeight(Activity activity){
        int result = 0;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
            result = getNotchHeight(activity.getWindow());
        }
        return result;
    }

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
    public int getNotchHeight(Window window) {
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

    public int getHeight(Window window) {
        DisplayMetrics metric = new DisplayMetrics();
        window.getWindowManager().getDefaultDisplay().getRealMetrics(metric);
        return  metric.heightPixels;
    }
}
