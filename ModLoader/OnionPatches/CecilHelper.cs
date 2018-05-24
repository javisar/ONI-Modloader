namespace OnionPatches
{
    using Mono.Cecil;
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

        public static ModuleDefinition GetModule(string moduleName, string directoryPath)
        {
            ReaderParameters        parameters       = new ReaderParameters();
            DefaultAssemblyResolver assemblyResolver = new DefaultAssemblyResolver();

            assemblyResolver.AddSearchDirectory(directoryPath);
            parameters.AssemblyResolver = assemblyResolver;

            return ModuleDefinition.ReadModule(moduleName, parameters);
        }

        public static TypeDefinition GetTypeDefinition(
        ModuleDefinition module,
        string           typeName,
        bool             useFullName = false)
        {
            return module.Types.First(type => useFullName ? type.FullName == typeName : type.Name == typeName);
        }
    }
}