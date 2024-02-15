using System;
using System.Collections.Generic;
using System.IO;
using Python.Runtime;
using UnityEngine;

namespace Disc0ver.PythonPlugin
{
    [Serializable]
    public class PythonEnvConfig
    {
        public string pythonHome;
        public string pythonDLL;
    }
    
    public class PythonModule
    {
#if UNITY_STANDALONE_OSX 
        private static readonly PythonEnvConfig PythonEnvConfig = new PythonEnvConfig()
        {
            pythonHome ="Python/Mac",
            pythonDLL = "lib/libpython3.10.dylib"
        };
#else
        private static readonly PythonEnvConfig PythonEnvConfig = new PythonEnvConfig()
        {
            pythonHome ="Assets/Resources/PythonLib/Windows",
            pythonDLL = "Python310.dll"
        };
#endif
        // private const string PythonHome = "Python/PythonLib";

        /// <summary>
        /// relative path in unity project root
        /// </summary>
        private static readonly List<string> PySitePackages = new List<string>
        {
            "Python/PyScripts"
        };

        private static PyModule _scope;

        private static bool _isInitialized;
        public static bool IsInitialized => _isInitialized;
        
        static IntPtr _threadState = IntPtr.Zero;

        /// <summary>
        /// Run Python Code
        /// </summary>
        /// <param name="pythonCode"> the code to execute </param>
        /// <param name="scopeName"> value to write to python variable __name__ </param>
        public static void RunString(string pythonCode, string scopeName = null)
        {
            if(string.IsNullOrEmpty(pythonCode))
                return;
            if (!IsInitialized)
            {
                Initialize();
            }

            using (Py.GIL())
            {
                if (string.IsNullOrEmpty(scopeName))
                {
                    PythonEngine.Exec(pythonCode);
                }
                else
                {
                    if (_scope == null)
                        _scope = Py.CreateScope();
                    _scope.Set("__name__", scopeName);
                    _scope.Exec(pythonCode);
                }
            }
        }

        /// <summary>
        /// Run Python File
        /// </summary>
        /// <param name="pythonFile"> python file path </param>
        /// <param name="scopeName"> value to write to python variable __name__ </param>
        public static void RunFile(string pythonFile, string scopeName = null)
        {
            if (!IsInitialized)
            {
                Initialize();
            }

            if (string.IsNullOrEmpty(pythonFile))
            {
                throw new ArgumentException("PythonModule RunFile, Invalid file path");
            }

            pythonFile = Path.GetFullPath(pythonFile);
            pythonFile = pythonFile.Replace("\\", "/");
            if (!File.Exists(pythonFile))
            {
                throw new ArgumentException($"Python File not found {pythonFile}");
            }

            using (Py.GIL())
            {
                if (string.IsNullOrEmpty(scopeName))
                {
                    PythonEngine.Exec($"exec(open('{pythonFile}').read())");
                }
                else
                {
                    using (var scop = Py.CreateScope())
                    {
                        scop.Set("__name__", scopeName);
                        scop.Set("__file__", pythonFile);
                        scop.Exec($"exec(open('{pythonFile}').read())");
                    }
                }
            }
        }

        public static void Initialize()
        {
            if (IsInitialized)
                return;

            try
            {
                _isInitialized = true;
                DoInitialized();
            }
            catch
            {
                _isInitialized = false;
                throw;
            }
        }

        static void DoInitialized()
        {
            SetPythonEnvironment();
            PythonEngine.Initialize();
            
            using (Py.GIL())
            {
                AddSitePackages(PySitePackages);

                var sys = Py.Import("sys");
                var sysPath = sys.GetAttr("path").ToString();
                Debug.Log($"Python Home: {PythonEngine.PythonHome}");
                Debug.Log($"Python Path: {PythonEngine.PythonPath}");
                Debug.Log($"python initialized, sys.path = {sysPath}");
            }

            RedirectStdout();
            
            // AllowThreads();
        }
        
