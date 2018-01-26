using System;

namespace WasmLib.Bytecode
{
    internal class ConversionInstruction : Instruction
    {
        private ConversionInstruction(OpCodes opcode, BuiltinLanguageType popped, BuiltinLanguageType pushed)
            : base(opcode)
        {
            InputType = popped;
            OutputType = pushed;
            return;
        }

        internal BuiltinLanguageType InputType { get; private set; }

        internal BuiltinLanguageType OutputType { get; private set; }

        internal static ConversionInstruction Create(OpCodes opcode)
        {
            switch (opcode) {
                case OpCodes.F32Convert_sI32:
                    return _f32ConvertSI32;
                case OpCodes.F32Convert_uI32:
                    return _f32ConvertUI32;
                case OpCodes.F32Convert_sI64:
                    return _f32ConvertSI64;
                case OpCodes.F32Convert_uI64:
                    return _f32ConvertUI64;
                case OpCodes.F32demoteF64:
                    return _f32DemoteF64;
                case OpCodes.F64Convert_sI32:
                    return _f64ConvertSI32;
                case OpCodes.F64Convert_uI32:
                    return _f64ConvertUI32;
                case OpCodes.F64Convert_sI64:
                    return _f64ConvertSI64;
                case OpCodes.F64Convert_uI64:
                    return _f64ConvertUI64;
                case OpCodes.F64PromoteF32:
                    return _f64PromoteF32;
                case OpCodes.I32Trunc_sF32:
                    return _i32TruncSF32;
                case OpCodes.I32Trunc_uF32:
                    return _i32TruncUF32;
                case OpCodes.I32Trunc_sF64:
                    return _i32TruncSF64;
                case OpCodes.I32Trunc_uF64:
                    return _i32TruncUF64;
                case OpCodes.I32wrapI64:
                    return _i32WrapI64;
                case OpCodes.I64Extend_sI32:
                    return _i64ExtendSI32;
                case OpCodes.I64Extend_uI32:
                    return _i64ExtendUI32;
                case OpCodes.I64Trunc_sF32:
                    return _i64TruncSF32;
                case OpCodes.I64Trunc_uF32:
                    return _i64TruncUF32;
                case OpCodes.I64Trunc_sF64:
                    return _i64TruncSF64;
                case OpCodes.I64Trunc_uF64:
                    return _i64TruncUF64;
                default:
                    throw new ArgumentException();
            }
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

        private static ConversionInstruction _f32ConvertSI32 =
            new ConversionInstruction(OpCodes.F32Convert_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.F32);
        private static ConversionInstruction _f32ConvertUI32 =
            new ConversionInstruction(OpCodes.F32Convert_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.F32);
        private static ConversionInstruction _f32ConvertSI64 =
            new ConversionInstruction(OpCodes.F32Convert_sI64, BuiltinLanguageType.I64, BuiltinLanguageType.F32);
        private static ConversionInstruction _f32ConvertUI64 =
            new ConversionInstruction(OpCodes.F32Convert_uI64, BuiltinLanguageType.I64, BuiltinLanguageType.F32);
        private static ConversionInstruction _f32DemoteF64 =
            new ConversionInstruction(OpCodes.F32demoteF64, BuiltinLanguageType.F64, BuiltinLanguageType.F32);
        private static ConversionInstruction _f64ConvertSI32 =
            new ConversionInstruction(OpCodes.F64Convert_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.F64);
        private static ConversionInstruction _f64ConvertUI32 =
            new ConversionInstruction(OpCodes.F64Convert_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.F64);
        private static ConversionInstruction _f64ConvertSI64 =
            new ConversionInstruction(OpCodes.F64Convert_sI64, BuiltinLanguageType.I64, BuiltinLanguageType.F64);
        private static ConversionInstruction _f64ConvertUI64 =
            new ConversionInstruction(OpCodes.F32Convert_uI64, BuiltinLanguageType.I64, BuiltinLanguageType.F64);
        private static ConversionInstruction _f64PromoteF32 =
            new ConversionInstruction(OpCodes.F64PromoteF32, BuiltinLanguageType.F32, BuiltinLanguageType.F64);
        private static ConversionInstruction _i32TruncSF32 =
            new ConversionInstruction(OpCodes.I32Trunc_sF32, BuiltinLanguageType.F32, BuiltinLanguageType.I32);
        private static ConversionInstruction _i32TruncUF32 =
            new ConversionInstruction(OpCodes.I32Trunc_uF32, BuiltinLanguageType.F32, BuiltinLanguageType.I32);
        private static ConversionInstruction _i32TruncSF64 =
            new ConversionInstruction(OpCodes.I32Trunc_sF64, BuiltinLanguageType.F64, BuiltinLanguageType.I32);
        private static ConversionInstruction _i32TruncUF64 =
            new ConversionInstruction(OpCodes.I32Trunc_uF64, BuiltinLanguageType.F64, BuiltinLanguageType.I32);
        private static ConversionInstruction _i32WrapI64 =
            new ConversionInstruction(OpCodes.I32wrapI64, BuiltinLanguageType.I64, BuiltinLanguageType.I32);
        private static ConversionInstruction _i64ExtendSI32 =
            new ConversionInstruction(OpCodes.I64Extend_sI32, BuiltinLanguageType.I32, BuiltinLanguageType.I64);
        private static ConversionInstruction _i64ExtendUI32 =
            new ConversionInstruction(OpCodes.I64Extend_uI32, BuiltinLanguageType.I32, BuiltinLanguageType.I64);
        private static ConversionInstruction _i64TruncSF32 =
            new ConversionInstruction(OpCodes.I64Trunc_sF32, BuiltinLanguageType.F32, BuiltinLanguageType.I64);
        private static ConversionInstruction _i64TruncUF32 =
            new ConversionInstruction(OpCodes.I64Extend_uI32, BuiltinLanguageType.F32, BuiltinLanguageType.I64);
        private static ConversionInstruction _i64TruncSF64 =
            new ConversionInstruction(OpCodes.I64Trunc_sF64, BuiltinLanguageType.F64, BuiltinLanguageType.I64);
        private static ConversionInstruction _i64TruncUF64 =
            new ConversionInstruction(OpCodes.I64Extend_uI32, BuiltinLanguageType.F64, BuiltinLanguageType.I64);
    }
}
