using SafeArea.UI;
using System.Collections.Generic;

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
    }
}