namespace Injector
{
    using Mono.Cecil;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public static class CecilHelper
    {
        public static FieldDefinition GetFieldDefinition(ModuleDefinition module, string typeName, string fieldName)
        {
            TypeDefinition type = GetTypeDefinition(module, typeName);

            return GetFieldDefinition(type, fieldName);
        }

        public static FieldDefinition GetFieldDefinition(TypeDefinition type, string fieldName)
        {
            return type.Fields.First(field => field.Name == fieldName);
        }

        public static MethodDefinition GetMethodDefinition(
        ModuleDefinition module,
        string           typeName,
        string           methodName,
        bool             useFullName = false)
        {
            TypeDefinition type = GetTypeDefinition(module, typeName, useFullName);

            return GetMethodDefinition(module, type, methodName);
        }

        public static MethodDefinition GetMethodDefinition(
        ModuleDefinition module,
        TypeDefinition   type,
        string           methodName)
        {
            return type.Methods.First(method => method.Name == methodName);
        }

        public static MethodReference GetMethodReference(ModuleDefinition targetModule, MethodDefinition method)
        {
            return targetModule.ImportReference(method);
        }

        public static AssemblyDefinition GetAssembly(string absoluteAssemblyPath)
        {
            ReaderParameters parameters = new ReaderParameters();
            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
            FileStream assemblyStream = new FileStream(absoluteAssemblyPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(absoluteAssemblyPath));
            parameters.AssemblyResolver = assemblyResolver;

            try
            {
                AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(assemblyStream, parameters);
                assemblyStream.Flush();
                assemblyStream.Close();

                return assemblyDef;
            }
            catch
            {
                Console.Error.WriteLine("Could not read assembly: \n");
                throw;
            }
        }

        public static AssemblyDefinition GetAssembly(string assemblyName, string directoryPath)
        {
            ReaderParameters parameters = new ReaderParameters();
            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
            FileStream assemblyStream = new FileStream(directoryPath + assemblyName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(directoryPath));
            parameters.AssemblyResolver = assemblyResolver;

            try
            {
                AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(assemblyStream, parameters);
                assemblyStream.Flush();
                assemblyStream.Close();

                return assemblyDef;
            }
            catch
            {
                Console.Error.WriteLine("Could not read assembly: \n");
                throw;
            }
        }

        public static ModuleDefinition GetModule(string moduleName, string directoryPath)
        {
            ReaderParameters parameters = new ReaderParameters();
            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();
            FileStream moduleStream = new FileStream(directoryPath + moduleName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(directoryPath));
            parameters.AssemblyResolver = assemblyResolver;

            try
            {
                Console.WriteLine(moduleName);
                ModuleDefinition moduleDef = ModuleDefinition.ReadModule(moduleStream, parameters);
                moduleStream.Flush();
                moduleStream.Close();

                return moduleDef;
            }
            catch(BadImageFormatException)
            {
                Console.Error.WriteLine(" === Patching old files not yet implemented === ");
                Console.Error.WriteLine($"Could not read module: {moduleName} \n");
                throw;
            }
            catch
            {
                Console.Error.WriteLine($"Could not read module: {moduleName} \n");
                throw;
            }
        }

        public static TypeDefinition GetTypeDefinition(
        ModuleDefinition module,
        string typeName,
        bool useFullName = false)
        {
            return module.Types.First(type => useFullName ? type.FullName == typeName : type.Name == typeName);
        }
    }
}