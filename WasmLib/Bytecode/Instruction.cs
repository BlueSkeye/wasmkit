using System;
using System.Collections.ObjectModel;

namespace WasmLib.Bytecode
{
    internal abstract class Instruction
    {
        static Instruction()
        {
            Instruction[] instructions = new Instruction[256];
            foreach (Instruction instruction in
                new Instruction[] { Unreachable, Nop, Else, End, Return, Drop, Select, CurrentMemory, GrowMemory,
                I32Eqz, I32Eq, I32Ne, I32LtS, I32LtU, I32GtS, I32GtU, I32LeS, I32LeU, I32GeS, I32GeU,
                I64EqZ, I64Eq, I64Ne, I64LtS, I64LtU, I64GtS, I64GtU, I64LeS, I64LeU, I64GeS, I64GeU,
                F32Eq, F32Ne, F32Lt, F32Gt, F32Le, F32Ge, F64Eq, F64Ne, F64Lt, F64Gt, F64Le, F64Ge,
                I32Clz, I32Ctz, I32PopCnt, I32Add, I32Sub, I32Mul, I32DivS, I32DivU, I32RemS, I32RemU,
                I32And, I32Or, I32Xor, I32Shl, I32ShrS, I32ShrU, I32Rotl, I32RotR, I64Clz, I64Ctz,
                I64PopCnt, I64Add, I64Sub, I64Mul, I64DivS, I64DivU, I64RemS, I64RemU, I64And, I64Or,
                I64Xor, I64Shl, I64ShrS, I64ShrU, I64Rotl, I64Rotr, F32Abs, F32Neg, F32Ceil, F32Floor,
                F32Trunc, F32Nearest, F32Sqrt, F32Add, F32Sub, F32Mul, F32Div, F32Min, F32Max, F32CopySign,
                F64Abs, F64Neg, F64Ceil, F64Floor, F64Trunc, F64Nearest, F64Sqrt, F64Add, F64Sub, F64Mul,
                F64Div, F64Min, F64Max, F64CopySign, I32WrapI64, I32TruncSF32, I32TruncUF32, I32TruncSF64,
                I32TruncUF64, I64ExtendSI32, I64ExtendUI32, I64TruncSF32, I64TruncUF32, I64TruncSF64,
                I64TruncUF64, F32ConvertSI32, F32ConvertUI32, F32ConvertSI64, F32ConvertUI64, F32DemoteF64,
                F64ConvertSI32, F64ConvertUI32, F64ConvertSI64, F64ConvertUI64, F64PromoteF32, I32ReinterpretF32,
                I64ReinterpretF64, F32ReinterpretI32, F64ReinterpretI64})
            {
                instructions[(int)instruction.OpCode] = instruction;
            }
            FixedOpCodes = Array.AsReadOnly(instructions);
        }

        protected Instruction(OpCodes opcode)
        {
            OpCode = opcode;
        }

        internal OpCodes OpCode { get; private set; }

        internal BuiltinLanguageType GetResultType()
        {
            return GetResultType(OpCode);
        }

