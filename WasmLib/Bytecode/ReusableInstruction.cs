using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class ReusableInstruction : Instruction
    {
        internal ReusableInstruction(OpCodes opcode)
            : base(opcode)
        {
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
