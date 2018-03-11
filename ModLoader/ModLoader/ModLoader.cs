using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ModLoader
{

    public class ModLoader
    {

        internal static string failureMessage = "";
        internal static bool HasFailed => failureMessage != "";

        public static void Start()
        {
            // Patch in Mod Loader helpers
            HarmonyInstance.Create("Mod Loader").PatchAll(Assembly.GetExecutingAssembly());

            // Load mods
            DirectoryInfo modsDir = GetModsDirectory();
            FileInfo[] files = modsDir.GetFiles("*.dll");

            try
            {
                DependencyGraph dependencyGraph = LoadModAssemblies(files);
                List<Assembly> sortedAssemblies = dependencyGraph.TopologicalSort();

                ApplyHarmonyPatches(sortedAssemblies);
                CallOnLoadMethods(sortedAssemblies);

                Debug.Log("All mods successfully loaded!");
            }
            catch (ModLoadingException mle)
            {
                failureMessage = mle.Message;
            }
        }

        private static DirectoryInfo GetModsDirectory()
        {
            DirectoryInfo dataDir = new DirectoryInfo(Application.dataPath);

            DirectoryInfo oniBaseDirectory;
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                oniBaseDirectory = dataDir.Parent?.Parent; 
            }
            else
            {
                oniBaseDirectory = dataDir.Parent;
            }
            Debug.Log("Path to mods is: " + Path.Combine(oniBaseDirectory?.FullName, "Mods"));
            return new DirectoryInfo(Path.Combine(oniBaseDirectory?.FullName, "Mods"));
        }

        private static DependencyGraph LoadModAssemblies(FileInfo[] assemblyFiles)
        {
            Debug.Log("Loading mod assemblies");
            List<Assembly> loadedAssemblies = new List<Assembly>();
            List<String> failedAssemblies = new List<String>();

            foreach (FileInfo file in assemblyFiles)
            {
                if (file.Extension != ".dll") // GetFiles filter is too inclusive
                {
                    continue;
                }

                if (file.Name.ToLower() == "0harmony" || file.Name.ToLower() == "modloader") // GetFiles filter is too inclusive
                {
                    continue;
                }

                try
                {
                    Assembly modAssembly = Assembly.LoadFrom(file.FullName);
                    Debug.Log("Loading "+modAssembly.FullName);
                    loadedAssemblies.Add(modAssembly);
                }
                catch (Exception e)
                {
                    failedAssemblies.Add(file.Name + ExceptionToString(e));
                    Debug.LogError("Loading mod " + file.Name + " failed!");
                    Debug.LogException(e);
                }
            }

            if (failedAssemblies.Count > 0)
            {
                throw new ModLoadingException("The following mods could not be loaded:", failedAssemblies);
            }

            return new DependencyGraph(loadedAssemblies);
        }

        private static void ApplyHarmonyPatches(List<Assembly> modAssemblies)
        {
            Debug.Log("Applying Harmony patches");
            List<String> failedMods = new List<String>();

            foreach (Assembly modAssembly in modAssemblies)
            {
                try
                {
                    HarmonyInstance.Create(modAssembly.FullName).PatchAll(modAssembly);
                }
                catch (Exception e)
                {
                    failedMods.Add(modAssembly.GetName() + ExceptionToString(e));
                    Debug.LogError("Patching mod " + modAssembly.GetName() + " failed!");
                    Debug.LogException(e);
                }
            }

            if (failedMods.Count > 0)
            {
                throw new ModLoadingException("The following mods could not be patched:", failedMods);
            }
        }

        private static void CallOnLoadMethods(List<Assembly> modAssemblies)
        {
            Debug.Log("Calling OnLoad methods");

            foreach (Assembly modAssembly in modAssemblies)
            {
                foreach (Type type in modAssembly.GetTypes())
                {
                    try
                    {
                        MethodInfo onLoad = type.GetMethod("OnLoad", new Type[0]);
                        onLoad?.Invoke(null, new object[0]);
                    }
                    catch (Exception e)
                    {
                        if (e is TargetInvocationException && e.InnerException != null)
                        {
                            e = e.InnerException;
                        }

                        string message = "OnLoad method failed for type " + type.FullName + " of mod " + modAssembly.GetName().Name;
                        Debug.LogError(message);
                        Debug.LogException(e);
                        throw new ModLoadingException(message + ExceptionToString(e));
                    }
                }
            }
        }

        private static string ExceptionToString(Exception ex)
        {
            return " - " + ex.GetType().Name + ": " + ex.Message;
        }
    }

    internal class ModLoadingException : Exception
    {

        internal ModLoadingException(String message) : base(message)
        {
        }

        internal ModLoadingException(String baseMessage, List<String> mods) : base(BuildMessage(baseMessage, mods))
        {
        }

        private static string BuildMessage(String baseMessage, List<String> mods)
        {
            return baseMessage + "\n" + String.Join("\n", mods.ToArray());
        }
    }
}
