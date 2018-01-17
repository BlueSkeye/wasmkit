using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class BlockInstruction : Instruction
    {
        private BlockInstruction()
            : base(OpCodes.Block)
        {
            return;
        }

        internal sbyte BlockType { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _perTypeBlock.Length); }
        }

        internal static BlockInstruction Create(BinaryParsingReader reader)
        {
            sbyte blockType = reader.ReadVarInt7();
            BlockInstruction result = _perTypeBlock[blockType + 128];

            if (null != result)
            {
                _reuseCount++;
            }
            else
            {
                result = new BlockInstruction() { BlockType = blockType };
                _perTypeBlock[blockType + 128] = result;
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
        private static BlockInstruction[] _perTypeBlock = new BlockInstruction[256];
    }
}
