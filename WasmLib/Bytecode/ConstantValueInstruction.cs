using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class ConstantValueInstruction<T> : Instruction
    {
        private ConstantValueInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownValues.Count); }
        }

        internal T Value { get; private set; }

        internal static ConstantValueInstruction<T> Create(BinaryParsingReader reader, OpCodes opcode, T value)
        {
            ConstantValueInstruction<T> result;

            if (_knownValues.TryGetValue(value, out result))
            {
                _reuseCount++;
                return result;
            }
            result = new ConstantValueInstruction<T>(opcode) { Value = value };
            _knownValues.Add(value, result);
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + " " + Value.ToString();
        }

        internal override bool Validate(Stack<sbyte> stack, ValidationContext context)
        {
            throw new NotImplementedException();
        }

        private static int _reuseCount = 0;
        private static SortedList<T, ConstantValueInstruction<T>> _knownValues = new SortedList<T, ConstantValueInstruction<T>>();
    }
}
