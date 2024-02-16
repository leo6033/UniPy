using System;
using System.Collections.Generic;
using Disc0ver.Engine;
using Disc0ver.PythonPlugin;
using Python.Runtime;
using UnityEditor;
using UnityEngine;

namespace Disc0ver.Editor
{
    [CustomEditor(typeof(PyBehaviour))]
    public class PyBehaviourEditor : UnityEditor.Editor
    {
        private static List<string> _scriptList = RefreshScriptList();
        private static List<string> _classList = new List<string>();

        private SerializedProperty _scriptPathProperty;
        private SerializedProperty _classNameProperty;

        private int _choiceScriptIndex = -1;
        private int _choiceNameIndex = -1;
        private string _filter;
        
        private List<string> FilterScriptList()
        {
            return _scriptList.FindAll(s => string.IsNullOrEmpty(_filter) || s.Contains(_filter));
        }

        private void OnEnable()
        {
            _scriptPathProperty = serializedObject.FindProperty("scriptPath");
            _classNameProperty = serializedObject.FindProperty("className");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var filterList = FilterScriptList();
            _choiceScriptIndex = EditorGUILayout.Popup("ScriptPath", GetIndex(filterList, _scriptPathProperty.stringValue), filterList.ToArray());
            if (_choiceScriptIndex >= 0)
            {
                _scriptPathProperty.stringValue = filterList[_choiceScriptIndex];
                RefreshNameList();
            }

            _choiceNameIndex = EditorGUILayout.Popup("className", GetIndex(_classList, _classNameProperty.stringValue), _classList.ToArray());
            if (_choiceNameIndex >= 0)
            {
                _classNameProperty.stringValue = _classList[_choiceNameIndex];
            }

            EditorGUILayout.BeginHorizontal();

            _filter = EditorGUILayout.TextField("filter", _filter);
            if (GUILayout.Button("Refresh"))
            {
                RefreshScriptList();
            }
            
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshNameList()
        {
            if (String.IsNullOrEmpty(_scriptPathProperty.stringValue))
            {
                return;
            }
            
            _classList.Clear();
            using (Py.GIL())
            {
                var module = PythonModule.Import("Editor.PyBehaviour");
                var classList = module.InvokeMethod("get_script_class", _scriptPathProperty.stringValue.ToPython());
                for (int i = 0; i < classList.Length(); i++)
                {
                    _classList.Add(classList[i].ToString());
                }
            }
        }

        private static List<string> RefreshScriptList()
        {
            if (_scriptList == null)
                _scriptList = new List<string>();

            _scriptList.Clear();
            using (Py.GIL())
            {
                var module = PythonModule.Import("Editor.PyBehaviour");
                var scriptList = module.InvokeMethod("get_all_script_path", "Python/PyScripts".ToPython());
                for (int i = 0; i < scriptList.Length(); i++)
                {
                    _scriptList.Add(scriptList[i].ToString());
                }
            }
            
            return _scriptList;
        }

        private int GetIndex(List<string> list, string value)
        {
            if (string.IsNullOrEmpty(value))
                return -1;

            return list.IndexOf(value);
        }
        
        [MenuItem("Tools/Refresh")]
        private static void RefreshEditor()
        {
            PythonModule.PyShutdown();
            PythonModule.Initialize();
            PythonModule.Reload();
        }
    } 
}

