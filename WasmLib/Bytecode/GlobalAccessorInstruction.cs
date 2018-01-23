
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

        public bool IsGetter
        {
            get { return OpCodes.GetGlobal == OpCode; }
        }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownGlobalGetters.Length + _knownGlobalSetters.Length); }
        }

        internal static GlobalAccessorInstruction Create(BinaryParsingReader reader, bool getter)
        {
            uint globalIndex = reader.ReadVarUint32();
            OpCodes opcode = getter ? OpCodes.GetGlobal : OpCodes.SetGlobal;

            if (MaxReusableIndex < globalIndex) {
                return new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
            }
            GlobalAccessorInstruction[] reuseTarget = getter ? _knownGlobalGetters : _knownGlobalSetters;
            GlobalAccessorInstruction result = reuseTarget[globalIndex];

            if (null != result) { _reuseCount++; }
            else {
                result = new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
                reuseTarget[globalIndex] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", GlobalIndex);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType globalVariableType = context.GetGlobalVariableType(GlobalIndex);
            if (0 == globalVariableType) { return false; }
            if (IsGetter) {
                context.StackPush(globalVariableType);
                return true;
            }
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { context.AddError("Attempt to pop an empty stack."); }
            if (globalVariableType != poppedType) {
                context.AddError(string.Format("Attempt to set a {0} global variable with a {1} value.",
                    globalVariableType, poppedType));
                return false;
            }
            return true;
        }

        private const int MaxReusableIndex = 1024;
        private static int _reuseCount = 0;
        private static GlobalAccessorInstruction[] _knownGlobalGetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
        private static GlobalAccessorInstruction[] _knownGlobalSetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
    }
}
