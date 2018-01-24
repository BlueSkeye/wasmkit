using System;

namespace WasmLib.Bytecode
{
    internal class LoopInstruction : Instruction
    {
        private LoopInstruction()
            : base(OpCodes.Loop)
        {
            return;
        }

        internal BuiltinLanguageType BlockType { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownLoops.Length); }
        }

        internal static LoopInstruction Create(BinaryParsingReader reader)
        {
            BuiltinLanguageType blockType = (BuiltinLanguageType)reader.ReadVarInt7();
            LoopInstruction result = _knownLoops[(int)blockType + 128];

            if (null != result) { _reuseCount++; }
            else {
                result = new LoopInstruction() { BlockType = blockType };
                _knownLoops[(int)blockType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + " " + BlockType.ToString();
        }

        internal override bool Validate(ValidationContext context)
        {
            context.CreateLabel(BlockType, false);
            return true;
        }

        private static int _reuseCount = 0;
        /// <summary>These instructions can be reused between modules because they don't
        /// hold any module dependent information.</summary>
        private static LoopInstruction[] _knownLoops = new LoopInstruction[256];
    }
}
