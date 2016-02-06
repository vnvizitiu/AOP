using System;
using LoggerAspect;

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
    }
}