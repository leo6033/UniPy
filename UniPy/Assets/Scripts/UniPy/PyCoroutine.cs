using System.Collections;
using Python.Runtime;
using UnityEngine;

namespace Disc0ver.Engine
{
    public class PyCoroutine
    {
        private static PyCoroutine _instance;

        public static PyCoroutine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PyCoroutine();
                return _instance;
            }
        }
        
        public Coroutine StartCoroutine(PyIter pyIterable)
        {
            return GameManager.Instance.StartCoroutine(CoIterable(pyIterable));
        }

        public IEnumerator CoIterable(PyIter pyIterable)
        {
            Log.Info("[PyCoroutine][CoIterable] Start");
            var isNotEnd = true;
            object enumerator = null;

            while (isNotEnd)
            {
                using (Py.GIL())
                {
                    isNotEnd = pyIterable.MoveNext() && pyIterable.Current != PyObject.None;
                    enumerator = pyIterable.Current == null ? null : pyIterable.Current.As<object>();
                }

                yield return enumerator;
            }
            yield return null;
            pyIterable.Dispose();

        }

        public IEnumerator WaitForSeconds(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}