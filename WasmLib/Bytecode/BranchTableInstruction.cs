using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class BranchTableInstruction : Instruction
    {
        private BranchTableInstruction()
            : base(OpCodes.BrTable)
        {
            return;
        }

        internal uint DefaultTarget { get; private set; }

        internal uint[] Targets { get; private set; }

        internal static BranchTableInstruction Create(BinaryParsingReader reader)
        {
            uint targetCount = reader.ReadVarUint32();
            uint[] targets = new uint[targetCount];
            for (int index = 0; index < targetCount; index++)
            {
                targets[index] = reader.ReadVarUint32();
            }
            uint defaultTarget = reader.ReadVarUint32();
            return new BranchTableInstruction() { DefaultTarget = defaultTarget, Targets = targets };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", Targets);
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
