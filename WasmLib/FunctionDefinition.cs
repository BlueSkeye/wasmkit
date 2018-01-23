using System;
using System.Collections.Generic;

using WasmLib.Bytecode;

namespace WasmLib
{
    public class FunctionDefinition
    {
        internal FunctionDefinition(FunctionSignature signature)
        {
            Signature = signature ?? throw new ArgumentNullException("signature");
            Id = ++LastId;
        }

        internal BuiltinLanguageType[] Locals { get; private set; }

        /// <summary>A debug identifier. Not part of the binary format.</summary>
        internal int Id { get; private set; }

        internal FunctionSignature Signature { get; private set; }

        internal IEnumerable<Instruction> EnumerateInstructions()
        {
            foreach (Instruction instruction in _instructions) { yield return instruction; }
        }

        /// <summary>This method will assert the last instruction is an End, then remove it for later
        /// validation purpose.</summary>
        /// <param name="instructions">THe set of instructions to be bound to the function.</param>
        internal void SetBody(List<Instruction> instructions)
        {
            if ((null == instructions) || (0 == instructions.Count)) { throw new ArgumentNullException(); }
            int instructionsCount = instructions.Count;
            if (OpCodes.End != instructions[instructionsCount - 1].OpCode) {
                throw new WasmParsingException("End instruction missing at end of function.");
            }
            instructions.RemoveAt(instructionsCount - 1);
            _instructions = instructions;
        }

        internal void SetLocalTypes(BuiltinLanguageType[] types)
        {
            if (null != Locals) { throw new InvalidOperationException(); }
            Locals = types;
        }

        private static int LastId = 0;
        private List<Instruction> _instructions;
    }
}
