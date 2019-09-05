namespace ModLoader
{
    using Harmony;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public static class ModLoader
    {
		public const string ModLoaderVersion = "v0.5.2";

		//public const string AssemblyDir = "Assemblies";

        internal static string failureMessage = string.Empty;

        internal static bool HasFailed => failureMessage != string.Empty;

        public static void Start()
        {
			ModLogger.WriteLine("Initializing Modloader "+ModLoaderVersion);

			// Patch in Mod Loader helpers
			HarmonyInstance.Create("ONI-ModLoader")?.PatchAll(Assembly.GetExecutingAssembly());

            // Load mods
            DirectoryInfo modsDir = GetModsDirectory();
            /*
            List<FileInfo> files = modsDir.GetFiles("*.dll").ToList();            
            foreach (DirectoryInfo modDirectory in modsDir.GetDirectories())
            {
                DirectoryInfo[] sub = modDirectory?.GetDirectories();
                DirectoryInfo assmeblies = sub?.FirstOrDefault(x => x != null && x.Name.Contains(AssemblyDir));                
                if (assmeblies != null)
                {
                    foreach (FileInfo file in assmeblies.GetFiles("*.dll"))
                    {
                        files.Add(file);
                    }
                }
            }
            */

            if (modsDir.Exists)
            {
                string[] pathFiles = Directory.GetFiles(modsDir.FullName, "*.dll", SearchOption.AllDirectories);

                List<FileInfo> files = new List<FileInfo>();
                foreach (var file in pathFiles)
                {
                    files.Add(new FileInfo(file));
                }

                try
                {
                    DependencyGraph dependencyGraph = PreloadModAssemblies(files);
                    List<Assembly> sortedAssemblies = dependencyGraph?.TopologicalSort();
                    List<Assembly> loadableMods = CheckForDependences(sortedAssemblies);
                    List<Assembly> loadedMods = ApplyHarmonyPatches(loadableMods);
                    CallOnLoadMethods(loadedMods);

                    ModLogger.WriteLine("All mods successfully loaded!");
                }
                catch (ModLoadingException mle)
                {
                    failureMessage = mle.Message;
                }
            }
        }

        private static List<Assembly> CheckForDependences(List<Assembly> modAssemblies)
        {
            ModLogger.WriteLine("CheckForDependences");
            List<string> failedMods = new List<string>();
            List<Assembly> loadableMods = new List<Assembly>();

            foreach (Assembly modAssembly in modAssemblies)
            {
                if (modAssembly == null)
                {
                    continue;
                }

                try
                {
                    ModLogger.WriteLine("\tChecking " + modAssembly.FullName);
                    
                    bool correct = true;
                    foreach (AssemblyName referenced in modAssembly.GetReferencedAssemblies())
                    {
                        ModLogger.WriteLine("\t\t" + referenced.FullName);
                        try
                        {
                            Assembly dependence = Assembly.ReflectionOnlyLoad(referenced.FullName);
                            if (dependence == null)
                            {
                                correct = false;
                                ModLogger.Error.WriteLine("\tCannot find dependence: " + referenced.FullName);

                                continue;
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            ModLogger.WriteLine("\t\t\tChecking embedded dependences:");
                            correct = CheckForEmbeddedDependence(referenced.Name, modAssembly);
                            if (!correct)
                            {
                                ModLogger.Error.WriteLine("\tCannot find dependence: " + referenced.FullName);
                                ModLogger.Error.WriteLine(ex);
                                continue;
                            }
                            
                        }
                    }

                    if (correct)
                    {                        
                        loadableMods.Add(modAssembly);
                    }
                    else
                    {
                        failedMods.Add(modAssembly.GetName().ToString());
                    }

                }
                catch (Exception e)
                {
                    failedMods.Add(modAssembly.GetName() + ExceptionToString(e));
                    ModLogger.Error.WriteLine("Patching mod " + modAssembly.GetName() + " failed!");
                    ModLogger.Error.WriteLine(e);
                }
            }
            return loadableMods;
        }

        private static bool CheckForEmbeddedDependence(string name, Assembly modAssembly)
        {
            // Load embedded resources to check inner dependences
            string[] resources = modAssembly.GetManifestResourceNames();
            foreach (string resource in resources)
            {
                ModLogger.WriteLine("\t\t\t" + resource);
                if (resource.Contains(name))
                    return true;               
            }
            return false;
        }

        private static List<Assembly> ApplyHarmonyPatches([NotNull] List<Assembly> modAssemblies)
        {
            ModLogger.WriteLine("Applying Harmony patches");
            List<string> failedMods = new List<string>();
            List<Assembly> loadedMods = new List<Assembly>();

            foreach (Assembly modAssembly in modAssemblies)
            {
                if (modAssembly == null)
                {
                    continue;
                }

                try
                {
                    ModLogger.WriteLine("Loading: " + modAssembly.FullName);
                    Assembly loaded = Assembly.LoadFrom(modAssembly.Location);
                    HarmonyInstance.Create(loaded.FullName)?.PatchAll(loaded);
                    loadedMods.Add(loaded);

                }
                catch (Exception e)
                {
                    failedMods.Add(modAssembly.GetName() + ExceptionToString(e));
                    ModLogger.Error.WriteLine("Patching mod " + modAssembly.GetName() + " failed!");
                    ModLogger.Error.WriteLine(e);
                }
            }

            if (failedMods.Count > 0)
            {
                //throw new ModLoadingException("The following mods could not be patched:", failedMods);
                string message = "\nThe following mods could not be patched:\n" + String.Join("\n\t", failedMods.ToArray());
                ModLogger.Error.WriteLine(message);
                /*
                MainMenu mainMenus = (MainMenu)UnityEngine.Object.FindObjectOfType(typeof(MainMenu));


                ConfirmDialogScreen confirmDialogScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, mainMenus.gameObject, force_active: true);
                confirmDialogScreen.PopupConfirmDialog(message, null, null);
                */
            }
            return loadedMods;
        }

        private static void CallOnLoadMethods([NotNull] List<Assembly> modAssemblies)
        {
            ModLogger.WriteLine("Calling OnLoad methods");

            foreach (Assembly modAssembly in modAssemblies)
            {
                if (modAssembly == null)
                {
                    continue;
                }

                foreach (Type type in modAssembly.GetTypes())
                {
                    if (type == null)
                    {
                        continue;
                    }

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

                        string message = "OnLoad method failed for type " + type.FullName + " of mod "
                                       + modAssembly.GetName().Name;
                        ModLogger.Error.WriteLine(message);
                        ModLogger.Error.WriteLine(e);
                        throw new ModLoadingException(message + ExceptionToString(e));
                    }
                }
            }
        }

        private static string ExceptionToString([NotNull] Exception ex)
        {
            return " - " + ex.GetType().Name + ": " + ex.Message;
        }

        public static DirectoryInfo GetModsDirectory()
        {
            DirectoryInfo dataDir = new DirectoryInfo(Application.dataPath);
            ModLogger.WriteLine("Data dir: " + dataDir.FullName);
            ModLogger.WriteLine("RuntimePlatform: " + Application.platform);

            DirectoryInfo oniBaseDirectory;
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                oniBaseDirectory = new DirectoryInfo(Path.Combine(dataDir.FullName, "Resources"));
            }
			else if (Application.platform == RuntimePlatform.LinuxPlayer)
			{
				oniBaseDirectory = dataDir.Parent;
			}
			else
            {
                oniBaseDirectory = dataDir.Parent;
            }

            ModLogger.WriteLine("Path to mods is: " + Path.Combine(oniBaseDirectory?.FullName, "Mods"));
            return new DirectoryInfo(Path.Combine(oniBaseDirectory?.FullName, "Mods"));
        }

        private static DependencyGraph PreloadModAssemblies(List<FileInfo> assemblyFiles)
        {
            ModLogger.WriteLine("Loading mod assemblies");
            List<Assembly> loadedAssemblies = new List<Assembly>();
            List<string> failedAssemblies = new List<string>();

            if (assemblyFiles != null)
            {
                foreach (FileInfo file in assemblyFiles)
                {
                    if (file?.Extension != ".dll")
                    {
                        // GetFiles filter is too inclusive
                        continue;
                    }

                    if (file.Name.ToLower() == "0harmony" || file.Name.ToLower() == "modloader"
                    )
                    {
                        // GetFiles filter is too inclusive
                        continue;
                    }

                    try
                    {
                        //Assembly modAssembly = Assembly.LoadFrom(file.FullName);
                        Assembly modAssembly = Assembly.ReflectionOnlyLoadFrom(file.FullName);
                        if (loadedAssemblies.Contains(modAssembly))
                        {
                            ModLogger.WriteLine("Skipping duplicate mod " + modAssembly.FullName);
                        }
                        else
                        {
                            ModLogger.WriteLine("Preloading " + modAssembly.FullName);
                            loadedAssemblies.Add(modAssembly);
                        }
                    }
                    catch (Exception e)
                    {
                        failedAssemblies.Add(file.Name + ExceptionToString(e));
                        ModLogger.Error.WriteLine("Preloading mod " + file.Name + " failed!");
                        ModLogger.Error.WriteLine(e);
                    }
                }
            }

            if (failedAssemblies.Count > 0)
            {
                throw new ModLoadingException("The following mods could not be preloaded:", failedAssemblies);
            }

            return new DependencyGraph(loadedAssemblies);
        }
    }

    public class ModInfo
    {
        public string modName;

        // public List<>
    }

    internal class ModLoadingException : Exception
    {
        internal ModLoadingException(string message)
        : base(message)
        {
        }

        internal ModLoadingException(string baseMessage, List<string> mods)
        : base(BuildMessage(baseMessage, mods))
        {
        }

        private static string BuildMessage(string baseMessage, [NotNull] List<string> mods)
        {
            return baseMessage + "\n" + string.Join("\n", mods.ToArray());
        }
    }
}