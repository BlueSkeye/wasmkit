using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class LocalAccessorInstruction : Instruction
    {
        private LocalAccessorInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
        }

        public uint LocalIndex { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownAccessors.Length); }
        }

        internal static LocalAccessorInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint localIndex = reader.ReadVarUint32();

            if (MaxReusablIndex < localIndex)
            {
                return new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
            }
            LocalAccessorInstruction result = _knownAccessors[localIndex];

            if (null != result) { _reuseCount++; }
            else
            {
                result = new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
                _knownAccessors[localIndex] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", LocalIndex);
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private static int MaxReusablIndex = 255;
        private static int _reuseCount = 0;
        private static LocalAccessorInstruction[] _knownAccessors = new LocalAccessorInstruction[MaxReusablIndex + 1];
    }
}
