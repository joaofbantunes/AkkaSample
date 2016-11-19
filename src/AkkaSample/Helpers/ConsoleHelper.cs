using System;

namespace AkkaSample.Helpers
{
    public class ConsoleHelper
    {
        public static void Write(string text, params object[] args)
        {
            var originalForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text, args);
            Console.ForegroundColor = originalForeground;
        }
    }
}
