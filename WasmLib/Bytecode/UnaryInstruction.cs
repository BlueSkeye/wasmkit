using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class UnaryInstruction : Instruction
    {
        static UnaryInstruction()
        {
            RegisterKnownInstruction(OpCodes.F32abs, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32neg, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32ceil, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32floor, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32trunc, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32nearest, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32sqrt, BuiltinLanguageType.F32);

            RegisterKnownInstruction(OpCodes.F64abs, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64neg, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64ceil, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64floor, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64Trunc, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64nearest, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64sqrt, BuiltinLanguageType.F64);

            RegisterKnownInstruction(OpCodes.I32clz, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32ctz, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32popcnt, BuiltinLanguageType.I32);

            RegisterKnownInstruction(OpCodes.I64clz, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64ctz, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64popcnt, BuiltinLanguageType.I64);

            RegisterKnownInstruction(OpCodes.GrowMemory, BuiltinLanguageType.I32);
        }

        private UnaryInstruction(OpCodes opcode, BuiltinLanguageType argumentType)
            : base(opcode)
        {
            ArgumentType = argumentType;
        }

        internal BuiltinLanguageType ArgumentType { get; private set; }

        internal static UnaryInstruction GetInstruction(OpCodes opcode)
        {
            UnaryInstruction result = _knownInstructions[(int)opcode];
            if (null == result) { throw new ArgumentException(); }
            return result;
        }

        private static void RegisterKnownInstruction(OpCodes opcode, BuiltinLanguageType type)
        {
            _knownInstructions[(int)opcode] = new UnaryInstruction(opcode, type);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType argumentType = ArgumentType;
            // We don't pop because the result is from the same type than the argument. Thus we
            // avoid a pop/push.
            BuiltinLanguageType arg1type = context.StackPeek(0);
            if (0 == arg1type) { return false; }
            if (argumentType != arg1type) {
                context.AddError(string.Format(
                    "Invalid stack content for binary operator {0}. Stack item type {1}.",
                    OpCode, arg1type));
                return false;
            }
            return true;
        }

        private static UnaryInstruction[] _knownInstructions = new UnaryInstruction[256];
    }
}
