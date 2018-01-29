using System;

namespace WasmLib.Bytecode
{
    internal class BinaryInstruction : Instruction
    {
        static BinaryInstruction()
        {
            RegisterKnownInstruction(OpCodes.I32add, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32sub, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32mul, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32div_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32div_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32rem_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32rem_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32and, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32or, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32xor, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32shl, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32shr_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32shr_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32rotl, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32rotr, BuiltinLanguageType.I32);

            RegisterKnownInstruction(OpCodes.I64add, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64sub, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64mul, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64div_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64div_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64rem_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64rem_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64and, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64or, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64xor, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64shl, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64shr_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64shr_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64rotl, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64rotr, BuiltinLanguageType.I64);

            RegisterKnownInstruction(OpCodes.F32add, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32sub, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32mul, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32div, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32min, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32max, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32copysign, BuiltinLanguageType.F32);

            RegisterKnownInstruction(OpCodes.F64add, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64sub, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64mul, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64div, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64min, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64max, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64copysign, BuiltinLanguageType.F64);
        }

        private BinaryInstruction(OpCodes opcode, BuiltinLanguageType argumentType)
            : base(opcode)
        {
            ArgumentType = argumentType;
        }

        internal BuiltinLanguageType ArgumentType { get; private set; }

        internal static BinaryInstruction GetInstruction(OpCodes opcode)
        {
            BinaryInstruction result = _knownInstructions[(int)opcode];
            if (null == result) { throw new ArgumentException(); }
            return result;
        }

        private static void RegisterKnownInstruction(OpCodes opcode, BuiltinLanguageType type)
        {
            _knownInstructions[(int)opcode] = new BinaryInstruction(opcode, type);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType resultType = GetResultType();
            if (!context.ValidatePopableItemsCount(2)) { return false; }
            BuiltinLanguageType arg1type = context.StackPeek(0);
            if (0 == arg1type) { return false; }
            BuiltinLanguageType arg2type = context.StackPeek(1);
            if (0 == arg2type) { return false; }
            if ((resultType != arg1type) || (resultType != arg2type)) {
                context.AddError(string.Format(
                    "Invalid stack content for binary operator {0}. Stack item types {1}, {2}.",
                    OpCode, arg1type, arg2type));
                return false;
            }
            // We pop a single one because both arguments are of the same type that the result, so we save
            // one pop/push here.
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            return true;
        }

        private static BinaryInstruction[] _knownInstructions = new BinaryInstruction[256];
    }
}
