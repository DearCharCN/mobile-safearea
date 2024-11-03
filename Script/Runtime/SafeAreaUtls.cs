using System;
using System.Collections.Generic;
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

        #region event
        static Dictionary<Action,int> resolutionChangedCallbacks = new Dictionary<Action,int>();
        public static void AddResolutionChanged(Action callback)
        {
            if (resolutionChangedCallbacks.ContainsKey(callback))
                return;
            resolutionChangedCallbacks.Add(callback,1);
        }

        public static void RemoveResolutionChanged(Action callback)
        {
            if (!resolutionChangedCallbacks.ContainsKey(callback))
                return;
            resolutionChangedCallbacks.Remove(callback);
        }

        internal static void PerformResolutionChanged()
        {
            foreach(var kv in resolutionChangedCallbacks)
            {
                try
                {
                    kv.Key?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        #endregion
    }
}
