namespace spaar.ModLoader.Injector
{
    using global::Injector;
    using global::Injector.IO;
    using System;
    using System.IO;

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

        public static string ManagedFolderPath
        {
            get { return _managedFolderPath ?? (_managedFolderPath = null); }
            set { _managedFolderPath = value; }
        }

        public static FileManager FileManager
        {
            get { return _fileManager ?? (_fileManager = new FileManager()); }
        }

        private static void Main(string[] args)
        {
            ModLogger.Init();

            try
            {
                FindCorrectPaths();
            }
            catch
            {
                Console.Error.WriteLine("Could not find your game files. Please make sure Injector.exe is at least inside the game folders.");
            }

            if (!string.IsNullOrEmpty(Program._modsFolderPath) && !string.IsNullOrEmpty(Program._managedFolderPath))
            {
                try
                {
                    ModLogger.WriteLine(ConsoleColor.Green, "Reading modules: \n");
                    new InjectionManager(FileManager, Program._modsFolderPath, Program._managedFolderPath).InjectDefaultAndBackup();

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
                Program.ManagedFolderPath = baseFolder + managedFolder;
            }
            else
            {
                Console.Error.WriteLine("Could not find {0} folder.", searchString);

                Program.ModsFolderPath = null;
                Program.ManagedFolderPath = null;
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