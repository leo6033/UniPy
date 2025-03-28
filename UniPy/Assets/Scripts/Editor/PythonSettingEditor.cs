using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Disc0ver.Engine
{
    [CustomEditor(typeof(PythonEnvConfig))]
    public class PythonSettingEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUI.changed)
            {
                var settings = (PythonEnvConfig)target;
                EditorUtility.SetDirty(settings);
                settings.Save();
            }

            if (GUILayout.Button("Save"))
            {
                var settings = (PythonEnvConfig)target;
                EditorUtility.SetDirty(settings);
                settings.Save();
            }

            if (GUILayout.Button("Reload"))
            {
                PythonModule.PyShutdown();
                PythonModule.Initialize();
            }
        }
        
        [SettingsProvider]
        static SettingsProvider CreatePythonSettingsProvider()
        {
            return new AssetSettingsProvider("Project/Python Config", () => PythonEnvConfig.Instance);
        }
    }
}