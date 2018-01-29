using System;

namespace WasmLib.Bytecode
{
    internal class RelationalInstruction : Instruction
    {
        static RelationalInstruction()
        {
            RegisterKnownInstruction(OpCodes.F32eq, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32ge, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32gt, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32le, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32lt, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F32ne, BuiltinLanguageType.F32);
            RegisterKnownInstruction(OpCodes.F64eq, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64ge, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64gt, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64le, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64lt, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.F64ne, BuiltinLanguageType.F64);
            RegisterKnownInstruction(OpCodes.I32eq, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32ge_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32ge_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32gt_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32gt_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32le_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32le_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32lt_s, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32lt_u, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I32ne, BuiltinLanguageType.I32);
            RegisterKnownInstruction(OpCodes.I64eq, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64ge_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64ge_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64gt_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64gt_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64le_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64le_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64lt_s, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64lt_u, BuiltinLanguageType.I64);
            RegisterKnownInstruction(OpCodes.I64ne, BuiltinLanguageType.I64);
        }

        private RelationalInstruction(OpCodes opcode, BuiltinLanguageType argumentsType)
            : base(opcode)
        {
            ArgumentsType = argumentsType;
        }

        internal BuiltinLanguageType ArgumentsType { get; private set; }

        internal static RelationalInstruction GetInstruction(OpCodes opcode)
        {
            RelationalInstruction result = _knownInstructions[(int)opcode];
            if (null == result) { throw new ArgumentException(); }
            return result;
        }

        private static void RegisterKnownInstruction(OpCodes opcode, BuiltinLanguageType type)
        {
            _knownInstructions[(int)opcode] = new RelationalInstruction(opcode, type);
        }

        internal override bool Validate(ValidationContext context)
        {
            for(int index =0; index < 2; index++) {
                BuiltinLanguageType poppedType = context.StackPop();
                if (0 == poppedType) { context.AddError("Attempt to pop an empty stack."); }
                if (ArgumentsType != poppedType) {
                    context.AddError(string.Format("Popped an {0} for a relational instruction on {1}.",
                        poppedType, ArgumentsType));
                    return false;
                }
            }
            context.StackPush(BuiltinLanguageType.I32);
            return true;
        }

        private static readonly RelationalInstruction[] _knownInstructions = new RelationalInstruction[256];

        private static readonly RelationalInstruction _f32eqInstruction = new RelationalInstruction(OpCodes.F32eq, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f32geInstruction = new RelationalInstruction(OpCodes.F32ge, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f32gtInstruction = new RelationalInstruction(OpCodes.F32gt, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f32leInstruction = new RelationalInstruction(OpCodes.F32le, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f32ltInstruction = new RelationalInstruction(OpCodes.F32lt, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f32neInstruction = new RelationalInstruction(OpCodes.F32ne, BuiltinLanguageType.F32);
        private static readonly RelationalInstruction _f64eqInstruction = new RelationalInstruction(OpCodes.F64eq, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _f64geInstruction = new RelationalInstruction(OpCodes.F64ge, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _f64gtInstruction = new RelationalInstruction(OpCodes.F64gt, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _f64leInstruction = new RelationalInstruction(OpCodes.F64le, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _f64ltInstruction = new RelationalInstruction(OpCodes.F64lt, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _f64neInstruction = new RelationalInstruction(OpCodes.F64ne, BuiltinLanguageType.F64);
        private static readonly RelationalInstruction _i32eqInstruction = new RelationalInstruction(OpCodes.I32eq, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32gesInstruction = new RelationalInstruction(OpCodes.I32ge_s, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32geuInstruction = new RelationalInstruction(OpCodes.I32ge_u, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32gtsInstruction = new RelationalInstruction(OpCodes.I32gt_s, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32gtuInstruction = new RelationalInstruction(OpCodes.I32gt_u, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32lesInstruction = new RelationalInstruction(OpCodes.I32le_s, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32leuInstruction = new RelationalInstruction(OpCodes.I32le_u, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32ltsInstruction = new RelationalInstruction(OpCodes.I32lt_s, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32ltuInstruction = new RelationalInstruction(OpCodes.I32lt_u, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i32neInstruction = new RelationalInstruction(OpCodes.I32ne, BuiltinLanguageType.I32);
        private static readonly RelationalInstruction _i64eqInstruction = new RelationalInstruction(OpCodes.I64eq, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64gesInstruction = new RelationalInstruction(OpCodes.I64ge_s, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64geuInstruction = new RelationalInstruction(OpCodes.I64ge_u, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64gtsInstruction = new RelationalInstruction(OpCodes.I64gt_s, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64gtuInstruction = new RelationalInstruction(OpCodes.I64gt_u, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64lesInstruction = new RelationalInstruction(OpCodes.I64le_s, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64leuInstruction = new RelationalInstruction(OpCodes.I64le_u, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64ltsInstruction = new RelationalInstruction(OpCodes.I64lt_s, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64ltuInstruction = new RelationalInstruction(OpCodes.I64lt_u, BuiltinLanguageType.I64);
        private static readonly RelationalInstruction _i64neInstruction = new RelationalInstruction(OpCodes.I64ne, BuiltinLanguageType.I64);
    }
}
