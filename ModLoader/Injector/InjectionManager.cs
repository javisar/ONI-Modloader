namespace Injector
{
    using global::Injector.IO;
    using Mono.Cecil;
    using ONI_Common.IO;
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
            string           path            = Directory.GetCurrentDirectory();
            ModuleDefinition onionModule     = CecilHelper.GetModule("ONI-Common.dll",                path);
            ModuleDefinition csharpModule    = CecilHelper.GetModule("Assembly-CSharp.dll",           path);
            ModuleDefinition firstPassModule = CecilHelper.GetModule("Assembly-CSharp-firstpass.dll", path);

            InjectorOnion injection = new InjectorOnion(onionModule, csharpModule, firstPassModule);
            injection.Inject();

            this.BackupAndSaveCSharpModule(csharpModule);
            this.BackupAndSaveFirstPassModule(firstPassModule);
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
            string pathUnityScript = GetTempPathForFile(filePath);

            if (this.TempForFileExists(filePath))
            {
                File.Delete(pathUnityScript);
            }

            File.Move(filePath, pathUnityScript);
            AssemblyDefinition aUnityScript = AssemblyDefinition.ReadAssembly(pathUnityScript);
            Injector.Inject(aUnityScript, filePath);

            File.Delete(pathUnityScript);
        }

        public bool RestoreBackupForFile(string filePath)
        {
            string backupPath   = GetBackupPathForFile(filePath);
            bool   backupExists = this.BackupForFileExists(filePath);
            bool   pathBlocked  = File.Exists(filePath);

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

        private void BackupAndSaveCSharpModule(ModuleDefinition module)
        {
            string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Assembly-CSharp.dll";

            this.MakeBackup(path);

            this.SaveModule(module, path);

            // Harmony & Co.
            this.PatchMod(path);
        }

        private void BackupAndSaveFirstPassModule(ModuleDefinition module)
        {
            string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar
                                                          + "Assembly-CSharp-firstpass.dll";
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