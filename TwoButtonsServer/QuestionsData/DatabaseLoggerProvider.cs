using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace QuestionsData
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private class DatabaseLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId,
                TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                File.AppendAllText("log.txt", formatter(state, exception));
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
