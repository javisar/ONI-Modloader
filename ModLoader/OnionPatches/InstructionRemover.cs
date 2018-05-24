namespace OnionPatches
{
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using System.Linq;

    public class InstructionRemover
    {
        private readonly ModuleDefinition _targetModule;

        public InstructionRemover(ModuleDefinition targetModule)
        {
            this._targetModule = targetModule;
        }

        public void ClearAllButLast(string typeName, string methodName)
        {
            MethodDefinition method     = CecilHelper.GetMethodDefinition(this._targetModule, typeName, methodName);
            MethodBody       methodBody = method.Body;

            this.ClearAllButLast(methodBody);
        }

        public void ClearAllButLast(MethodBody methodBody)
        {
            ILProcessor methodILProcessor = methodBody.GetILProcessor();

            for (int i = methodBody.Instructions.Count - 1; i > 0; i--)
            {
                methodILProcessor.Remove(methodBody.Instructions.First());
            }
        }

        public void ReplaceByNop(MethodBody methodBody, Instruction instruction)
        {
            methodBody.GetILProcessor().Replace(instruction, Instruction.Create(OpCodes.Nop));
        }

        public void ReplaceByNopAt(string typeName, string methodName, int instructionIndex)
        {
            MethodDefinition method     = CecilHelper.GetMethodDefinition(this._targetModule, typeName, methodName);
            MethodBody       methodBody = method.Body;

            this.ReplaceByNop(methodBody, methodBody.Instructions[instructionIndex]);
        }
    }
}