        static void AllowThreads()
        {
            // Let the threads flow! Since the main thread will only execute
            // Python sporadically, make the main thread release the GIL. This
            // mean that every time Python-related code is executed (in the main
            // thread and everywhere else), the GIL must be held.
            _threadState = PythonEngine.BeginAllowThreads();

            // And restore it on shutdown.
            PythonEngine.AddShutdownHandler(() => {PythonEngine.EndAllowThreads(_threadState);});
        }
        
        static void RedirectStdout()
        {
            if(!IsInitialized)
                return;
            using (Py.GIL())
            {
                var redirectStdout = Py.Import("redirecting_stdout");
                redirectStdout.InvokeMethod("redirect_stdout");
                PythonEngine.AddShutdownHandler(UndoRedirectStdout);
            }
        }

        static void UndoRedirectStdout()
        {
            if (!IsInitialized)
            {
                return;
            }
            using (Py.GIL())
            {
                try
                {
                    var redirectStdout = Py.Import("redirecting_stdout");
                    redirectStdout.InvokeMethod("undo_redirection");
                }
                catch (PythonException e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
            PythonEngine.RemoveShutdownHandler(UndoRedirectStdout);
        }

        public static void AddSitePackages(IEnumerable<string> sitePackages)
        {
            if(!IsInitialized)
                return;

            using (Py.GIL())
            {
                var builtins = Py.Import("builtins");
                var sys = Py.Import("sys");
                var sysPath = sys.GetAttr("path");
                var pySitePackages = builtins.InvokeMethod("list");
                var currentPackages = builtins.InvokeMethod("set", sysPath);

                foreach (var sitePackage in sitePackages)
                {
                    var package = new PyString(Environment.CurrentDirectory + "/" + sitePackage);
                    if (!string.IsNullOrEmpty(sitePackage) &&
                        !currentPackages.InvokeMethod("__contains__", package).ToBoolean(null))
                    {
                        pySitePackages.InvokeMethod("append", package);
                    }
                }

                pySitePackages.InvokeMethod("extend", sysPath);
                sys.SetAttr("path", pySitePackages);
            }
        }

        static void SetPythonEnvironment()
        {
            // TODO: Multi platform support
            var prefix = Environment.CurrentDirectory + "/";
            Debug.Log($"prefix: {prefix}");
            Environment.SetEnvironmentVariable("PYTHONDONTWRITEBYTECODE", "1");
            Environment.SetEnvironmentVariable("PYTHONNOUSERSITE", "1");
            Runtime.PythonDLL = prefix + PythonEnvConfig.pythonHome + "/" + PythonEnvConfig.pythonDLL;

#if UNITY_STANDALONE_OSX
            PythonEngine.PythonHome = $"{prefix + PythonEnvConfig.pythonHome}:{prefix + PythonEnvConfig.pythonHome}";
            // PythonEngine.PythonPath = $"{prefix + PythonEnvConfig.pythonHome}/lib;{prefix + PythonEnvConfig.pythonHome}/lib/python3.10;{prefix + PythonEnvConfig.pythonHome}/lib/python3.10/lib-dynload;{prefix + PythonEnvConfig.pythonHome}/lib/python3.10/site-packages;";
#else
            PythonEngine.PythonHome = prefix + PythonEnvConfig.pythonHome;
            PythonEngine.PythonPath = $"{prefix + PythonEnvConfig.pythonHome};{prefix + PythonEnvConfig.pythonHome}/DLLs;{prefix + PythonEnvConfig.pythonHome}/Lib";
#endif
        }

        public static void PyShutdown()
        {
            // RunFile("reload.py", "main");
            // RunString("import reload\nreload.reload()");
            PythonEngine.Shutdown();
        }
        
#if UNITY_EDITOR
        // [MenuItem("Engine/PyReload")]
        public static void Reload()
        {
            RunString("import reload\nreload.reload()");
        }
            
#endif
    }
}