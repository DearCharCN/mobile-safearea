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
            float offsetUnit = SafeAreaUtls.GetTopOffsetUnit();
            float offset = 0;
            ScreenDirection screenDirection = SafeAreaUtls.GetScreenDirection();
            Canvas rootCanvas = GetRootCanvas();
            if (rootCanvas != null)
            {
                var rect = rootCanvas.pixelRect;
                switch (screenDirection)
                {
                    case ScreenDirection.Portrait:
                    case ScreenDirection.PortraitUpsideDown:
                        offset = rect.height * offsetUnit;
                        break;
                    case ScreenDirection.LandscapeRight:
                    case ScreenDirection.LandscapeLeft:
                        offset = rect.width * offsetUnit;
                        break;
                }
            }
            AdjustSize(offset, screenDirection);
        }

        private void AdjustSize(float offset, ScreenDirection dir)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            switch (dir)
            {
                case ScreenDirection.Portrait:
                    rectTransform.sizeDelta = new Vector2(0, -offset);
                    rectTransform.anchoredPosition = new Vector2(0, -offset / 2f);
                    break;
                case ScreenDirection.PortraitUpsideDown:
                    rectTransform.sizeDelta = new Vector2(0, -offset);
                    rectTransform.anchoredPosition = new Vector2(0, offset / 2f);
                    break;
                case ScreenDirection.LandscapeRight:
                    rectTransform.sizeDelta = new Vector2(-offset, 0);
                    rectTransform.anchoredPosition = new Vector2(-offset / 2f, 0);
                    break;
                case ScreenDirection.LandscapeLeft:
                    rectTransform.sizeDelta = new Vector2(-offset, 0);
                    rectTransform.anchoredPosition = new Vector2(offset / 2f, 0);
                    break;
            }
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