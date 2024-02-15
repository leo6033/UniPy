using System;
using System.Collections.Generic;
using UnityEngine;

namespace Disc0ver.Engine
{
    
    public class GameManager : MonoBehaviour
    {

        private static GameManager _instance;

        private static GameObject _go;
        
        public static GameManager Instance{
            get
            {
                if(_instance == null)
                {
                    var go = Resources.Load<GameObject>("GameManager");
                    _go = GameObject.Instantiate(go);
                    _go.name = "GameManger";
                    DontDestroyOnLoad(_go);
                    _instance = _go.GetComponent<GameManager>();
                }

                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance != null)
            {
                Log.Error("[Engine][GameManager] script GameManager has already loaded !!!");
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
        }
    }
}