        internal static BuiltinLanguageType GetResultType(OpCodes opcode)
        {
            switch (opcode) {
                case OpCodes.Unreachable: throw new NotImplementedException();
                case OpCodes.Nop: throw new NotImplementedException();
                case OpCodes.Block: throw new NotImplementedException();
                case OpCodes.Loop: throw new NotImplementedException();
                case OpCodes.If: throw new NotImplementedException();
                case OpCodes.Else: throw new NotImplementedException();
                case OpCodes.End: throw new NotSupportedException();
                case OpCodes.Br: throw new NotImplementedException();
                case OpCodes.BrIf: throw new NotImplementedException();
                case OpCodes.BrTable: throw new NotImplementedException();
                case OpCodes.Return: throw new NotImplementedException();
                case OpCodes.Call: throw new NotImplementedException();
                case OpCodes.CallIndirect: throw new NotImplementedException();
                case OpCodes.Drop: return BuiltinLanguageType.EmptyBlock;
                case OpCodes.Select: throw new NotImplementedException();
                case OpCodes.GetLocal: throw new NotImplementedException();
                case OpCodes.SetLocal: throw new NotImplementedException();
                case OpCodes.TeeLocal: throw new NotImplementedException();
                case OpCodes.GetGlobal: throw new NotImplementedException();
                case OpCodes.SetGlobal: throw new NotImplementedException();
                case OpCodes.I32Load: throw new NotImplementedException();
                case OpCodes.I64Load: throw new NotImplementedException();
                case OpCodes.F32Load: throw new NotImplementedException();
                case OpCodes.F64Load: throw new NotImplementedException();
                case OpCodes.I32Load8_s: throw new NotImplementedException();
                case OpCodes.I32Load8_u: throw new NotImplementedException();
                case OpCodes.I32Load16_s: throw new NotImplementedException();
                case OpCodes.I32Load16_u: throw new NotImplementedException();
                case OpCodes.I64Load8_s: throw new NotImplementedException();
                case OpCodes.I64Load8_u: throw new NotImplementedException();
                case OpCodes.I64Load16_s: throw new NotImplementedException();
                case OpCodes.I64Load16_u: throw new NotImplementedException();
                case OpCodes.I64Load32_s: throw new NotImplementedException();
                case OpCodes.I64Load32_u: throw new NotImplementedException();
                case OpCodes.I32Store: throw new NotImplementedException();
                case OpCodes.I64Store: throw new NotImplementedException();
                case OpCodes.F32Store: throw new NotImplementedException();
                case OpCodes.F64Store: throw new NotImplementedException();
                case OpCodes.I32Store8: throw new NotImplementedException();
                case OpCodes.I32Store16: throw new NotImplementedException();
                case OpCodes.I64Store8: throw new NotImplementedException();
                case OpCodes.I64Store16: throw new NotImplementedException();
                case OpCodes.I64Store32: throw new NotImplementedException();
                case OpCodes.CurrentMemory: throw new NotImplementedException();
                case OpCodes.GrowMemory: throw new NotImplementedException();
                case OpCodes.I32Const: throw new NotImplementedException();
                case OpCodes.I64Const: throw new NotImplementedException();
                case OpCodes.F32Const: throw new NotImplementedException();
                case OpCodes.F64Const: throw new NotImplementedException();
                case OpCodes.I32eqz: return BuiltinLanguageType.I32;
                case OpCodes.I32eq: return BuiltinLanguageType.I32;
                case OpCodes.I32ne: return BuiltinLanguageType.I32;
                case OpCodes.I32lt_s: return BuiltinLanguageType.I32;
                case OpCodes.I32lt_u: return BuiltinLanguageType.I32;
                case OpCodes.I32gt_s: return BuiltinLanguageType.I32;
                case OpCodes.I32gt_u: return BuiltinLanguageType.I32;
                case OpCodes.I32le_s: return BuiltinLanguageType.I32;
                case OpCodes.I32le_u: return BuiltinLanguageType.I32;
                case OpCodes.I32ge_s: return BuiltinLanguageType.I32;
                case OpCodes.I32ge_u: return BuiltinLanguageType.I32;
                case OpCodes.I64eqz: return BuiltinLanguageType.I32;
                case OpCodes.I64eq: return BuiltinLanguageType.I32;
                case OpCodes.I64ne: return BuiltinLanguageType.I32;
                case OpCodes.I64lt_s: return BuiltinLanguageType.I32;
                case OpCodes.I64lt_u: return BuiltinLanguageType.I32;
                case OpCodes.I64gt_s: return BuiltinLanguageType.I32;
                case OpCodes.I64gt_u: return BuiltinLanguageType.I32;
                case OpCodes.I64le_s: return BuiltinLanguageType.I32;
                case OpCodes.I64le_u: return BuiltinLanguageType.I32;
                case OpCodes.I64ge_s: return BuiltinLanguageType.I32;
                case OpCodes.I64ge_u: return BuiltinLanguageType.I32;
                case OpCodes.F32eq: return BuiltinLanguageType.I32;
                case OpCodes.F32ne: return BuiltinLanguageType.I32;
                case OpCodes.F32lt: return BuiltinLanguageType.I32;
                case OpCodes.F32gt: return BuiltinLanguageType.I32;
                case OpCodes.F32le: return BuiltinLanguageType.I32;
                case OpCodes.F32ge: return BuiltinLanguageType.I32;
                case OpCodes.F64eq: return BuiltinLanguageType.I32;
                case OpCodes.F64ne: return BuiltinLanguageType.I32;
                case OpCodes.F64lt: return BuiltinLanguageType.I32;
                case OpCodes.F64gt: return BuiltinLanguageType.I32;
                case OpCodes.F64le: return BuiltinLanguageType.I32;
                case OpCodes.F64ge: return BuiltinLanguageType.I32;
                case OpCodes.I32clz: return BuiltinLanguageType.I32;
                case OpCodes.I32ctz: return BuiltinLanguageType.I32;
                case OpCodes.I32popcnt: return BuiltinLanguageType.I32;
                case OpCodes.I32add: return BuiltinLanguageType.I32;
                case OpCodes.I32sub: return BuiltinLanguageType.I32;
                case OpCodes.I32mul: return BuiltinLanguageType.I32;
                case OpCodes.I32div_s: return BuiltinLanguageType.I32;
                case OpCodes.I32div_u: return BuiltinLanguageType.I32;
                case OpCodes.I32rem_s: return BuiltinLanguageType.I32;
                case OpCodes.I32rem_u: return BuiltinLanguageType.I32;
                case OpCodes.I32and: return BuiltinLanguageType.I32;
                case OpCodes.I32or: return BuiltinLanguageType.I32;
                case OpCodes.I32xor: return BuiltinLanguageType.I32;
                case OpCodes.I32shl: return BuiltinLanguageType.I32;
                case OpCodes.I32shr_s: return BuiltinLanguageType.I32;
                case OpCodes.I32shr_u: return BuiltinLanguageType.I32;
                case OpCodes.I32rotl: return BuiltinLanguageType.I32;
                case OpCodes.I32rotr: return BuiltinLanguageType.I32;
                case OpCodes.I64clz: return BuiltinLanguageType.I64;
                case OpCodes.I64ctz: return BuiltinLanguageType.I64;
                case OpCodes.I64popcnt: return BuiltinLanguageType.I64;
                case OpCodes.I64add: return BuiltinLanguageType.I64;
                case OpCodes.I64sub: return BuiltinLanguageType.I64;
                case OpCodes.I64mul: return BuiltinLanguageType.I64;
                case OpCodes.I64div_s: return BuiltinLanguageType.I64;
                case OpCodes.I64div_u: return BuiltinLanguageType.I64;
                case OpCodes.I64rem_s: return BuiltinLanguageType.I64;
                case OpCodes.I64rem_u: return BuiltinLanguageType.I64;
                case OpCodes.I64and: return BuiltinLanguageType.I64;
                case OpCodes.I64or: return BuiltinLanguageType.I64;
                case OpCodes.I64xor: return BuiltinLanguageType.I64;
                case OpCodes.I64shl: return BuiltinLanguageType.I64;
                case OpCodes.I64shr_s: return BuiltinLanguageType.I64;
                case OpCodes.I64shr_u: return BuiltinLanguageType.I64;
                case OpCodes.I64rotl: return BuiltinLanguageType.I64;
                case OpCodes.I64rotr: return BuiltinLanguageType.I64;
                case OpCodes.F32abs: throw new NotImplementedException();
                case OpCodes.F32neg: throw new NotImplementedException();
                case OpCodes.F32ceil: throw new NotImplementedException();
                case OpCodes.F32floor: throw new NotImplementedException();
                case OpCodes.F32trunc: throw new NotImplementedException();
                case OpCodes.F32nearest: throw new NotImplementedException();
                case OpCodes.F32sqrt: throw new NotImplementedException();
                case OpCodes.F32add: return BuiltinLanguageType.F32;
                case OpCodes.F32sub: return BuiltinLanguageType.F32;
                case OpCodes.F32mul: return BuiltinLanguageType.F32;
                case OpCodes.F32div: return BuiltinLanguageType.F32;
                case OpCodes.F32min: return BuiltinLanguageType.F32;
                case OpCodes.F32max: return BuiltinLanguageType.F32;
                case OpCodes.F32copysign: return BuiltinLanguageType.F32;
                case OpCodes.F64abs: throw new NotImplementedException();
                case OpCodes.F64neg: throw new NotImplementedException();
                case OpCodes.F64ceil: throw new NotImplementedException();
                case OpCodes.F64floor: throw new NotImplementedException();
                case OpCodes.F64Trunc: throw new NotImplementedException();
                case OpCodes.F64nearest: throw new NotImplementedException();
                case OpCodes.F64sqrt: throw new NotImplementedException();
                case OpCodes.F64add: return BuiltinLanguageType.F64;
                case OpCodes.F64sub: return BuiltinLanguageType.F64;
                case OpCodes.F64mul: return BuiltinLanguageType.F64;
                case OpCodes.F64div: return BuiltinLanguageType.F64;
                case OpCodes.F64min: return BuiltinLanguageType.F64;
                case OpCodes.F64max: return BuiltinLanguageType.F64;
                case OpCodes.F64copysign: return BuiltinLanguageType.F64;
                case OpCodes.I32wrapI64: return BuiltinLanguageType.I32;
                case OpCodes.I32Trunc_sF32: return BuiltinLanguageType.I32;
                case OpCodes.I32Trunc_uF32: return BuiltinLanguageType.I32;
                case OpCodes.I32Trunc_sF64: return BuiltinLanguageType.I32;
                case OpCodes.I32Trunc_uF64: return BuiltinLanguageType.I32;
                case OpCodes.I64Extend_sI32: return BuiltinLanguageType.I64;
                case OpCodes.I64Extend_uI32: return BuiltinLanguageType.I64;
                case OpCodes.I64Trunc_sF32: return BuiltinLanguageType.I64;
                case OpCodes.I64Trunc_uF32: return BuiltinLanguageType.I64;
                case OpCodes.I64Trunc_sF64: return BuiltinLanguageType.I64;
                case OpCodes.I64Trunc_uF64: return BuiltinLanguageType.I64;
                case OpCodes.F32Convert_sI32: return BuiltinLanguageType.F32;
                case OpCodes.F32Convert_uI32: return BuiltinLanguageType.F32;
                case OpCodes.F32Convert_sI64: return BuiltinLanguageType.F32;
                case OpCodes.F32Convert_uI64: return BuiltinLanguageType.F32;
                case OpCodes.F32demoteF64: return BuiltinLanguageType.F32;
                case OpCodes.F64Convert_sI32: return BuiltinLanguageType.F64;
                case OpCodes.F64Convert_uI32: return BuiltinLanguageType.F64;
                case OpCodes.F64Convert_sI64: return BuiltinLanguageType.F64;
                case OpCodes.F64Convert_uI64: return BuiltinLanguageType.F64;
                case OpCodes.F64PromoteF32: return BuiltinLanguageType.F64;
                case OpCodes.I32ReinterpretF32: return BuiltinLanguageType.I32;
                case OpCodes.I64ReinterpretF64: return BuiltinLanguageType.I64;
                case OpCodes.F32ReinterpretI32: return BuiltinLanguageType.F32;
                case OpCodes.F64ReinterpretI64: return BuiltinLanguageType.F64;
                default:
                    throw new ArgumentException();
            }
        }

