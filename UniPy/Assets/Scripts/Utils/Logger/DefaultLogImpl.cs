using System;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

namespace Disc0ver.Engine
{
    public class DefaultLogImpl : ILogImpl
    {
        
        private static readonly StringBuilder _stringBuilder = new StringBuilder(1024);
        
        public void Log(LogLevel level, object message)
        {
            StringBuilder infoBuilder = GetFormatString(level, message.ToString(), true);
            string logStr = infoBuilder.ToString();

            if (level is LogLevel.Error or LogLevel.Warning or LogLevel.Fatal)
            {
                StackFrame[] stackFrames = new StackTrace().GetFrames();
                if (stackFrames != null)
                {
                    for (int i = 0; i < stackFrames.Length; i++)
                    {
                        var frame = stackFrames[i];
                        var declaringTypeName = frame.GetMethod().DeclaringType.FullName;
                        string methodName = frame.GetMethod().Name;
                        
                        infoBuilder.AppendFormat("[{0}::{1}\n", declaringTypeName, methodName);
                    }
                }
            }
            
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    Debug.Log(logStr);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(logStr);
                    break;
                case LogLevel.Error:
                    Debug.LogError(logStr);
                    break;
                case LogLevel.Fatal:
                    throw new Exception(logStr);
            }
        }

        /// <summary>
        /// 日志 format
        /// </summary>
        /// <param name="level"> 日志级别 </param>
        /// <param name="logString"> 日志消息 </param>
        /// <param name="bColor"> 是否使用颜色 </param>
        /// <returns></returns>
        private static StringBuilder GetFormatString(LogLevel level, string logString, bool bColor)
        {
            _stringBuilder.Clear();
            switch (level)
            {
                case LogLevel.Debug:
                    _stringBuilder.AppendFormat(
                        bColor
                            ? "<color=gray><b>[Debug] ► </b></color> - <color=#00FF18>{0}</color>"
                            : "<color=#00FF18><b>[Debug] ► </b></color> - {0}",
                        logString);
                    break;
                case LogLevel.Info:
                    _stringBuilder.AppendFormat(
                        bColor
                            ? "<color=gray><b>[INFO] ► </b></color> - <color=gray>{0}</color>"
                            : "<color=gray><b>[INFO] ► </b></color> - {0}",
                        logString);
                    break;
                case LogLevel.Warning:
                    _stringBuilder.AppendFormat(
                        bColor
                            ? "<color=#FF9400><b>[WARNING] ► </b></color> - <color=yellow>{0}</color>"
                            : "<color=#FF9400><b>[WARNING] ► </b></color> - {0}",
                        logString);
                    break;
                case LogLevel.Error:
                    _stringBuilder.AppendFormat(
                        bColor
                            ? "<color=red><b>[ERROR] ► </b></color> - <color=red>{0}</color>"
                            : "<color=red><b>[ERROR] ► </b></color>- {0}",
                        logString);
                    break;
                case LogLevel.Fatal:
                    _stringBuilder.AppendFormat(
                        bColor
                            ? "<color=red><b>[EXCEPTION] ► </b></color> - <color=red>{0}</color>"
                            : "<color=red><b>[EXCEPTION] ► </b></color> - {0}",
                        logString);
                    break;
            }

            return _stringBuilder;
        }
        
    }
}