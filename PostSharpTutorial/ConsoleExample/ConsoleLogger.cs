using System;
using LoggerAspect;
using LoggerAspect.Interfaces;

namespace ConsoleExample
{
    class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        public void Error(string message, Exception exception)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(exception);
            Console.ForegroundColor = color;
        }

        public void Fatal(string message, Exception exception)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.WriteLine(exception);
            Console.ForegroundColor = color;
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Trace(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        public void Warn(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}