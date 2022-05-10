using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PyroMod
{
    public class PyroLogs
    {
        internal static MelonLogger.Instance logger = new MelonLogger.Instance("PyroMod", ConsoleColor.Red);
        internal static string CurrentLogFile;

        internal static void Log(string msg) => ProcessLog(LogLevel.Log, msg, ConsoleColor.Gray);
        internal static void Log(string msg, ConsoleColor col) => ProcessLog(LogLevel.Log, msg, col);
        internal static void Warning(string msg) => ProcessLog(LogLevel.Warning, msg, ConsoleColor.Gray);
        internal static void Warning(string msg, ConsoleColor col) => ProcessLog(LogLevel.Warning, msg, col);
        internal static void Error(string msg) => ProcessLog(LogLevel.Error, msg, ConsoleColor.Gray);
        internal static void Error(string msg, ConsoleColor col) => ProcessLog(LogLevel.Error, msg, col);
        internal static void Success(string msg) => ProcessLog(LogLevel.Success, msg, ConsoleColor.Green);
        internal static void Failure(string msg) => ProcessLog(LogLevel.Failure, msg, ConsoleColor.Red);

        internal static void Initialize()
        {
            var directoryInfo = new DirectoryInfo("UserData\\PyroMod\\Logs");
            if (directoryInfo.GetFiles().Length == 5)
            {
                File.Delete(directoryInfo.GetFiles()
                    .OrderByDescending(f => f.LastWriteTime)
                    .Last().FullName);
            }
            var rnd = new System.Random().Next(1000, 9999);
            var file = File.Create($"UserData\\PyroMod\\Logs\\PyroLogs-{rnd}.log");
            file.Close();
            CurrentLogFile = file.Name;
        }

        internal static void ProcessLog(LogLevel lvl, string msg, ConsoleColor col, Instance instance = null)
        {
            string fullLog = string.Empty;
            var time = DateTime.Now.ToString("HH:mm:fffff");
            time = time.Insert(8, ".");
            fullLog += $"[{time}] [PyroMod] ";
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(time);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("PyroMod");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");
            if (instance != null)
            {
                fullLog += $" [{instance.ModuleName}] ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[");
                Console.ForegroundColor = instance.ModuleColor;
                Console.Write(instance.ModuleName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("] ");
            }
            fullLog += $"[{lvl}] ";
            Console.Write(" [");
            Console.ForegroundColor = LogLevelToColor(lvl);
            Console.Write(lvl);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            fullLog += msg;
            Console.WriteLine(msg);
            Console.ResetColor();
            File.AppendAllText(CurrentLogFile, fullLog + "\n");
        }

        private static ConsoleColor LogLevelToColor(LogLevel lvl)
        {
            switch (lvl)
            {
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;

                case LogLevel.Error:
                    return ConsoleColor.Red;

                case LogLevel.Success:
                    return ConsoleColor.Green;

                case LogLevel.Failure:
                    return ConsoleColor.Red;

                default:
                    return ConsoleColor.DarkGray;
            }
        }

        public class Instance
        {
            public Instance(string moduleName, ConsoleColor col)
            {
                ModuleName = moduleName.Replace(' ', '-');
                ModuleColor = col;
            }

            public string ModuleName { get; }
            public ConsoleColor ModuleColor { get; } = ConsoleColor.DarkGray;

            public void Log(string msg) => ProcessLog(LogLevel.Log, msg, ConsoleColor.Gray);
            public void Log(string msg, ConsoleColor col) => ProcessLog(LogLevel.Log, msg, col);
            public void Warning(string msg) => ProcessLog(LogLevel.Warning, msg, ConsoleColor.Gray);
            public void Warning(string msg, ConsoleColor col) => ProcessLog(LogLevel.Warning, msg, col);
            public void Error(string msg) => ProcessLog(LogLevel.Error, msg, ConsoleColor.Gray);
            public void Error(string msg, ConsoleColor col) => ProcessLog(LogLevel.Error, msg, col);
            public void Success(string msg) => ProcessLog(LogLevel.Success, msg, ConsoleColor.Green);
            public void Failure(string msg) => ProcessLog(LogLevel.Failure, msg, ConsoleColor.Red);
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
