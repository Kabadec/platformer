using System;
using UnityEditor;

namespace PixelCrew.UI.Widgets.Editor
{
    [CustomEditor(typeof(LifeBarWidget))]
    public class LifeBarWidgetEditor : UnityEditor.Editor
    {
        private SerializedProperty _scaleMode;

        private void OnEnable()
        {
            _scaleMode = serializedObject.FindProperty("_saveWorldScale");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_lifeBar"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_hp"));

            EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(_scaleMode);

            if (_scaleMode.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_invertXScale"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_invertYScale"));
            }
            serializedObject.ApplyModifiedProperties();

            //base.OnInspectorGUI();
        }
    }
}