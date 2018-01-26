using System;

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
