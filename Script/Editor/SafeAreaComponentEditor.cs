#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SafeArea.UI
{
    [CustomEditor(typeof(SafeAreaComponent))]
    public class SafeAreaComponentEditor : Editor
    {
        [MenuItem("GameObject/UI/SafeArea")]
        public static void AddSafeAreaComponent()
        {
            GameObject go = ObjectFactory.CreateGameObject("SafeArea");
            var com = go.AddComponent<SafeAreaComponent>();

            if (Selection.activeGameObject != null)
            {
                go.transform.SetParent(Selection.activeGameObject.transform, false);
            }

            go.transform.localPosition = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            com.rectTransform.anchorMin = Vector2.zero;
            com.rectTransform.anchorMax = Vector2.one;
            com.rectTransform.sizeDelta = Vector2.zero;
        }

        public static float EditorTopOffset = 0;


        [MenuItem("刘海预览/开 \\ 关")]
        public static void CloseSafeArea()
        {
            if(SafeAreaUtls.EditorUnit == 0)
            {
                SafeAreaUtls.EditorUnit = 0.05f;
            }
            else
            {
                SafeAreaUtls.EditorUnit = 0f;
            }
            SafeAreaComponentMgr.Ins.RefreshAllComponents();
        }
    }
}
#endif