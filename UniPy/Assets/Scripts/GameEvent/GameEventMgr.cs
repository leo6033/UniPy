using System;
using System.Collections.Generic;

namespace Disc0ver.Engine
{
    public class GameEventMgr
    {
        private readonly List<string> _eventTypes;
        private readonly List<Delegate> _handlers;

        public GameEventMgr()
        {
            _eventTypes = new List<string>();
            _handlers = new List<Delegate>();
        }

        public void Clear()
        {
            for (int i = 0; i < _eventTypes.Count; i++)
            {
                var eventType = _eventTypes[i];
                var handler = _handlers[i];
                GameEvent.RemoveEventListener(eventType, handler);
            }
            
            _eventTypes.Clear();
            _handlers.Clear();
        }

        private void AddEventImp(string eventType, Delegate handler)
        {
            _eventTypes.Add(eventType);
            _handlers.Add(handler);
        }

        public void AddEvent(string eventType, Action handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
        
        public void AddEvent<T1>(string eventType, Action<T1> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
        
        public void AddEvent<T1, T2>(string eventType, Action<T1, T2> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
        
        public void AddEvent<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
        
        public void AddEvent<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
        
        public void AddEvent<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            if (GameEvent.AddEventListener(eventType, handler))
            {
                AddEventImp(eventType, handler);
            }
        }
    }
}