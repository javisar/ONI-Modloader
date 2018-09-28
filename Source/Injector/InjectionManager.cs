namespace Injector
{
    using global::Injector.IO;
	using Mono.Cecil;
    using System;
    using System.IO;
	using System.Reflection;

	public class InjectionManager
    {
        public const string BackupString = ".orig";

        public const string tmpString = ".tmp";


        private readonly FileManager _fileManager;


        public InjectionManager(FileManager fileManager)
        {
            this._fileManager = fileManager;
        }

        public Logger Logger { get; set; }

        public bool BackupForFileExists(string filePath) => File.Exists(GetBackupPathForFile(filePath));

        public void InjectDefaultAndBackup()
        {
            string path = Directory.GetCurrentDirectory();
            try
            {
                //ModuleDefinition onionModule = CecilHelper.GetModule("\\ONI-Common.dll", path);
                ModuleDefinition csharpModule = CecilHelper.GetModule(Path.DirectorySeparatorChar+"Assembly-CSharp.dll", path);
                ModuleDefinition firstPassModule = CecilHelper.GetModule(Path.DirectorySeparatorChar + "Assembly-CSharp-firstpass.dll", path);
				
                try
                {
					//InjectorOnion injection = new InjectorOnion(onionModule, csharpModule, firstPassModule);
					//injection.Inject();

					//InjectPatchedSign(csharpModule, firstPassModule);
				}
                catch (Exception ex)
                {
					ModLogger.WriteLine(ConsoleColor.Red, "Onion injector errored: "+ex);
                    throw;
                }
				
                try
                {
                    this.BackupAndSaveCSharpModule(csharpModule, path);
                    this.BackupAndSaveFirstPassModule(firstPassModule, path);
                }
                catch (Exception ex)
                {
					ModLogger.WriteLine(ConsoleColor.Red, "Backup errored: " + ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
				ModLogger.WriteLine(ConsoleColor.Red, "ModuleDefinition errored: " +ex);
                throw;
            }
        }

        public void MakeBackup(string filePath)
        {
            string backupPath = GetBackupPathForFile(filePath);

            if (this.BackupForFileExists(filePath))
            {
                File.Delete(backupPath);
            }

            File.Move(filePath, backupPath);
        }

        public void PatchMod(ModuleDefinition module, string filePath, string className, string methodName)
        {

            AssemblyDefinition originalDll = CecilHelper.GetAssembly(filePath);
            if (Injector.IsPatched(originalDll))
            {
                ModLogger.WriteLine(ConsoleColor.Red, module.Name + " already patched.");
                return;
            }

            this.MakeBackup(filePath);
            this.SaveModule(module, filePath);

            string tempDllPath = GetTempPathForFile(filePath);

            if (this.TempForFileExists(filePath))
            {
                File.Delete(tempDllPath);
            }
            File.Move(filePath, tempDllPath);

            AssemblyDefinition unityGameDll = CecilHelper.GetAssembly(tempDllPath);
            Injector.Inject(ref unityGameDll, className, methodName);
            Injector.InjectPatchedSign(ref unityGameDll);
            unityGameDll.Write(filePath);

            File.Delete(tempDllPath);
        }

        public bool RestoreBackupForFile(string filePath)
        {
            string backupPath = GetBackupPathForFile(filePath);
            bool backupExists = this.BackupForFileExists(filePath);
            bool pathBlocked = File.Exists(filePath);

            if (!backupExists)
            {
                return false;
            }

            if (pathBlocked)
            {
                File.Delete(filePath);
            }

            File.Move(backupPath, filePath);

            return true;
        }

        public void SaveModule(ModuleDefinition module, string filePath)
        {
            module.Write(filePath);
        }

        public bool TempForFileExists(string filePath) => File.Exists(GetBackupPathForFile(filePath));

        private static string GetBackupPathForFile(string filePath) => filePath + BackupString;

        private static string GetTempPathForFile(string filePath) => filePath + tmpString;

        private void BackupAndSaveCSharpModule(ModuleDefinition module, string path)
        {
            ModLogger.WriteLine(ConsoleColor.Green, "Applying patch... "+ module.ToString());
            path += Path.DirectorySeparatorChar+ module.ToString();
            
			try
			{
				// Harmony & Co.
				
				//FieldInfo fi = typeof(LaunchInitializer).GetField("BUILD_PREFIX", BindingFlags.Public | BindingFlags.Static);
				//string upgradeS = ((string)fi.GetValue(null));
               
                this.PatchMod(module, path, "LaunchInitializer", "Update");
            }
			catch (Exception ex)
			{
				ModLogger.WriteLine(ConsoleColor.Red, "Patching Assembly-CSharp.dll failed: " + ex);
				throw;
			}
			
        }

        private void BackupAndSaveFirstPassModule(ModuleDefinition module, string path)
        {
            ModLogger.WriteLine(ConsoleColor.Green, "Applying patch... " + module.ToString());
            path += Path.DirectorySeparatorChar +module.ToString();          
			
			try
			{
                // Harmony & Co.
                this.PatchMod(module, path, "App", "Awake");
			}
			catch (Exception ex)
			{
				ModLogger.WriteLine(ConsoleColor.Red, "Patching Assembly-CSharp.dll failed: " + ex);
				throw;
			}
			
		}

		// public bool IsCurrentAssemblyCSharpPatched()
		// => CecilHelper.GetModule(Paths.DefaultAssemblyCSharpPath, Paths.ManagedDirectoryPath).Types.Any(TypePatched);
		// public bool IsCurrentAssemblyFirstpassPatched()
		// => CecilHelper.GetModule(Paths.DefaultAssemblyFirstPassPath, Paths.ManagedDirectoryPath).Types.Any(TypePatched);
		// private static bool TypePatched(TypeDefinition type)
		// {
		// return type.Namespace == "Mods" && type.Name == "Patched";
		// }

		private void InjectPatchedSign(ModuleDefinition _csharpModule, ModuleDefinition _firstPassModule)
		{
			_csharpModule.Types.Add(
										 new TypeDefinition(
															"Mods",
															"Patched",
															Mono.Cecil.TypeAttributes.Class,
															_csharpModule.TypeSystem.Object));
			_firstPassModule.Types.Add(
											new TypeDefinition(
															   "Mods",
															   "Patched",
															   Mono.Cecil.TypeAttributes.Class,
															   _firstPassModule.TypeSystem.Object));
		}
	}
}