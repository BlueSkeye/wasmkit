using System;

namespace WasmLib.Bytecode
{
    internal class CallInstruction : Instruction
    {
        private CallInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal byte ExpectedSignature { get; private set; }

        internal bool Indirect { get; private set; }

        internal uint ItemIndex { get; private set; }

        internal static CallInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint itemIndex = reader.ReadVarUint32();
            bool indirect = (OpCodes.CallIndirect == opcode);
            byte expectedSIgnature = indirect ? reader.ReadVarUint1() : (byte)0;
            return new CallInstruction(opcode) { ExpectedSignature = expectedSIgnature, Indirect = indirect, ItemIndex = itemIndex };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X8} {1}", ItemIndex, Indirect ? "(I)" : string.Empty);
        }

        internal override bool Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
