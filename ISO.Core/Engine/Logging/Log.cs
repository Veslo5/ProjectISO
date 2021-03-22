using System;
using System.ComponentModel;

namespace ISO.Core.Engine.Logging
{
    public static class Log
    {
        private static ILog Logger { get; set; }
        public static bool IsEnabled { get; set; } = true;

        public static void SetLogger(ILog log)
        {
            Logger = log;
        }

        public static void Write(string text, LogModule module = LogModule.EMPTY)
        {
            if (IsEnabled)
            {
                Logger.Write(addModuleBeforeText(text, module));
            }
        }

        /// <summary>
        /// Write without module
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text)
        {
            if (IsEnabled)
            {
                Logger.Write(text);
            }
        }

        /// <summary>
        /// Informational message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="module"></param>
        public static void Info(string text, LogModule module = LogModule.EMPTY)
        {
            if (IsEnabled)
            {
                Logger.Info("[INF]" + addModuleBeforeText(text, module));
            }
        }

        /// <summary>
        /// Warning message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="module"></param>
        public static void Warning(string text, LogModule module = LogModule.EMPTY)
        {
            if (IsEnabled)
            {
                Logger.Warning("[WRN]" + addModuleBeforeText(text, module));
            }
        }

        /// <summary>
        /// Error message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="module"></param>
        public static void Error(string text, LogModule module = LogModule.EMPTY)
        {
            if (IsEnabled)
            {
                Logger.Error("[ERR]" + addModuleBeforeText(text, module));
            }
        }

        /// <summary>
        /// Unique - special case message
        /// </summary>
        /// <param name="text"></param>
        /// <param name="module"></param>
        public static void Unique(string text, LogModule module = LogModule.EMPTY)
        {
            if (IsEnabled)
            {
                Logger.Unique("[UNQ]" + addModuleBeforeText(text, module));
            }
        }

        /// <summary>
        /// add module string [XX] before text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private static string addModuleBeforeText(string text, LogModule module)
        {
            var moduleText = module == LogModule.EMPTY ? string.Empty : Enum.GetName(typeof(LogModule), module);
            return $"[{moduleText}] - {text}";
        }

    }

    public enum LogModule
    {
        /// <summary>
        /// blank/empty module
        /// </summary>
        EMPTY = 0,

        /// <summary>
        /// Loading module
        /// </summary>
        LO = 1,

        /// <summary>
        /// UI module
        /// </summary>
        UI = 2,

        /// <summary>
        /// Core/Engine module
        /// </summary>
        CR = 3,

        /// <summary>
        /// Game Logic
        /// </summary>
        LG = 4,

        /// <summary>
        /// Scripts
        /// </summary>
        SC = 5

    }
}
