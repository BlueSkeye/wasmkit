using System;
using System.Collections.Generic;

using WasmLib.Bytecode;

namespace WasmLib
{
    public class FunctionDefinition
    {
        internal FunctionDefinition(FunctionSignature signature)
        {
            _signature = signature ?? throw new ArgumentNullException("signature");
            Id = ++LastId;
        }

        /// <summary>A debug identifier. Not part of the binary format.</summary>
        internal int Id { get; private set; }

        internal IEnumerable<Instruction> EnumerateInstructions()
        {
            foreach (Instruction instruction in _instructions) { yield return instruction; }
        }

        internal void SetBody(List<Instruction> instructions)
        {
            if ((null == instructions) || (0 == instructions.Count)) {
                throw new ArgumentNullException();
            }
            _instructions = instructions;
        }

        private static int LastId = 0;
        private List<Instruction> _instructions;
        private FunctionSignature _signature;
    }
}
