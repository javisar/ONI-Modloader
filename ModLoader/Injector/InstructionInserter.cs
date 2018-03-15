using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;
using MethodBody = Mono.Cecil.Cil.MethodBody;

namespace Injector
{
    public class InstructionInserter
    {
        public InstructionInserter(MethodDefinition targetMethod) : this(targetMethod.Body.GetILProcessor()) { }

        public InstructionInserter(MethodBody targetMethodBody) : this(targetMethodBody.GetILProcessor()) { }

        public InstructionInserter(ILProcessor targetMethodILProcessor)
        {
            this._ilProcessor = targetMethodILProcessor;
        }

        private readonly ILProcessor _ilProcessor;

        public void InsertBefore(Instruction targetInstruction, IEnumerable<Instruction> instructionsToInsert)
        {
            foreach (Instruction newInstruction in instructionsToInsert)
            {
                this._ilProcessor.InsertBefore(targetInstruction, newInstruction);
            }
        }

        public void InsertBefore(Instruction targetInstruction, Instruction instructionToInsert)
        {
            this._ilProcessor.InsertBefore(targetInstruction, instructionToInsert);
        }

        public void InsertAfter(Instruction targetInstruction, IEnumerable<Instruction> instructionsToInsert)
        {
            IEnumerable<Instruction> reversedInstructions = instructionsToInsert.Reverse();

            foreach (Instruction newInstruction in reversedInstructions)
            {
                this._ilProcessor.InsertAfter(targetInstruction, newInstruction);
            }
        }

        public void InsertAfter(int instructionIndex, Instruction instructionToInsert)
        {
            this.InsertAfter(this._ilProcessor.Body.Instructions[instructionIndex], instructionToInsert);
        }

        public void InsertAfter(Instruction targetInstruction, Instruction instructionToInsert)
        {
            this._ilProcessor.InsertAfter(targetInstruction, instructionToInsert);
        }
    }
}
