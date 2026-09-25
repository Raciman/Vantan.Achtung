using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ach.UI.Editor
{
    [CustomEditor(typeof(SceneLoader)), CanEditMultipleObjects]
    public sealed class SceneLoaderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var id = serializedObject.FindProperty("sceneId");
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).ToArray();
            EditorGUILayout.PropertyField(id, new GUIContent("Scene ID"));
            var labels = scenes.Select((scene, index) => $"{index}: {Path.GetFileNameWithoutExtension(scene.path)}").ToArray();
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUILayout.Popup("Scene", id.intValue, labels);
            if (EditorGUI.EndChangeCheck())
                id.intValue = selected;
            if (id.intValue < 0 || id.intValue >= scenes.Length)
                EditorGUILayout.HelpBox("Choose an enabled scene from Build Settings.", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
