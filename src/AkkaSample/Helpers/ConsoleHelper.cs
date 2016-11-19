using System;
using System.Threading.Tasks;

namespace AkkaSample.Helpers
{
    public static class ConsoleHelper
    {
        private static readonly object SyncRoot = new object();

        public static void WriteNow(string text, params object[] args)
        {
            WriteNow(DateTime.Now, text, args);
        }

        private static void WriteNow(DateTime dateTime, string text, params object[] args)
        {
            lock (SyncRoot) //should be using an actor instead of locking shouldn't I? :)
            {
                var originalForeground = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(dateTime.ToString("hh:mm:ss:fff") + " - " +text, args);
                Console.ForegroundColor = originalForeground;
            }
        }

        public static void Write(string text, params object[] args)
        {
            WriteNow(text, args);
        }

        public static async Task WriteAsync(string text, params object[] args)
        {
            var now = DateTime.Now;
            await Task.Run(() => WriteNow(now, text, args));
        }
    }
}
