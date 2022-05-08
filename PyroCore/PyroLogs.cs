using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroMod
{
    public class PyroLogs
    {
        internal static MelonLogger.Instance logger = new MelonLogger.Instance("PyroMod", ConsoleColor.Red);

        internal static void Log(string msg) => ProcessLog(msg, ConsoleColor.Gray);
        internal static void Log(string msg, ConsoleColor col) => ProcessLog(msg, col);
        internal static void Warning(string msg) => ProcessLog("[Warning] " + msg, ConsoleColor.Gray);
        internal static void Warning(string msg, ConsoleColor col) => ProcessLog("[Warning] " + msg, col);
        internal static void Error(string msg) => ProcessLog("[Error] " + msg, ConsoleColor.Gray);
        internal static void Error(string msg, ConsoleColor col) => ProcessLog("[Error] " + msg, col);
        internal static void Success(string msg) => ProcessLog("[Success] " + msg, ConsoleColor.Green);
        internal static void Failure(string msg) => ProcessLog("[Failure] " + msg, ConsoleColor.Red);

        private static void ProcessLog(string msg, ConsoleColor col)
        {
            logger.Msg(col, msg);
        }

        public class Instance
        {
            public string ModuleName { get; private set; }
            public Instance(string moduleName)
            {
                ModuleName = moduleName;
            }

            public void Log(string msg) => ProcessLog(LogLevel.Log, msg, ConsoleColor.Gray);
            public void Log(string msg, ConsoleColor col) => ProcessLog(LogLevel.Log, msg, col);
            public void Warning(string msg) => ProcessLog(LogLevel.Warning, msg, ConsoleColor.Gray);
            public void Warning(string msg, ConsoleColor col) => ProcessLog(LogLevel.Warning, msg, col);
            public void Error(string msg) => ProcessLog(LogLevel.Error, msg, ConsoleColor.Gray);
            public void Error(string msg, ConsoleColor col) => ProcessLog(LogLevel.Error, msg, col);
            public void Success(string msg) => ProcessLog(LogLevel.Success, msg, ConsoleColor.Green);
            public void Failure(string msg) => ProcessLog(LogLevel.Failure, msg, ConsoleColor.Red);

            private void ProcessLog(LogLevel logLvl, string msg, ConsoleColor col)
            {
                logger.Msg(col, $"[{logLvl} {msg}");
            }
        }

        internal enum LogLevel
        {
            Log,
            Warning,
            Error,
            Success,
            Failure
        }
    }
}
