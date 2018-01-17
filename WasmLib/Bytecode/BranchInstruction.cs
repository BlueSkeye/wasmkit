using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class BranchInstruction : Instruction
    {
        private BranchInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal bool Conditional { get; private set; }

        internal uint Depth { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _perDepthBranch.Length + _perDepthConditionalBranch.Length); }
        }

        internal static BranchInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint depth = reader.ReadVarUint32();
            bool conditional = (OpCodes.BrIf == opcode);

            if (MaxReusableDepth < depth)
            {
                return new BranchInstruction(opcode) { Conditional = conditional, Depth = depth };
            }
            BranchInstruction[] reuseTargets = conditional ? _perDepthBranch : _perDepthConditionalBranch;
            BranchInstruction result = reuseTargets[depth];

            if (null != result) { _reuseCount++; }
            else
            {
                result = new BranchInstruction(opcode) { Conditional = true, Depth = depth };
                reuseTargets[depth] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2} {1}", Depth, Conditional ? "(C)" : string.Empty);
        }

        internal override bool Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private const int MaxReusableDepth = 255;
        private static int _reuseCount = 0;
        private static BranchInstruction[] _perDepthBranch = new BranchInstruction[MaxReusableDepth + 1];
        private static BranchInstruction[] _perDepthConditionalBranch = new BranchInstruction[MaxReusableDepth + 1];
    }
}
