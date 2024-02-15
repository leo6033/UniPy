using System;
using System.Collections.Generic;

namespace Disc0ver.Engine
{
    public static class StringId
    {
        private static readonly Dictionary<string, int> String2IdMap = new Dictionary<string, int>();
        private static readonly Dictionary<int, string> Id2StringMap = new Dictionary<int, string>();

        private static int _currentId = 0;

        public static int GetId(string value)
        {
            if (String2IdMap.TryGetValue(value, out var id))
                return id;
            id = ++_currentId;
            String2IdMap[value] = id;
            Id2StringMap[id] = value;
            return id;
        }

        public static string GetString(int value)
        {
            return Id2StringMap.TryGetValue(value, out var str) ? str : String.Empty;
        }
    }
}