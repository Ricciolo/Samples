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
            // Non compila!
            // logger2.Log("ciao2!");

            // Membro visibile perché implementato su ConsoleLogger
            Console.WriteLine(logger2.DefaultLevel);
        }
    }

    interface ILogger
    {
        void Log(LogLevel level, string message);
        void Log(string message) => Log(DefaultLevel, message);
        void Log(Exception ex) => Log(LogLevel.Error, ex.ToString());

        LogLevel DefaultLevel => LogLevel.Information;
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
