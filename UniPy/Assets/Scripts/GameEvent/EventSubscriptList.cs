using System;
using System.Collections.Generic;

namespace Disc0ver.Engine
{
    internal class EventSubscriptList
    {
        private readonly int _eventType = 0;
        private readonly List<Delegate> _existDelegates = new List<Delegate>();
        private readonly List<Delegate> _addDelegates = new List<Delegate>();
        private readonly List<Delegate> _deleteDelegates = new List<Delegate>();
        private bool _inExecute = false;
        private bool _dirty = false;

        internal EventSubscriptList(int eventType)
        {
            _eventType = eventType;
        }

        internal bool AddHandler(Delegate handler)
        {
            if (_existDelegates.Contains(handler))
            {
                Log.Fatal("[Engine][EventSubscriptList][AddHandler] Repeated Add Handler");
                return false;
            }

            if (_inExecute)
            {
                _dirty = true;
                _addDelegates.Add(handler);
            }
            else
            {
                _existDelegates.Add(handler);
            }

            return true;
        }

        internal void RemoveHandler(Delegate handler)
        {
            if (_inExecute)
            {
                _dirty = true;
                _deleteDelegates.Add(handler);
            }
            else
            {
                _existDelegates.Remove(handler);
            }
        }

        private void UpdateList()
        {
            _inExecute = false;
            if (_dirty)
            {
                foreach (var handler in _deleteDelegates)
                {
                    _existDelegates.Remove(handler);
                }
                
                _existDelegates.Clear();
                
                foreach (var handler in _addDelegates)
                {
                    _existDelegates.Add(handler);
                }

                _addDelegates.Clear();
            }
        }

        public void Callback()
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action action)
                    action();
            }

            UpdateList();
        }

        public void Callback<T1>(T1 arg1)
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action<T1> action)
                    action(arg1);
            }

            UpdateList();
        }
        
        public void Callback<T1, T2>(T1 arg1, T2 arg2)
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action<T1, T2> action)
                    action(arg1, arg2);
            }

            UpdateList();
        }
        
        public void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action<T1, T2, T3> action)
                    action(arg1, arg2, arg3);
            }

            UpdateList();
        }
        
        public void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action<T1, T2, T3, T4> action)
                    action(arg1, arg2, arg3, arg4);
            }

            UpdateList();
        }
        
        public void Callback<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            _inExecute = true;
            foreach (var handler in _existDelegates)
            {
                if (handler is Action<T1, T2, T3, T4, T5> action)
                    action(arg1, arg2, arg3, arg4, arg5);
            }

            UpdateList();
        }
        
    }
}