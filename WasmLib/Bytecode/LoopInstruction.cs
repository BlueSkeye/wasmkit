using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class LoopInstruction : Instruction
    {
        private LoopInstruction()
            : base(OpCodes.Loop)
        {
            return;
        }

        internal sbyte BlockType { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownLoops.Length); }
        }

        internal static LoopInstruction Create(BinaryParsingReader reader)
        {
            sbyte blockType = reader.ReadVarInt7();
            LoopInstruction result = _knownLoops[blockType + 128];

            if (null != result) { _reuseCount++; }
            else
            {
                result = new LoopInstruction() { BlockType = blockType };
                _knownLoops[blockType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", BlockType);
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private static int _reuseCount = 0;
        private static LoopInstruction[] _knownLoops = new LoopInstruction[256];
    }
}
