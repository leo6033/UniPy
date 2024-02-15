namespace Disc0ver.Engine
{
    public static class Log
    {
        private static ILogImpl _logImpl = new DefaultLogImpl();

        public static void SetLogImpl(ILogImpl logImpl)
        {
            _logImpl = logImpl;
        }

        public static void Debug(object message)
        {
            _logImpl?.Log(LogLevel.Debug, message);
        }

        public static void Info(object message)
        {
            _logImpl?.Log(LogLevel.Info, message);
        }

        public static void Error(object message)
        {
            _logImpl?.Log(LogLevel.Error, message);
        }

        public static void Warning(object message)
        {
            _logImpl?.Log(LogLevel.Warning, message);
        }

        public static void Fatal(object message)
        {
            _logImpl?.Log(LogLevel.Fatal, message);
        }
    }
}