using System;
using UnityEngine;

namespace Disc0ver.PythonPlugin
{
    public class Test: MonoBehaviour
    {
        public GameObject behaviourTestPrefab;
        
        private void Start()
        {
            PythonModule.RunString("import main\nmain.main()");
            GameObject.Instantiate(behaviourTestPrefab);
        }
    }
}