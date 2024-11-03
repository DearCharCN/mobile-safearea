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
            return NativeBridge.getOutsideHeightUnit();
        }

        public static ScreenDirection GetScreenDirection()
        {
            return NativeBridge.getScreenDirection();
        }
    }
}
