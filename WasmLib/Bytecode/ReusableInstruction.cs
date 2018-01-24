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
            return;
        }

        internal override bool Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
