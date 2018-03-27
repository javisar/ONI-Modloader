namespace Injector
{
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using System.Linq;

    public class MethodInjector
    {
        private readonly ModuleDefinition _sourceModule;

        private readonly ModuleDefinition _targetModule;

        public MethodInjector(ModuleDefinition sourceModule, ModuleDefinition targetModule)
        {
            this._sourceModule = sourceModule;
            this._targetModule = targetModule;
        }

        /// <param name="passArgumentsByRef">
        /// Doesn't work on calling object
        /// </param>
        public void InjectAsFirst(
        string sourceTypeName,
        string sourceMethodName,
        string targetTypeName,
        string targetMethodName,
        bool   includeCallingObject = false,
        int    includeArgumentCount = 0,
        bool   passArgumentsByRef   = false)
        {
            this.InjectBefore(
                              sourceTypeName,
                              sourceMethodName,
                              targetTypeName,
                              targetMethodName,
                              0,
                              includeCallingObject,
                              includeArgumentCount,
                              passArgumentsByRef);
        }

        public void InjectAsFirst(
        string     sourceTypeName,
        string     sourceMethodName,
        MethodBody targetMethodBody,
        bool       includeCallingObject = false,
        int        includeArgumentCount = 0,
        bool       passArgumentsByRef   = false)
        {
            this.InjectBefore(
                              sourceTypeName,
                              sourceMethodName,
                              targetMethodBody,
                              targetMethodBody.Instructions.First(),
                              includeCallingObject,
                              includeArgumentCount,
                              passArgumentsByRef);
        }

        public void InjectBefore(
        string sourceTypeName,
        string sourceMethodName,
        string targetTypeName,
        string targetMethodName,
        int    instructionIndex,
        bool   includeCallingObject = false,
        int    includeArgumentCount = 0,
        bool   passArgumentsByRef   = false,
        bool   useFullName          = false)
        {
            MethodBody targetMethodBody = CecilHelper
                                         .GetMethodDefinition(
                                                              this._targetModule,
                                                              targetTypeName,
                                                              targetMethodName,
                                                              useFullName).Body;
            Instruction instruction = targetMethodBody.Instructions[instructionIndex];

            this.InjectBefore(
                              sourceTypeName,
                              sourceMethodName,
                              targetMethodBody,
                              instruction,
                              includeCallingObject,
                              includeArgumentCount,
                              passArgumentsByRef,
                              useFullName);
        }

        public void InjectBefore(
        string      sourceTypeName,
        string      sourceMethodName,
        MethodBody  targetMethodBody,
        Instruction targetInstruction,
        bool        includeCallingObject = false,
        int         includeArgumentCount = 0,
        bool        passArgumentsByRef   = false,
        bool        useFullName          = false)
        {
            MethodDefinition sourceMethod =
            CecilHelper.GetMethodDefinition(this._sourceModule, sourceTypeName, sourceMethodName, useFullName);
            MethodReference sourceMethodReference = CecilHelper.GetMethodReference(this._targetModule, sourceMethod);

            this.InjectBefore(
                              sourceMethodReference,
                              targetMethodBody,
                              targetInstruction,
                              includeCallingObject,
                              includeArgumentCount,
                              passArgumentsByRef);
        }

        public void InjectBefore(
        MethodReference sourceMethodReference,
        MethodBody      targetMethodBody,
        Instruction     targetInstruction,
        bool            includeCallingObject = false,
        int             includeArgumentCount = 0,
        bool            passArgumentsByRef   = false)
        {
            ILProcessor methodILProcessor = targetMethodBody.GetILProcessor();

            if (includeCallingObject)
            {
                IncludeCallingObject(targetInstruction, targetMethodBody);
            }

            if (includeArgumentCount > 0)
            {
                OpCode argumentOpCode = passArgumentsByRef ? OpCodes.Ldarga : OpCodes.Ldarg;

                for (int i = includeArgumentCount; i > 0; i--)
                {
                    Instruction argumentInstruction =
                    Instruction.Create(argumentOpCode, targetMethodBody.Method.Parameters[i - 1]);
                    methodILProcessor.InsertBefore(targetInstruction, argumentInstruction);
                }
            }

            methodILProcessor.InsertBefore(targetInstruction, Instruction.Create(OpCodes.Call, sourceMethodReference));
        }

        private static void IncludeCallingObject(Instruction nextInstruction, MethodBody methodBody)
        {
            Instruction thisInstruction = Instruction.Create(OpCodes.Ldarg_0);
            methodBody.GetILProcessor().InsertBefore(nextInstruction, thisInstruction);
        }

        private MethodBody GetMethodBody(string typeName, string methodName) =>
        CecilHelper.GetMethodDefinition(this._targetModule, typeName, methodName).Body;
    }
}