using UnityEditor;
using UnityEditor.UI;

namespace Plugins.SimpleRoundedImage.Script.Editor
{
    [CustomEditor(typeof(SimpleRoundedImage), true)]
    //[CanEditMultipleObjects]
    public class SimpleRoundedImageEditor : ImageEditor
    {
        private SerializedProperty _radius;
        private SerializedProperty _triangleNum;
        private SerializedProperty _sprite;


        protected override void OnEnable()
        {
            base.OnEnable();

            _sprite = serializedObject.FindProperty("m_Sprite");
            _radius = serializedObject.FindProperty("radius");
            _triangleNum = serializedObject.FindProperty("triangleNum");

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpriteGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();
            var showNativeSize = _sprite.objectReferenceValue != null;
            m_ShowNativeSize.target = showNativeSize;
            NativeSizeButtonGUI();
            EditorGUILayout.PropertyField(_radius);
            EditorGUILayout.PropertyField(_triangleNum);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
