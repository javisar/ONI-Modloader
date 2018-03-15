using Mono.Cecil;

namespace Injector
{
    public class Publisher
    {
        public Publisher(ModuleDefinition targetModule)
        {
            this._targetModule = targetModule;
        }

        private readonly ModuleDefinition _targetModule;

        public void MakeFieldPublic(string typeName, string fieldName)
        {
            FieldDefinition field = CecilHelper.GetFieldDefinition(this._targetModule, typeName, fieldName);

            field.IsPublic = true;
        }

        public void MakeMethodPublic(string typeName, string methodName)
        {
            MethodDefinition method = CecilHelper.GetMethodDefinition(this._targetModule, typeName, methodName);

            method.IsPublic = true;
        }
    }
}