        public override string ToString()
        {
            return OpCode.ToString();
        }

        internal abstract bool Validate(ValidationContext context);

        internal static readonly Instruction Unreachable = UnreachableInstruction.Singleton;
        internal static readonly Instruction Nop = NopInstruction.Singleton; // No operation
        internal static readonly Instruction Else = ElseInstruction.Singleton; // begin else expression of if
        internal static readonly Instruction End = EndInstruction.Singleton; // end a block, loop, or if
        internal static readonly Instruction Return = ReturnInstruction.Singleton; // return zero or one value from this function
        internal static readonly Instruction Drop = DropInstruction.Singleton; // ignore value
        internal static readonly Instruction Select = SelectInstruction.Singleton; // select one of two values based on condition
        internal static readonly Instruction CurrentMemory = CurrentMemoryInstruction.Singleton; // reserved : varuint1 query the size of memory
        internal static readonly Instruction GrowMemory = UnaryInstruction.GetInstruction(OpCodes.GrowMemory); // reserved : varuint1 grow the size of memory
        internal static readonly Instruction I32Eqz = IntegerTestInstruction.Create(OpCodes.I32eqz);
        internal static readonly Instruction I32Eq = RelationalInstruction.GetInstruction(OpCodes.I32eq);
        internal static readonly Instruction I32Ne = RelationalInstruction.GetInstruction(OpCodes.I32ne);
        internal static readonly Instruction I32LtS = RelationalInstruction.GetInstruction(OpCodes.I32lt_s);
        internal static readonly Instruction I32LtU = RelationalInstruction.GetInstruction(OpCodes.I32lt_u);
        internal static readonly Instruction I32GtS = RelationalInstruction.GetInstruction(OpCodes.I32gt_s);
        internal static readonly Instruction I32GtU = RelationalInstruction.GetInstruction(OpCodes.I32gt_u);
        internal static readonly Instruction I32LeS = RelationalInstruction.GetInstruction(OpCodes.I32le_s);
        internal static readonly Instruction I32LeU = RelationalInstruction.GetInstruction(OpCodes.I32le_u);
        internal static readonly Instruction I32GeS = RelationalInstruction.GetInstruction(OpCodes.I32ge_s);
        internal static readonly Instruction I32GeU = RelationalInstruction.GetInstruction(OpCodes.I32ge_u);
        internal static readonly Instruction I64EqZ = IntegerTestInstruction.Create(OpCodes.I64eqz);
        internal static readonly Instruction I64Eq = RelationalInstruction.GetInstruction(OpCodes.I64eq);
        internal static readonly Instruction I64Ne = RelationalInstruction.GetInstruction(OpCodes.I64ne);
        internal static readonly Instruction I64LtS = RelationalInstruction.GetInstruction(OpCodes.I64lt_s);
        internal static readonly Instruction I64LtU = RelationalInstruction.GetInstruction(OpCodes.I64lt_u);
        internal static readonly Instruction I64GtS = RelationalInstruction.GetInstruction(OpCodes.I64gt_s);
        internal static readonly Instruction I64GtU = RelationalInstruction.GetInstruction(OpCodes.I64gt_u);
        internal static readonly Instruction I64LeS = RelationalInstruction.GetInstruction(OpCodes.I64le_s);
        internal static readonly Instruction I64LeU = RelationalInstruction.GetInstruction(OpCodes.I64le_u);
        internal static readonly Instruction I64GeS = RelationalInstruction.GetInstruction(OpCodes.I64ge_s);
        internal static readonly Instruction I64GeU = RelationalInstruction.GetInstruction(OpCodes.I64ge_u);
        internal static readonly Instruction F32Eq = RelationalInstruction.GetInstruction(OpCodes.F32eq);
        internal static readonly Instruction F32Ne = RelationalInstruction.GetInstruction(OpCodes.F32ne);
        internal static readonly Instruction F32Lt = RelationalInstruction.GetInstruction(OpCodes.F32lt);
        internal static readonly Instruction F32Gt = RelationalInstruction.GetInstruction(OpCodes.F32gt);
        internal static readonly Instruction F32Le = RelationalInstruction.GetInstruction(OpCodes.F32le);
        internal static readonly Instruction F32Ge = RelationalInstruction.GetInstruction(OpCodes.F32ge);
        internal static readonly Instruction F64Eq = RelationalInstruction.GetInstruction(OpCodes.F64eq);
        internal static readonly Instruction F64Ne = RelationalInstruction.GetInstruction(OpCodes.F64ne);
        internal static readonly Instruction F64Lt = RelationalInstruction.GetInstruction(OpCodes.F64lt);
        internal static readonly Instruction F64Gt = RelationalInstruction.GetInstruction(OpCodes.F64gt);
        internal static readonly Instruction F64Le = RelationalInstruction.GetInstruction(OpCodes.F64le);
        internal static readonly Instruction F64Ge = RelationalInstruction.GetInstruction(OpCodes.F64ge);
        internal static readonly Instruction I32Clz = UnaryInstruction.GetInstruction(OpCodes.I32clz);
        internal static readonly Instruction I32Ctz = UnaryInstruction.GetInstruction(OpCodes.I32ctz);
        internal static readonly Instruction I32PopCnt = UnaryInstruction.GetInstruction(OpCodes.I32popcnt);
        internal static readonly Instruction I32Add = BinaryInstruction.GetInstruction(OpCodes.I32add);
        internal static readonly Instruction I32Sub = BinaryInstruction.GetInstruction(OpCodes.I32sub);
        internal static readonly Instruction I32Mul = BinaryInstruction.GetInstruction(OpCodes.I32mul);
        internal static readonly Instruction I32DivS = BinaryInstruction.GetInstruction(OpCodes.I32div_s);
        internal static readonly Instruction I32DivU = BinaryInstruction.GetInstruction(OpCodes.I32div_u);
        internal static readonly Instruction I32RemS = BinaryInstruction.GetInstruction(OpCodes.I32rem_s);
        internal static readonly Instruction I32RemU = BinaryInstruction.GetInstruction(OpCodes.I32rem_u);
        internal static readonly Instruction I32And = BinaryInstruction.GetInstruction(OpCodes.I32and);
        internal static readonly Instruction I32Or = BinaryInstruction.GetInstruction(OpCodes.I32or);
        internal static readonly Instruction I32Xor = BinaryInstruction.GetInstruction(OpCodes.I32xor);
        internal static readonly Instruction I32Shl = BinaryInstruction.GetInstruction(OpCodes.I32shl);
        internal static readonly Instruction I32ShrS = BinaryInstruction.GetInstruction(OpCodes.I32shr_s);
        internal static readonly Instruction I32ShrU = BinaryInstruction.GetInstruction(OpCodes.I32shr_u);
        internal static readonly Instruction I32Rotl = BinaryInstruction.GetInstruction(OpCodes.I32rotl);
        internal static readonly Instruction I32RotR = BinaryInstruction.GetInstruction(OpCodes.I32rotr);
        internal static readonly Instruction I64Clz = UnaryInstruction.GetInstruction(OpCodes.I64clz);
        internal static readonly Instruction I64Ctz = UnaryInstruction.GetInstruction(OpCodes.I64ctz);
        internal static readonly Instruction I64PopCnt = UnaryInstruction.GetInstruction(OpCodes.I64popcnt);
        internal static readonly Instruction I64Add = BinaryInstruction.GetInstruction(OpCodes.I64add);
        internal static readonly Instruction I64Sub = BinaryInstruction.GetInstruction(OpCodes.I64sub);
        internal static readonly Instruction I64Mul = BinaryInstruction.GetInstruction(OpCodes.I64mul);
        internal static readonly Instruction I64DivS = BinaryInstruction.GetInstruction(OpCodes.I64div_s);
        internal static readonly Instruction I64DivU = BinaryInstruction.GetInstruction(OpCodes.I64div_u);
        internal static readonly Instruction I64RemS = BinaryInstruction.GetInstruction(OpCodes.I64rem_s);
        internal static readonly Instruction I64RemU = BinaryInstruction.GetInstruction(OpCodes.I64rem_u);
        internal static readonly Instruction I64And = BinaryInstruction.GetInstruction(OpCodes.I64and);
        internal static readonly Instruction I64Or = BinaryInstruction.GetInstruction(OpCodes.I64or);
        internal static readonly Instruction I64Xor = BinaryInstruction.GetInstruction(OpCodes.I64xor);
        internal static readonly Instruction I64Shl = BinaryInstruction.GetInstruction(OpCodes.I64shl);
        internal static readonly Instruction I64ShrS = BinaryInstruction.GetInstruction(OpCodes.I64shr_s);
        internal static readonly Instruction I64ShrU = BinaryInstruction.GetInstruction(OpCodes.I64shr_u);
        internal static readonly Instruction I64Rotl = BinaryInstruction.GetInstruction(OpCodes.I64rotl);
        internal static readonly Instruction I64Rotr = BinaryInstruction.GetInstruction(OpCodes.I64rotr);
        internal static readonly Instruction F32Abs = UnaryInstruction.GetInstruction(OpCodes.F32abs);
        internal static readonly Instruction F32Neg = UnaryInstruction.GetInstruction(OpCodes.F32neg);
        internal static readonly Instruction F32Ceil = UnaryInstruction.GetInstruction(OpCodes.F32ceil);
        internal static readonly Instruction F32Floor = UnaryInstruction.GetInstruction(OpCodes.F32floor);
        internal static readonly Instruction F32Trunc = UnaryInstruction.GetInstruction(OpCodes.F32trunc);
        internal static readonly Instruction F32Nearest = UnaryInstruction.GetInstruction(OpCodes.F32nearest);
        internal static readonly Instruction F32Sqrt = UnaryInstruction.GetInstruction(OpCodes.F32sqrt);
        internal static readonly Instruction F32Add = BinaryInstruction.GetInstruction(OpCodes.F32add);
        internal static readonly Instruction F32Sub = BinaryInstruction.GetInstruction(OpCodes.F32sub);
        internal static readonly Instruction F32Mul = BinaryInstruction.GetInstruction(OpCodes.F32mul);
        internal static readonly Instruction F32Div = BinaryInstruction.GetInstruction(OpCodes.F32div);
        internal static readonly Instruction F32Min = BinaryInstruction.GetInstruction(OpCodes.F32min);
        internal static readonly Instruction F32Max = BinaryInstruction.GetInstruction(OpCodes.F32max);
        internal static readonly Instruction F32CopySign = BinaryInstruction.GetInstruction(OpCodes.F32copysign);
        internal static readonly Instruction F64Abs = UnaryInstruction.GetInstruction(OpCodes.F64abs);
        internal static readonly Instruction F64Neg = UnaryInstruction.GetInstruction(OpCodes.F64neg);
        internal static readonly Instruction F64Ceil = UnaryInstruction.GetInstruction(OpCodes.F64ceil);
        internal static readonly Instruction F64Floor = UnaryInstruction.GetInstruction(OpCodes.F64floor);
        internal static readonly Instruction F64Trunc = UnaryInstruction.GetInstruction(OpCodes.F64Trunc);
        internal static readonly Instruction F64Nearest = UnaryInstruction.GetInstruction(OpCodes.F64nearest);
        internal static readonly Instruction F64Sqrt = UnaryInstruction.GetInstruction(OpCodes.F64sqrt);
        internal static readonly Instruction F64Add = BinaryInstruction.GetInstruction(OpCodes.F64add);
        internal static readonly Instruction F64Sub = BinaryInstruction.GetInstruction(OpCodes.F64sub);
        internal static readonly Instruction F64Mul = BinaryInstruction.GetInstruction(OpCodes.F64mul);
        internal static readonly Instruction F64Div = BinaryInstruction.GetInstruction(OpCodes.F64div);
        internal static readonly Instruction F64Min = BinaryInstruction.GetInstruction(OpCodes.F64min);
        internal static readonly Instruction F64Max = BinaryInstruction.GetInstruction(OpCodes.F64max);
        internal static readonly Instruction F64CopySign = BinaryInstruction.GetInstruction(OpCodes.F64copysign);
        internal static readonly Instruction I32WrapI64 = ConversionInstruction.GetInstruction(OpCodes.I32wrapI64);
        internal static readonly Instruction I32TruncSF32 = ConversionInstruction.GetInstruction(OpCodes.I32Trunc_sF32);
        internal static readonly Instruction I32TruncUF32 = ConversionInstruction.GetInstruction(OpCodes.I32Trunc_uF32);
        internal static readonly Instruction I32TruncSF64 = ConversionInstruction.GetInstruction(OpCodes.I32Trunc_sF64);
        internal static readonly Instruction I32TruncUF64 = ConversionInstruction.GetInstruction(OpCodes.I32Trunc_uF64);
        internal static readonly Instruction I64ExtendSI32 = ConversionInstruction.GetInstruction(OpCodes.I64Extend_sI32);
        internal static readonly Instruction I64ExtendUI32 = ConversionInstruction.GetInstruction(OpCodes.I64Extend_uI32);
        internal static readonly Instruction I64TruncSF32 = ConversionInstruction.GetInstruction(OpCodes.I64Trunc_sF32);
        internal static readonly Instruction I64TruncUF32 = ConversionInstruction.GetInstruction(OpCodes.I64Trunc_uF32);
        internal static readonly Instruction I64TruncSF64 = ConversionInstruction.GetInstruction(OpCodes.I64Trunc_sF64);
        internal static readonly Instruction I64TruncUF64 = ConversionInstruction.GetInstruction(OpCodes.I64Trunc_uF64);
        internal static readonly Instruction F32ConvertSI32 = ConversionInstruction.GetInstruction(OpCodes.F32Convert_sI32);
        internal static readonly Instruction F32ConvertUI32 = ConversionInstruction.GetInstruction(OpCodes.F32Convert_uI32);
        internal static readonly Instruction F32ConvertSI64 = ConversionInstruction.GetInstruction(OpCodes.F32Convert_sI64);
        internal static readonly Instruction F32ConvertUI64 = ConversionInstruction.GetInstruction(OpCodes.F32Convert_uI64);
        internal static readonly Instruction F32DemoteF64 = ConversionInstruction.GetInstruction(OpCodes.F32demoteF64);
        internal static readonly Instruction F64ConvertSI32 = ConversionInstruction.GetInstruction(OpCodes.F64Convert_sI32);
        internal static readonly Instruction F64ConvertUI32 = ConversionInstruction.GetInstruction(OpCodes.F64Convert_uI32);
        internal static readonly Instruction F64ConvertSI64 = ConversionInstruction.GetInstruction(OpCodes.F64Convert_sI64);
        internal static readonly Instruction F64ConvertUI64 = ConversionInstruction.GetInstruction(OpCodes.F64Convert_uI64);
        internal static readonly Instruction F64PromoteF32 = ConversionInstruction.GetInstruction(OpCodes.F64PromoteF32);
        internal static readonly Instruction I32ReinterpretF32 = ConversionInstruction.GetInstruction(OpCodes.I32ReinterpretF32);
        internal static readonly Instruction I64ReinterpretF64 = ConversionInstruction.GetInstruction(OpCodes.I64ReinterpretF64);
        internal static readonly Instruction F32ReinterpretI32 = ConversionInstruction.GetInstruction(OpCodes.F32ReinterpretI32);
        internal static readonly Instruction F64ReinterpretI64 = ConversionInstruction.GetInstruction(OpCodes.F64ReinterpretI64);

        internal static readonly ReadOnlyCollection<Instruction> FixedOpCodes;
    }
}
