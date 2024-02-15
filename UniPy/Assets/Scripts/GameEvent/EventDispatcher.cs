using System;
using System.Collections.Generic;

namespace Disc0ver.Engine
{
    public class EventDispatcher
    {
        private static readonly Dictionary<int, EventSubscriptList> EventSubscriptDict =
            new Dictionary<int, EventSubscriptList>();

        public EventDispatcher()
        {
            
        }

        public bool AddEventListener(int eventType, Delegate handler)
        {
            if (!EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list = new EventSubscriptList(eventType);
                EventSubscriptDict.Add(eventType, list);
            }

            return list.AddHandler(handler);
        }

        public void RemoveEventListener(int eventType, Delegate handler)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.RemoveHandler(handler);
            }
        }

        public void Send(int eventType)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback();
            }
        }

        public void Send<T1>(int eventType, T1 arg1)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback(arg1);
            }
        }
        
        public void Send<T1, T2>(int eventType, T1 arg1, T2 arg2)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback(arg1, arg2);
            }
        }
        
        public void Send<T1, T2, T3>(int eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback(arg1, arg2, arg3);
            }
        }
        
        public void Send<T1, T2, T3, T4>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback(arg1, arg2, arg3, arg4);
            }
        }
        
        public void Send<T1, T2, T3, T4, T5>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (EventSubscriptDict.TryGetValue(eventType, out var list))
            {
                list.Callback(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }
}