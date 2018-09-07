namespace ModLoader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class ModLogger : TextWriter
    {
        private static string logPath = Directory.GetCurrentDirectory().ToString() + Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar + "Mod_Log.txt";
        private static FileStream _filestream = new FileStream(logPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        private static StreamWriter _streamwriter = new StreamWriter(_filestream) { AutoFlush = true };
        private static List<TextWriter> _writers = new List<TextWriter>() { _streamwriter, Console.Out };

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public class Error : ModLogger
        {
            new public static void Write(string value)
            {
                foreach (var writer in ModLogger._writers)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    writer.Write("[= ERROR =] " + value);
                    Console.ForegroundColor = originalColor;
                }
            }

            new public static void WriteLine(string value)
            {
                foreach (var writer in ModLogger._writers)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    writer.WriteLine("[= ERROR =] " + value);
                    Console.ForegroundColor = originalColor;
                }
            }

            new public static void WriteLine(Object obj)
            {
                foreach (var writer in ModLogger._writers)
                {
                    ConsoleColor originalColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    writer.WriteLine("[= ERROR =] " + obj);
                    Console.ForegroundColor = originalColor;
                }
            }
        }

        new public static void Write(string value)
        {
            foreach (var writer in ModLogger._writers)
            {
                writer.Write("[= INFO =] " + value);
            }
        }

        new public static void WriteLine(string value)
        {
            foreach (var writer in ModLogger._writers)
            {
                writer.WriteLine("[= INFO =] " + value);
            }
        }

        public static void Init()
        {
            Console.SetOut(new ModLogger());
            Console.SetError(new ModLogger.Error());
        }
    }
}
