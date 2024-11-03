using SafeArea.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SafeArea
{
    public class SafeAreaComponentMgr
    {
        public static SafeAreaComponentMgr m_ins;

        public static SafeAreaComponentMgr Ins
        {
            get
            {
                if (m_ins == null)
                {
                    m_ins = new SafeAreaComponentMgr();
                }
                return m_ins;
            }
        }

        Dictionary<SafeAreaComponent,int> components = new Dictionary<SafeAreaComponent,int>();

        public void Register(SafeAreaComponent component)
        {
            components[component] = 0;
            TryAddLinstenResolutionChanged();
        }

        public void UnRegister(SafeAreaComponent component)
        {
            components.Remove(component);
        }

        public void RefreshAllComponents()
        {
            foreach (SafeAreaComponent component in components.Keys)
            {
                component.ApplySafeOffset();
            }
        }

        #region 监听屏幕分辨率变化
        bool isListenResolutionChanged = false;
        bool needRefreshOnResolutionChanged = false;
        private void TryAddLinstenResolutionChanged()
        {
            if (isListenResolutionChanged)
                return;
            TryCreateDriver();
            NativeBridge.AddListenResolutionChanged(OnResolutionChanged);
            isListenResolutionChanged = true;
        }

        private void TryCreateDriver()
        {
            GameObject gameObject = new GameObject("SafeAreaDriver");
            gameObject.AddComponent<SafeAreaDriver>();
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void OnResolutionChanged()
        {
            needRefreshOnResolutionChanged = true;
        }

        public class SafeAreaDriver:MonoBehaviour
        {
            private void Update()
            {
                if (SafeAreaComponentMgr.Ins.needRefreshOnResolutionChanged)
                {
                    SafeAreaComponentMgr.Ins.needRefreshOnResolutionChanged = false;
                    SafeAreaComponentMgr.Ins.RefreshAllComponents();
                }
            }
        }
        #endregion
    }
}