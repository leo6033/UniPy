

using System;
using Python.Runtime;
using UnityEngine;

namespace Disc0ver.Engine
{
    public class PyBehaviour: MonoBehaviour
    {
        public string scriptPath = "";
        public string className = "";
        private PyObject _pyObject;

        public PyObject Env => _pyObject;
        
        private Action _pyAwake;
        private Action _pyStart;
        private Action _pyOnDestroy;

        private void Awake()
        {
            var module = PyModule.Import(scriptPath);
            var pyClass = module.GetAttr(className);
            _pyObject = pyClass.Invoke();
            
            Env.SetAttr("Controller", this.ToPython());

            if (Env.GetAttr("Start") != PyObject.None)
            {
                Debug.Log("behaviour start");
                _pyStart += () =>
                {
                    Env.InvokeMethod("Start");
                };
            }

            if (Env.GetAttr("OnDestroy") != PyObject.None)
            {
                Debug.Log("behaviour destroy");
                _pyOnDestroy += () =>
                {
                    Env.InvokeMethod("OnDestroy");
                };
            }
            
            if (Env.GetAttr("Awake") != PyObject.None)
            {
                Env.InvokeMethod("Awake");
            }
            
        }

        private void Start()
        {
            _pyStart.Invoke();
        }

        private void OnDestroy()
        {
            Env.DelAttr("Controller");
            if(PythonEngine.IsInitialized)
                _pyOnDestroy.Invoke();
            _pyObject = null;
        }
    }
}

