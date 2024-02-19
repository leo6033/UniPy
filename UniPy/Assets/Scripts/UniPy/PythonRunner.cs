using Python.Runtime;
using UnityEditor;

namespace Disc0ver.PythonPlugin
{
    public class PythonRunner
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void Initialize()
        {
            // EditorApplication.delayCall -= PythonModule.Initialize;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
            
            // EditorApplication.delayCall += PythonModule.Initialize;
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }
        
        private static void OnPlayModeStateChange(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                PythonModule.Initialize();
                PythonModule.Reload();
            }
            // if (change == PlayModeStateChange.EnteredEditMode)
            //     PythonModule.PyShutdown();
        }
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            Application.quitting += PythonShutdown;
            PythonModule.Initialize();
        }
        
        private static void PythonShutdown()
        {
            Application.quitting -= PythonShutdown;
            PythonModule.PyShutdown();
        }
#endif

        
    }
}