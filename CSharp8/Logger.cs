using System;

namespace Demo
{
    class LoggerProgram
    {
        LoggerProgram()
        {
            ILogger2 logger1 = new ConsoleLogger();
            logger1.Log("ciao!");

            ConsoleLogger logger2 = new ConsoleLogger();
            // logger2.Log("ciao2!")
        }
    }

    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message) => logger.Log(LogLevel.Information, message);

        public static void Log(this ILogger logger, Exception ex) => logger.Log(LogLevel.Error, ex.ToString());
    }

    interface ILogger2
    {
        void Log(LogLevel level, string message);

        LogLevel DefaultLevel => LogLevel.Information;

        void Log(string message) => Log(DefaultLevel, message);

        void Log(Exception ex) => Log(LogLevel.Error, ex.ToString());
    }

    class ConsoleLogger : ILogger2
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
