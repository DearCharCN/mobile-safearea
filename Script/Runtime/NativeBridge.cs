#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace SafeArea
{
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
            return new StubNativeClass();
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

        public interface NativeClass
        {
            int getOutsideHeight();

            float getOutsideHeightUnit();
        }

#if UNITY_ANDROID

        public class AndroidNativeClass : NativeClass
        {
            private UnityEngine.AndroidJavaObject javaClass;

            public AndroidNativeClass()
            {
                javaClass = new UnityEngine.AndroidJavaObject("com.dc.safearea.UnitySupport");
            }

            int NativeClass.getOutsideHeight()
            {
                UnityEngine.AndroidJavaClass act = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var actObj = act.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
                int nativeResult = javaClass.Call<int>("getOutsideHeight", actObj);
                return nativeResult;
            }

            float NativeClass.getOutsideHeightUnit()
            {
                UnityEngine.AndroidJavaClass act = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var actObj = act.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
                float nativeResult = javaClass.Call<float>("getOutsideUnit", actObj);
                return nativeResult;
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

        [DllImport("__Internal")]
        private static extern float getScreenHeight();

        [DllImport("__Internal")]
        private static extern float getNotchHeight();

        [DllImport("__Internal")]
        private static extern float getNotchPercentage();
    }
#else

        public class StubNativeClass : NativeClass
        {
            int NativeClass.getOutsideHeight()
            {
                return 0;
            }

            float NativeClass.getOutsideHeightUnit()
            {
                return 0;
            }
        }

#endif
    }
}