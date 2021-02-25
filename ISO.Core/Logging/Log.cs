using System;
using System.Collections.Generic;
using System.Text;

namespace ISO.Core.Logging
{
    public static class Log
    {
        public static bool IsEnabled { get; set; } = true;
        public static void Write(string text)
        {
            if (IsEnabled)
            {
                Console.WriteLine(text);
            }
        }

        public static void Info(string text)
        {
            if (IsEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        public static void Warning(string text)
        {
            if (IsEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        public static void Error(string text)
        {
            if (IsEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        public static void Script(string text)
        {
            if (IsEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

    }
}
