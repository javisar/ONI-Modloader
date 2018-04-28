namespace Injector
{
    using global::Injector.IO;
    using Mono.Cecil;
    using ONI_Common.IO;
    using System;
    using System.IO;

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
                ModuleDefinition onionModule = CecilHelper.GetModule("\\ONI-Common.dll", path);
                ModuleDefinition csharpModule = CecilHelper.GetModule("\\Assembly-CSharp.dll", path);
                ModuleDefinition firstPassModule = CecilHelper.GetModule("\\Assembly-CSharp-firstpass.dll", path);

                try
                {
                    InjectorOnion injection = new InjectorOnion(onionModule, csharpModule, firstPassModule);
                    injection.Inject();
                }
                catch
                {
                    Console.Error.WriteLine("Onion injector errored: \n");
                    throw;
                }
                try
                {
                    this.BackupAndSaveCSharpModule(csharpModule, path);
                    this.BackupAndSaveFirstPassModule(firstPassModule, path);
                }
                catch
                {
                    Console.Error.WriteLine("Backup errored: \n");
                    throw;
                }
            }
            catch
            {
                Console.Error.WriteLine("ModuleDefinition errored: \n");
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

        public void PatchMod(string filePath)
        {
            string tempDllPath = GetTempPathForFile(filePath);

            if (this.TempForFileExists(filePath))
            {
                File.Delete(tempDllPath);
            }
            File.Move(filePath, tempDllPath);

            AssemblyDefinition unityGameDll = CecilHelper.GetAssembly(tempDllPath);
            Injector.Inject(unityGameDll, filePath);

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
            path += Path.DirectorySeparatorChar+ module.ToString();

            this.MakeBackup(path);

            this.SaveModule(module, path);

            try
            {
                // Harmony & Co.
                this.PatchMod(path);
            }
            catch
            {
                Console.Error.WriteLine("Patching Assembly-CSharp.dll failed: ");
                throw;
            }
        }

        private void BackupAndSaveFirstPassModule(ModuleDefinition module, string path)
        {
            path += Path.DirectorySeparatorChar +module.ToString();

            this.MakeBackup(path);
            this.SaveModule(module, path);
        }

        // public bool IsCurrentAssemblyCSharpPatched()
        // => CecilHelper.GetModule(Paths.DefaultAssemblyCSharpPath, Paths.ManagedDirectoryPath).Types.Any(TypePatched);
        // public bool IsCurrentAssemblyFirstpassPatched()
        // => CecilHelper.GetModule(Paths.DefaultAssemblyFirstPassPath, Paths.ManagedDirectoryPath).Types.Any(TypePatched);
        // private static bool TypePatched(TypeDefinition type)
        // {
        // return type.Namespace == "Mods" && type.Name == "Patched";
        // }
    }
}