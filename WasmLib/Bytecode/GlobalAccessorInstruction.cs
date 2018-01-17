using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class GlobalAccessorInstruction : Instruction
    {
        private GlobalAccessorInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
        }

        public uint GlobalIndex { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownGlobalGetters.Length + _knownGlobalSetters.Length); }
        }

        internal static GlobalAccessorInstruction Create(BinaryParsingReader reader, bool getter)
        {
            uint globalIndex = reader.ReadVarUint32();
            OpCodes opcode = getter ? OpCodes.GetGlobal : OpCodes.SetGlobal;

            if (MaxReusableIndex < globalIndex)
            {
                return new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
            }

            GlobalAccessorInstruction[] reusableAccessors = getter ? _knownGlobalGetters : _knownGlobalSetters;
            GlobalAccessorInstruction result = reusableAccessors[globalIndex];

            if (null != result) { _reuseCount++; }
            else
            {
                result = new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
                reusableAccessors[globalIndex] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", GlobalIndex);
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private const int MaxReusableIndex = 1024;
        private static int _reuseCount = 0;
        private static GlobalAccessorInstruction[] _knownGlobalGetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
        private static GlobalAccessorInstruction[] _knownGlobalSetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
    }
}
