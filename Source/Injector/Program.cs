using System;
using System.IO;
using Injector.IO;

namespace Injector
{
    internal class Program
    {
        private static FileManager _fileManager;
        private static string _modsFolderPath;
        private static string _managedFolderPath;

        public static string ModsFolderPath
        {
            get { return _modsFolderPath ?? (_modsFolderPath = null); }
            set { _modsFolderPath = value; }
        }



        public static FileManager FileManager
        {
            get { return _fileManager ?? (_fileManager = new FileManager()); }
        }

        private static void Main(string[] args)
        {
            ModLogger.Init();

            string currentPath = Directory.GetCurrentDirectory();
            // Paths return garbage for Mac
            //try
            //{
            //    FindCorrectPaths();
            //}
            //catch
            //{
            //    Console.Error.WriteLine("Could not find your game files. Please make sure Injector.exe is at least inside the Managed folder.");
            //}

         //   if (!string.IsNullOrEmpty(Program._modsFolderPath) && !string.IsNullOrEmpty(Program._managedFolderPath))
            {
                try
                {
                    ModLogger.WriteLine(ConsoleColor.Green, "Reading modules: \n");
                    new InjectionManager(FileManager).InjectDefaultAndBackup();
                  //  new InjectionManager(FileManager, Program._managedFolderPath).InjectDefaultAndBackup();

                    ModLogger.WriteLine(ConsoleColor.Green, "\nDone! Patch Sucessful.");
                }
                catch (Exception exc)
                {
                    Console.Error.WriteLine("Encountered errors: " + exc);
                }
            }
            Console.WriteLine("\nPress any key to continue . . . ");
            Console.ReadKey();
        }

        private static void FindCorrectPaths()
        {
            // Most stuff not working
            string currentDirectory = Directory.GetCurrentDirectory();
            string searchString = FindGameName();
            string baseFolder = "";
            string modsFolder = Path.DirectorySeparatorChar + "Mods" + Path.DirectorySeparatorChar;
            string managedFolder = Path.DirectorySeparatorChar + searchString + "_Data" + Path.DirectorySeparatorChar + "Managed" + Path.DirectorySeparatorChar;
            const int not_found = -1;

            ModLogger.WriteLine(ConsoleColor.Green, "Searching for {0} files . . . \n", searchString);

            if (currentDirectory.IndexOf(searchString) != not_found)
            {
                do
                {
                    baseFolder = currentDirectory;
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                } while (currentDirectory.IndexOf(searchString) != not_found);

                Program.ModsFolderPath = baseFolder + modsFolder;
            }
            else
            {
                Console.Error.WriteLine("Could not find {0} folder.", searchString);

                Program.ModsFolderPath = null;
            }
        }

        private static string FindGameName()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string searchString = "common";
            string baseFolder = "";
            string gameFolder = "";
            const int not_found = -1;

            if (currentDirectory.IndexOf(searchString) != not_found)
            {
                do
                {
                    gameFolder = baseFolder;
                    baseFolder = currentDirectory;
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                } while (currentDirectory.IndexOf(searchString) != not_found);

                baseFolder += Path.DirectorySeparatorChar;
                return gameFolder.Replace(baseFolder, "");
            }
            return null;
        }
    }
}