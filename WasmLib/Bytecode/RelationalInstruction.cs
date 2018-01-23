using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class RelationalInstruction : Instruction
    {
        private RelationalInstruction(OpCodes opcode, BuiltinLanguageType argumentsType)
            : base(opcode)
        {
            ArgumentsType = argumentsType;
        }

        internal BuiltinLanguageType ArgumentsType { get; private set; }

        internal static RelationalInstruction Create(OpCodes opcode)
        {
            switch (opcode) {
                case OpCodes.F32eq:
                    return _f32eqInstruction;
                case OpCodes.F32ge:
                    return _f32geInstruction;
                case OpCodes.F32gt:
                    return _f32gtInstruction;
                case OpCodes.F32le:
                    return _f32leInstruction;
                case OpCodes.F32lt:
                    return _f32ltInstruction;
                case OpCodes.F32ne:
                    return _f32neInstruction;
                case OpCodes.F64eq:
                    return _f64eqInstruction;
                case OpCodes.F64ge:
                    return _f64geInstruction;
                case OpCodes.F64gt:
                    return _f64gtInstruction;
                case OpCodes.F64le:
                    return _f64leInstruction;
                case OpCodes.F64lt:
                    return _f64ltInstruction;
                case OpCodes.F64ne:
                    return _f64neInstruction;
                case OpCodes.I32eq:
                    return _i32eqInstruction;
                case OpCodes.I32ge_s:
                    return _i32gesInstruction;
                case OpCodes.I32ge_u:
                    return _i32geuInstruction;
                case OpCodes.I32gt_s:
                    return _i32gtsInstruction;
                case OpCodes.I32gt_u:
                    return _i32gtuInstruction;
                case OpCodes.I32le_s:
                    return _i32lesInstruction;
                case OpCodes.I32le_u:
                    return _i32leuInstruction;
                case OpCodes.I32lt_s:
                    return _i32ltsInstruction;
                case OpCodes.I32lt_u:
                    return _i32ltuInstruction;
                case OpCodes.I32ne:
                    return _i32neInstruction;
                case OpCodes.I64eq:
                    return _i64eqInstruction;
                case OpCodes.I64ge_s:
                    return _i64gesInstruction;
                case OpCodes.I64ge_u:
                    return _i64geuInstruction;
                case OpCodes.I64gt_s:
                    return _i64gtsInstruction;
                case OpCodes.I64gt_u:
                    return _i64gtuInstruction;
                case OpCodes.I64le_s:
                    return _i64lesInstruction;
                case OpCodes.I64le_u:
                    return _i64leuInstruction;
                case OpCodes.I64lt_s:
                    return _i64ltsInstruction;
                case OpCodes.I64lt_u:
                    return _i64ltuInstruction;
                case OpCodes.I64ne:
                    return _i64neInstruction;
                default:
                    throw new ArgumentException();
            }
        }

        internal override bool Validate(ValidationContext context)
        {
            for(int index =0;index < 2; index++) {
                BuiltinLanguageType poppedType = context.StackPop();
                if (0 == poppedType) { context.AddError("Attempt to pop an empty stack."); }
                if (ArgumentsType != poppedType) {
                    context.AddError(string.Format("Popped an {0} for a relational instruction on {1}.",
                        poppedType, ArgumentsType));
                    return false;
                }
            }
            context.StackPush(ArgumentsType);
            return true;
        }

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
