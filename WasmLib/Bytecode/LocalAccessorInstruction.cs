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
            get { return (float)_reuseCount / (float)(_reuseCount + _knownGetters.Length + _knownSetters.Length + _knownTees.Length); }
        }

        internal static LocalAccessorInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint localIndex = reader.ReadVarUint32();

            if (MaxReusablIndex < localIndex) {
                return new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
            }
            LocalAccessorInstruction[] reuseTable = GetReuseTable(opcode);
            LocalAccessorInstruction result = reuseTable[localIndex];

            if (null != result) { _reuseCount++; }
            else {
                result = new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
                reuseTable[localIndex] = result;
            }
            return result;
        }

        private static LocalAccessorInstruction[] GetReuseTable(OpCodes opcode)
        {
            switch (opcode) {
                case OpCodes.GetLocal:
                    return _knownGetters;
                case OpCodes.SetLocal:
                    return _knownSetters;
                case OpCodes.TeeLocal:
                    return _knownTees;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", LocalIndex);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType variableType = context.GetLocalVariableType(LocalIndex);
            if (0 == variableType) { return false; }
            BuiltinLanguageType topOfStackType;
            switch (OpCode) {
                case OpCodes.GetLocal:
                    context.StackPush(variableType);
                    return true;
                case OpCodes.SetLocal:
                    topOfStackType = context.StackPop();
                    if (0 == topOfStackType) { return false; }
                    if (topOfStackType != variableType) {
                        context.AddError(string.Format(
                            "Local variable setter type mismatch. On stack {0], variable type is {1}.",
                            topOfStackType, variableType));
                        return false;
                    }
                    return true;
                case OpCodes.TeeLocal:
                    topOfStackType = context.StackPeek(0);
                    if (0 == topOfStackType) { return false; }
                    if (topOfStackType != variableType) {
                        context.AddError(string.Format(
                            "Local variable tee type mismatch. On stack {0], variable type is {1}.",
                            topOfStackType, variableType));
                        return false;
                    }
                    return true;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static int MaxReusablIndex = 255;
        private static int _reuseCount = 0;
        private static LocalAccessorInstruction[] _knownGetters = new LocalAccessorInstruction[MaxReusablIndex + 1];
        private static LocalAccessorInstruction[] _knownSetters = new LocalAccessorInstruction[MaxReusablIndex + 1];
        private static LocalAccessorInstruction[] _knownTees = new LocalAccessorInstruction[MaxReusablIndex + 1];
    }
}
