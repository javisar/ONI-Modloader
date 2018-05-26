namespace spaar.ModLoader.Injector
{
    using global::Injector;
    using global::Injector.IO;
    using System;
    using System.IO;

	internal class Program
    {
        private static FileManager _fileManager;

        public static FileManager FileManager
        {
            get
            {
                return _fileManager ?? (_fileManager = new FileManager());
            }
        }

        private static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();

            Console.WriteLine("Assembly-UnityScript.dll location:");

            // string pathUnityScript = Console.ReadLine();
            string pathUnityScript = currentPath + "\\Assembly-CSharp.dll.orig";

            Console.WriteLine("Output path:");

            // string pathOutput = Console.ReadLine();
            string pathOutput = currentPath + "\\Assembly-CSharp.dll";

            Console.WriteLine("Using Assembly.dll at " + pathUnityScript + " and writing to " + pathOutput);

            new InjectionManager(FileManager).InjectDefaultAndBackup();

			CreateModsDirectory();

			Console.WriteLine("Done.");
            Console.Read();
        }

		private static void CreateModsDirectory()
		{
			try
			{
				string currentPath = Directory.GetCurrentDirectory();
				DirectoryInfo dataDir = new DirectoryInfo(currentPath);

				Console.WriteLine("OSVersion: {0}", Environment.OSVersion.ToString());

				DirectoryInfo oniBaseDirectory;
				if (!Environment.OSVersion.ToString().Contains("Windows"))
				{
					oniBaseDirectory = dataDir.Parent?.Parent.Parent;
				}
				else
				{
					oniBaseDirectory = dataDir.Parent.Parent;
				}

				string modsDir = Path.Combine(oniBaseDirectory?.FullName, "Mods");
				Console.WriteLine("Creating mods folder is: " + modsDir);

				if (!Directory.Exists(modsDir))
				{
					Directory.CreateDirectory(modsDir);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Creating mods folder failed!");
				Console.WriteLine(e.Message);
			}
		}
		
	}
}