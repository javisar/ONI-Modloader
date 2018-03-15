using System;
using System.IO;
using Injector;
using Injector.IO;

namespace spaar.ModLoader.Injector
{
    class Program
    {
        private static FileManager _fileManager;

        public static FileManager FileManager
        {
            get { return _fileManager ?? (_fileManager = new FileManager()); }
        }

        static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();

            Console.WriteLine("Assembly-UnityScript.dll location:");

            //string pathUnityScript = Console.ReadLine();
            string pathUnityScript = currentPath+"\\Assembly-CSharp.dll.orig";

            Console.WriteLine("Output path:");

            //string pathOutput = Console.ReadLine();
            string pathOutput = currentPath + "\\Assembly-CSharp.dll";

            Console.WriteLine("Using Assembly.dll at " + pathUnityScript + " and writing to " + pathOutput);



            new InjectionManager(FileManager).InjectDefaultAndBackup();

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
