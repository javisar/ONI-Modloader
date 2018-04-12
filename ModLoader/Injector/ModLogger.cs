namespace Injector
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class ModLogger : TextWriter
    {
        private static FileStream _filestream = new FileStream("Mod_Log.txt", FileMode.Create);
        private static StreamWriter _streamwriter = new StreamWriter(_filestream) { AutoFlush = true };
        private static List<TextWriter> _writers = new List<TextWriter>() { _streamwriter, Console.Out };

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public class ErrorLogger : ModLogger
        {
            public override void Write(string value)
            {
                foreach (var writer in ModLogger._writers)
                {
                    try
                    {
                        ConsoleColor originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        writer.Write(value);
                        Console.ForegroundColor = originalColor;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Logger Write Error: " + e);
                    }
                }
            }

            public override void WriteLine(string value)
            {
                foreach (var writer in ModLogger._writers)
                {
                    try
                    {
                        ConsoleColor originalColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        writer.WriteLine(value);
                        Console.ForegroundColor = originalColor;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Logger WriteLine Error: " + e);
                    }
                }
            }
        }

        public override void Write(string value)
        {
            foreach (var writer in ModLogger._writers)
            {
                try
                {
                    writer.Write(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Logger Write Error: " + e);
                }
            }
        }

        public override void WriteLine(string value)
        {
            foreach (var writer in ModLogger._writers)
            {
                try
                {
                    writer.WriteLine(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Logger WriteLine Error: " + e);
                }
            }
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            foreach (var writer in ModLogger._writers)
            {
                try
                {
                    writer.WriteLine(value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Logger WriteLine Error: " + e);
                }
            }
            Console.ForegroundColor = originalColor;
        }

        public static void WriteLine(ConsoleColor color, string value, string searchString)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            foreach (var writer in ModLogger._writers)
            {
                try
                {
                    writer.WriteLine(value, searchString);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Logger WriteLine Error: " + e);
                }
            }
            Console.ForegroundColor = originalColor;
        }

        public static void Init()
        {
            Console.SetOut(new ModLogger());
            Console.SetError(new ModLogger.ErrorLogger());
        }
    }
}

