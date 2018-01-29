using System;

namespace WasmLib.Bytecode
{
    internal class IntegerTestInstruction : Instruction
    {
        private IntegerTestInstruction(OpCodes opcode)
            : base(opcode)
        {
        }

        internal static IntegerTestInstruction Create(OpCodes opcode)
        {
            switch (opcode) {
                case OpCodes.I32eqz:
                    return _i32ComparerInstruction;
                case OpCodes.I64eqz:
                    return _i64ComparerInstruction;
                default:
                    throw new ArgumentException();
            }
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType expectedType;

            switch (OpCode) {
                case OpCodes.I32eqz:
                    expectedType = BuiltinLanguageType.I32;
                    break;
                case OpCodes.I64eqz:
                    expectedType = BuiltinLanguageType.I64;
                    break;
                default:
                    throw new ArgumentException();
            }
            BuiltinLanguageType stackType = context.StackPeek(0);
            if (0 == stackType) { return false; }
            if (stackType != expectedType) {
                context.AddError(string.Format("Unexpected {0} type on stack. Was expected {1})",
                    stackType, expectedType));
                return false;
            }
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            context.StackPush(BuiltinLanguageType.I32);
            return true;
        }

        private static readonly IntegerTestInstruction _i32ComparerInstruction = new IntegerTestInstruction(OpCodes.I32eqz);
        private static readonly IntegerTestInstruction _i64ComparerInstruction = new IntegerTestInstruction(OpCodes.I64eqz);
    }
}
