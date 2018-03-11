using System;
using Mono.Cecil;
using System.IO;

namespace spaar.ModLoader.Injector
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();

            Console.WriteLine("Assembly-UnityScript.dll location:");

            File.Delete(currentPath + "\\Assembly-CSharp.dll.orig");
            File.Move(currentPath + "\\Assembly-CSharp.dll", currentPath + "\\Assembly-CSharp.dll.orig");

            //string pathUnityScript = Console.ReadLine();
            string pathUnityScript = currentPath+"\\Assembly-CSharp.dll.orig";

            Console.WriteLine("Output path:");

            //string pathOutput = Console.ReadLine();
            string pathOutput = currentPath + "\\Assembly-CSharp.dll";

            Console.WriteLine("Using Assembly-UnityScript.dll at " + pathUnityScript + " and writing to " + pathOutput);

            AssemblyDefinition aUnityScript
              = AssemblyDefinition.ReadAssembly(pathUnityScript);

            Injector.Inject(aUnityScript, pathOutput);

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
