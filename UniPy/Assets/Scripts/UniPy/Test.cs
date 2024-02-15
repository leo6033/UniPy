using System;
using UnityEngine;

namespace Disc0ver.PythonPlugin
{
    public class Test: MonoBehaviour
    {
        private void Start()
        {
            PythonModule.RunString("import test");
        }
    }
}