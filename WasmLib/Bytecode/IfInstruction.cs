using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class IfInstruction : Instruction
    {
        private IfInstruction()
            : base(OpCodes.If)
        {
            return;
        }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownTests.Length); }
        }

        internal sbyte ValueType { get; private set; }

        internal static IfInstruction Create(BinaryParsingReader reader)
        {
            sbyte valueType = reader.ReadValueType(true);
            IfInstruction result = _knownTests[valueType + 128];

            if (null != result) { _reuseCount++; }
            else
            {
                result = new IfInstruction() { ValueType = valueType };
                _knownTests[valueType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", ValueType);
        }

        internal override bool Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private static int _reuseCount = 0;
        private static IfInstruction[] _knownTests = new IfInstruction[256];
    }
}
