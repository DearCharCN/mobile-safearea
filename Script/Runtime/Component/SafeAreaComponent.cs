using UnityEngine;

namespace SafeArea.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaComponent : MonoBehaviour
    {
        public RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null)
                {
                    m_rectTransform = GetComponent<RectTransform>();
                }
                return m_rectTransform;
            }
        }

        private RectTransform m_rectTransform;

        private void Awake()
        {
            SafeAreaComponentMgr.Ins.Register(this);
        }

        private void OnDestroy()
        {
            SafeAreaComponentMgr.Ins.UnRegister(this);
        }

        private void OnEnable()
        {
            ApplySafeOffset();
        }

        private void OnTransformParentChanged()
        {
            if(enabled)
            {
                ApplySafeOffset();
            }
        }

        public void ApplySafeOffset()
        {
            float topUnit = SafeAreaUtls.GetTopOffsetUnit();
            Canvas rootCanvas = GetRootCanvas();
            if (rootCanvas != null)
            {
                var rect = rootCanvas.pixelRect;
                float topOffset = rect.height * topUnit;
                AdjustSize(topOffset);
            }
            else
            {
                AdjustSize(0);
            }
        }

        private void AdjustSize(float topOffset)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = new Vector2(0, -topOffset);
            rectTransform.anchoredPosition = new Vector2(0, -topOffset / 2f);
        }

        private Canvas GetCanvasInGrandpar()
        {
            Transform ptr = rectTransform;
            do
            {
                var canvas = ptr.GetComponent<Canvas>();
                if (canvas != null)
                    return canvas;
                ptr = ptr.parent;
            }
            while (ptr != null);
            return null;
        }

        private Canvas GetRootCanvas()
        {
            Canvas canvas = GetCanvasInGrandpar();
            if (canvas == null)
                return null;
            if (canvas.isRootCanvas)
            {
                return canvas;
            }
            return canvas.rootCanvas;
        }
    }
}