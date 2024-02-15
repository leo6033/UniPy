using System;

namespace Disc0ver.Engine
{
    public static class GameEvent
    {
        private static readonly EventDispatcher EventDispatcher = new EventDispatcher();

        public static bool AddEventListener(string eventType, Action handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }
        
        public static bool AddEventListener<T1>(string eventType, Action<T1> handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }

        public static bool AddEventListener<T1, T2>(string eventType, Action<T1, T2> handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }
        
        public static bool AddEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }
        
        public static bool AddEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }
        
        public static bool AddEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            return EventDispatcher.AddEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener(string eventType, Action handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
     
        public static void RemoveEventListener<T1>(string eventType, Action<T1> handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener<T1, T2>(string eventType, Action<T1, T2> handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }
        
        public static void RemoveEventListener(string eventType, Delegate handler)
        {
            EventDispatcher.RemoveEventListener(StringId.GetId(eventType), handler);
        }

        public static void Send(string eventType)
        {
            EventDispatcher.Send(StringId.GetId(eventType));
        }
        
        public static void Send<T1>(string eventType, T1 arg1)
        {
            EventDispatcher.Send(StringId.GetId(eventType), arg1);
        }
        
        public static void Send<T1, T2>(string eventType, T1 arg1, T2 arg2)
        {
            EventDispatcher.Send(StringId.GetId(eventType), arg1, arg2);
        }
        
        public static void Send<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            EventDispatcher.Send(StringId.GetId(eventType), arg1, arg2, arg3);
        }
        
        public static void Send<T1, T2, T3, T4>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            EventDispatcher.Send(StringId.GetId(eventType), arg1, arg2, arg3, arg4);
        }
        
        public static void Send<T1, T2, T3, T4, T5>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            EventDispatcher.Send(StringId.GetId(eventType), arg1, arg2, arg3, arg4, arg5);
        }
    }
}