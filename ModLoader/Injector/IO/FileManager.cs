namespace Injector.IO
{
    using Mono.Cecil;
    using System.IO;

    public class FileManager
    {
        public const string BackupString = ".backup";

        public bool BackupForFileExists(string filePath) => File.Exists(GetBackupPathForFile(filePath));

        public void MakeBackup(string filePath)
        {
            string backupPath = GetBackupPathForFile(filePath);

            if (this.BackupForFileExists(filePath))
            {
                File.Delete(backupPath);
            }

            File.Move(filePath, backupPath);
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

        private static string GetBackupPathForFile(string filePath) => filePath + BackupString;
    }
}