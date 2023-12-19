using UnityEngine;

namespace SafeArea
{
    public class SafeAreaUtls
    {
#if UNITY_EDITOR
        public static float EditorUnit = 0;
#endif

        public static float GetTopOffsetUnit()
        {
#if UNITY_EDITOR
            return EditorUnit;
#endif
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return GetOffsetUnitOnAndroid();
                case RuntimePlatform.WindowsPlayer:
                    return GetOffsetUnitOnWindows();
                case RuntimePlatform.IPhonePlayer:
                    return GetOffsetUnitOnApple();
            }
            return 0;
        }

        private static float GetOffsetUnitOnAndroid()
        {
            return NativeBridge.getOutsideHeightUnit();
        }

        private static float GetOffsetUnitOnWindows()
        {
            return 0;
        }

        private static float GetOffsetUnitOnApple()
        {
            return 0.08f;
        }
    }
}
