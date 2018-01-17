using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class MemoryControlInstruction : Instruction
    {
        private MemoryControlInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal static uint UseCount { get; private set; }

        internal byte Reserved { get; private set; }

        internal static MemoryControlInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            byte reserved = reader.ReadVarUint1();
            UseCount++;
            return new MemoryControlInstruction(opcode) { Reserved = reserved };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", Reserved);
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
