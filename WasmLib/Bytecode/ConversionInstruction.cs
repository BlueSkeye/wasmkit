using System;

namespace WasmLib.Bytecode
{
    internal class ConversionInstruction : Instruction
    {
        static ConversionInstruction()
        {
            RegisterKnownInstruction(OpCodes.F32Convert_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32Convert_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32Convert_sI64, BuiltinLanguageType.I64, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32Convert_uI64, BuiltinLanguageType.I64, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32demoteF64, BuiltinLanguageType.F64, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F64Convert_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64Convert_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64Convert_sI64, BuiltinLanguageType.I64, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64Convert_uI64, BuiltinLanguageType.I64, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64PromoteF32, BuiltinLanguageType.F32, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.I32Trunc_sF32, BuiltinLanguageType.F32, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32Trunc_uF32, BuiltinLanguageType.F32, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32Trunc_sF64, BuiltinLanguageType.F64, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32Trunc_uF64, BuiltinLanguageType.F64, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32wrapI64, BuiltinLanguageType.I64, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I64Extend_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64Extend_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64Trunc_sF32, BuiltinLanguageType.F32, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64Trunc_uF32, BuiltinLanguageType.F32, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64Trunc_sF64, BuiltinLanguageType.F64, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64Trunc_uF64, BuiltinLanguageType.F64, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I32ReinterpretF32, BuiltinLanguageType.F32, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I64ReinterpretF64, BuiltinLanguageType.F64, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.F32ReinterpretI32, BuiltinLanguageType.I32, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F64ReinterpretI64, BuiltinLanguageType.I64, BuiltinLanguageType.F64);
        }

    private ConversionInstruction(OpCodes opcode, BuiltinLanguageType from, BuiltinLanguageType to)
            : base(opcode)
        {
            InputType = from;
            OutputType = to;
            return;
        }

        internal BuiltinLanguageType InputType { get; private set; }

        internal BuiltinLanguageType OutputType { get; private set; }

        internal static ConversionInstruction GetInstruction(OpCodes opcode)
        {
            ConversionInstruction result = _knownInstructions[(int)opcode];
            if (null == result) { throw new ArgumentException(); }
            return result;
        }

        private static void RegisterKnownInstruction(OpCodes opcode, BuiltinLanguageType from, BuiltinLanguageType to)
        {
            _knownInstructions[(int)opcode] = new ConversionInstruction(opcode, from, to);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            if (poppedType != InputType) {
                context.AddError(string.Format("Encountered an {0} on the stack while expecting an {1}.",
                    poppedType, InputType));
                return false;
            }
            context.StackPush(OutputType);
            return true;
        }


        private static ConversionInstruction[] _knownInstructions = new ConversionInstruction[256];
    }
}
