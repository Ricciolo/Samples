using System;

namespace Demo
{
    class LoggerProgram
    {
        LoggerProgram()
        {
            ILogger logger1 = new ConsoleLogger();
            logger1.Log("ciao!");

            ConsoleLogger logger2 = new ConsoleLogger();
            // logger2.Log("ciao2!")
        }
    }

    interface ILogger
    {
        void Log(LogLevel level, string message);

        LogLevel DefaultLevel => LogLevel.Information;
        void Log(string message) => Log(DefaultLevel, message);
        void Log(Exception ex) => Log(LogLevel.Error, ex.ToString());
    }

    class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Console.WriteLine($"{level}: {message}");
        }

        public LogLevel DefaultLevel => LogLevel.Debug;
    }

    public enum LogLevel
    {
        Error,
        Information,
        Debug
    }
}
