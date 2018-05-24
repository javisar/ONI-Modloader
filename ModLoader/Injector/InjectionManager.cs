namespace Injector
{
    using global::Injector.IO;
    using Mono.Cecil;
	using Mono.Cecil.Cil;
	using Mono.Collections.Generic;
	using System;
	using System.IO;
	using System.Linq;

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
            //ModuleDefinition onionModule     = CecilHelper.GetModule("ONI-Common.dll",                path);
            ModuleDefinition csharpModule    = CecilHelper.GetModule("Assembly-CSharp.dll",           path);
            ModuleDefinition firstPassModule = CecilHelper.GetModule("Assembly-CSharp-firstpass.dll", path);

			//InjectorOnion injection = new InjectorOnion(onionModule, csharpModule, firstPassModule);
			//injection.Inject();
			InjectPatchedSign(csharpModule, firstPassModule);
			FixGameUpdateExceptionHandling(csharpModule);

			this.BackupAndSaveCSharpModule(csharpModule);
            this.BackupAndSaveFirstPassModule(firstPassModule);
        }


		private void FixGameUpdateExceptionHandling(ModuleDefinition _csharpModule)
		{
			ExceptionHandler handler = new ExceptionHandler(ExceptionHandlerType.Finally);
			MethodBody methodBody =
			CecilHelper.GetMethodDefinition(_csharpModule, "Game", "Update").Body;
			Collection<Instruction> methodInstructions = methodBody.Instructions;

			handler.TryStart = methodInstructions.First(instruction => instruction.OpCode == OpCodes.Ldsfld);

			Instruction tryEndInstruction =
			methodInstructions.Last(instruction => instruction.OpCode == OpCodes.Ldloca_S);

			handler.TryEnd = tryEndInstruction;
			handler.HandlerStart = tryEndInstruction;
			handler.HandlerEnd = methodInstructions.Last();
			handler.CatchType = _csharpModule.ImportReference(typeof(Exception));

			methodBody.ExceptionHandlers.Clear();
			methodBody.ExceptionHandlers.Add(handler);
		}

		private void InjectPatchedSign(ModuleDefinition _csharpModule, ModuleDefinition _firstPassModule)
		{
			_csharpModule.Types.Add(
										 new TypeDefinition(
															"Mods",
															"Patched",
															TypeAttributes.Class,
															_csharpModule.TypeSystem.Object));
			_firstPassModule.Types.Add(
											new TypeDefinition(
															   "Mods",
															   "Patched",
															   TypeAttributes.Class,
															   _firstPassModule.TypeSystem.Object));
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