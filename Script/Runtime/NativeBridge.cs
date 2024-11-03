#if UNITY_ANDROID
#elif UNITY_IOS
using System.Runtime.InteropServices;
#else
#endif
using System;
using UnityEngine;

namespace SafeArea
{
    public enum ScreenDirection
    {
        /// <summary>
        /// 竖屏
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// 倒竖屏
        /// </summary>
        PortraitUpsideDown = 2,
        /// <summary>
        /// 横向（刘海在左）
        /// </summary>
        LandscapeRight = 3,
        /// <summary>
        /// 横向（刘海在右）
        /// </summary>
        LandscapeLeft = 4,
    }

    public class NativeBridge
    {
        private static NativeClass m_nativeClass;

        private static NativeClass nativeClass
        {
            get
            {
                if (m_nativeClass == null)
                {
                    m_nativeClass = GetNativeClass();
                }
                return m_nativeClass;
            }
        }

        private static NativeClass GetNativeClass()
        {
#if UNITY_ANDROID
            return new AndroidNativeClass();
#elif UNITY_IOS
            return new AppleNativeClass();
#else
            return new StandardNativeClass();
#endif
        }

        public static int getOutsideHeight()
        {
            return nativeClass.getOutsideHeight();
        }

        public static float getOutsideHeightUnit()
        {
            return nativeClass.getOutsideHeightUnit();
        }

        public static ScreenDirection getScreenDirection()
        {
            return nativeClass.GetScreenDirection();
        }

        public static void AddListenResolutionChanged(Action callback)
        {
            nativeClass.AddListenResolutionChanged(callback);
        }

        public interface NativeClass
        {
            int getOutsideHeight();

            float getOutsideHeightUnit();

            ScreenDirection GetScreenDirection();

            void AddListenResolutionChanged(Action onResolutionChanged);

            void RemoveListenResolutionChanged();
        }
#if UNITY_ANDROID

        public class AndroidNativeClass : NativeClass
        {
            private UnityEngine.AndroidJavaObject javaClass;

            public AndroidNativeClass()
            {
                javaClass = new UnityEngine.AndroidJavaObject("com.dc.safearea.UnitySupport");
            }

            public AndroidJavaObject GetCurrentActivity()
            {
                UnityEngine.AndroidJavaClass unityPlayerClass = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = unityPlayerClass.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
                return currentActivity;
            }


            int NativeClass.getOutsideHeight()
            {
                var currentActivity = GetCurrentActivity();
                int nativeResult = javaClass.Call<int>("getOutsideHeight", currentActivity);
                return nativeResult;
            }

            float NativeClass.getOutsideHeightUnit()
            {
                var currentActivity = GetCurrentActivity();
                float nativeResult = javaClass.Call<float>("getOutsideUnit", currentActivity);
                return nativeResult;
            }

            ScreenDirection NativeClass.GetScreenDirection()
            {
                var currentActivity = GetCurrentActivity();
                int nativeResult = javaClass.Call<int>("getScreenDiraction", currentActivity);
                return (ScreenDirection)nativeResult;
            }

            void NativeClass.AddListenResolutionChanged(Action onResolutionChanged)
            {
                AndroidJavaRunnable runnable = new AndroidJavaRunnable(onResolutionChanged);
                var currentActivity = GetCurrentActivity();
                int nativeResult = javaClass.Call<int>("addListenResolutionChanged", currentActivity, runnable);
            }

            void NativeClass.RemoveListenResolutionChanged()
            {
                var currentActivity = GetCurrentActivity();
                int nativeResult = javaClass.Call<int>("removeListenResolutionChanged");
            }
        }

#elif UNITY_IOS
        public class AppleNativeClass : NativeClass
        {
            int NativeClass.getOutsideHeight()
            {
                float heightF = getNotchHeight();
                int result = (int)heightF;
                UnityEngine.Debug.Log($"刘海高度为{heightF}");
                return result;
            }

            float NativeClass.getOutsideHeightUnit()
            {
                float result = getNotchPercentage();
                UnityEngine.Debug.Log($"刘海占比{result}");
                return result;
            }

            ScreenDirection NativeClass.GetScreenDirection()
            {
                return (ScreenDirection)0;
            }

            void NativeClass.AddListenResolutionChanged(Action onResolutionChanged) { }

            void NativeClass.RemoveListenResolutionChanged() { }

            [DllImport("__Internal")]
            private static extern float getScreenHeight();

            [DllImport("__Internal")]
            private static extern float getNotchHeight();

            [DllImport("__Internal")]
            private static extern float getNotchPercentage();
        }
#else

        public class StandardNativeClass : NativeClass
        {
            int NativeClass.getOutsideHeight()
            {
                return 0;
            }

            float NativeClass.getOutsideHeightUnit()
            {
                return 0;
            }

            ScreenDirection NativeClass.GetScreenDirection()
            {
                switch(Screen.orientation)
                {
                    case ScreenOrientation.Portrait:
                        return ScreenDirection.Portrait;
                    case ScreenOrientation.PortraitUpsideDown:
                        return ScreenDirection.PortraitUpsideDown;
                    case ScreenOrientation.LandscapeLeft:
                        return ScreenDirection.LandscapeLeft;
                    case ScreenOrientation.LandscapeRight:
                        return ScreenDirection.LandscapeRight;
                    default:
                        return ScreenDirection.Portrait;
                }
            }

            void NativeClass.AddListenResolutionChanged(Action onResolutionChanged)
            {
            }

            void NativeClass.RemoveListenResolutionChanged()
            {
            }
        }

#endif
    }
}