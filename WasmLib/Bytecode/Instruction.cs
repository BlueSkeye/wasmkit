using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public override string ToString()
        {
            return OpCode.ToString();
        }

        internal abstract bool Validate(Stack<sbyte> stack, ValidationContext context);

        internal static readonly Instruction Unreachable = new ReusableInstruction(OpCodes.Unreachable);
        internal static readonly Instruction Nop = new ReusableInstruction(OpCodes.Nop); // No operation
        internal static readonly Instruction Else = new ReusableInstruction(OpCodes.Else); // begin else expression of if
        internal static readonly Instruction End = new ReusableInstruction(OpCodes.End); // end a block, loop, or if
        internal static readonly Instruction Return = new ReusableInstruction(OpCodes.Return); // return zero or one value from this function
        internal static readonly Instruction Drop = new ReusableInstruction(OpCodes.Drop); // ignore value
        internal static readonly Instruction Select = new ReusableInstruction(OpCodes.Select); // select one of two values based on condition
        internal static readonly Instruction CurrentMemory = new ReusableInstruction(OpCodes.CurrentMemory); // reserved : varuint1 query the size of memory
        internal static readonly Instruction GrowMemory = new ReusableInstruction(OpCodes.GrowMemory); // reserved : varuint1 grow the size of memory
        internal static readonly Instruction I32Eqz = new ReusableInstruction(OpCodes.I32eqz);
        internal static readonly Instruction I32Eq = new ReusableInstruction(OpCodes.I32eq);
        internal static readonly Instruction I32Ne = new ReusableInstruction(OpCodes.I32ne);
        internal static readonly Instruction I32LtS = new ReusableInstruction(OpCodes.I32lt_s);
        internal static readonly Instruction I32LtU = new ReusableInstruction(OpCodes.I32lt_u);
        internal static readonly Instruction I32GtS = new ReusableInstruction(OpCodes.I32gt_s);
        internal static readonly Instruction I32GtU = new ReusableInstruction(OpCodes.I32gt_u);
        internal static readonly Instruction I32LeS = new ReusableInstruction(OpCodes.I32le_s);
        internal static readonly Instruction I32LeU = new ReusableInstruction(OpCodes.I32le_u);
        internal static readonly Instruction I32GeS = new ReusableInstruction(OpCodes.I32ge_s);
        internal static readonly Instruction I32GeU = new ReusableInstruction(OpCodes.I32ge_u);
        internal static readonly Instruction I64EqZ = new ReusableInstruction(OpCodes.I64eqz);
        internal static readonly Instruction I64Eq = new ReusableInstruction(OpCodes.I64eq);
        internal static readonly Instruction I64Ne = new ReusableInstruction(OpCodes.I64ne);
        internal static readonly Instruction I64LtS = new ReusableInstruction(OpCodes.I64lt_s);
        internal static readonly Instruction I64LtU = new ReusableInstruction(OpCodes.I64lt_u);
        internal static readonly Instruction I64GtS = new ReusableInstruction(OpCodes.I64gt_s);
        internal static readonly Instruction I64GtU = new ReusableInstruction(OpCodes.I64gt_u);
        internal static readonly Instruction I64LeS = new ReusableInstruction(OpCodes.I64le_s);
        internal static readonly Instruction I64LeU = new ReusableInstruction(OpCodes.I64le_u);
        internal static readonly Instruction I64GeS = new ReusableInstruction(OpCodes.I64ge_s);
        internal static readonly Instruction I64GeU = new ReusableInstruction(OpCodes.I64ge_u);
        internal static readonly Instruction F32Eq = new ReusableInstruction(OpCodes.F32eq);
        internal static readonly Instruction F32Ne = new ReusableInstruction(OpCodes.F32ne);
        internal static readonly Instruction F32Lt = new ReusableInstruction(OpCodes.F32lt);
        internal static readonly Instruction F32Gt = new ReusableInstruction(OpCodes.F32gt);
        internal static readonly Instruction F32Le = new ReusableInstruction(OpCodes.F32le);
        internal static readonly Instruction F32Ge = new ReusableInstruction(OpCodes.F32ge);
        internal static readonly Instruction F64Eq = new ReusableInstruction(OpCodes.F64eq);
        internal static readonly Instruction F64Ne = new ReusableInstruction(OpCodes.F64ne);
        internal static readonly Instruction F64Lt = new ReusableInstruction(OpCodes.F64lt);
        internal static readonly Instruction F64Gt = new ReusableInstruction(OpCodes.F64gt);
        internal static readonly Instruction F64Le = new ReusableInstruction(OpCodes.F64le);
        internal static readonly Instruction F64Ge = new ReusableInstruction(OpCodes.F64ge);
        internal static readonly Instruction I32Clz = new ReusableInstruction(OpCodes.I32clz);
        internal static readonly Instruction I32Ctz = new ReusableInstruction(OpCodes.I32ctz);
        internal static readonly Instruction I32PopCnt = new ReusableInstruction(OpCodes.I32popcnt);
        internal static readonly Instruction I32Add = new ReusableInstruction(OpCodes.I32add);
        internal static readonly Instruction I32Sub = new ReusableInstruction(OpCodes.I32sub);
        internal static readonly Instruction I32Mul = new ReusableInstruction(OpCodes.I32mul);
        internal static readonly Instruction I32DivS = new ReusableInstruction(OpCodes.I32div_s);
        internal static readonly Instruction I32DivU = new ReusableInstruction(OpCodes.I32div_u);
        internal static readonly Instruction I32RemS = new ReusableInstruction(OpCodes.I32rem_s);
        internal static readonly Instruction I32RemU = new ReusableInstruction(OpCodes.I32rem_u);
        internal static readonly Instruction I32And = new ReusableInstruction(OpCodes.I32and);
        internal static readonly Instruction I32Or = new ReusableInstruction(OpCodes.I32or);
        internal static readonly Instruction I32Xor = new ReusableInstruction(OpCodes.I32xor);
        internal static readonly Instruction I32Shl = new ReusableInstruction(OpCodes.I32shl);
        internal static readonly Instruction I32ShrS = new ReusableInstruction(OpCodes.I32shr_s);
        internal static readonly Instruction I32ShrU = new ReusableInstruction(OpCodes.I32shr_u);
        internal static readonly Instruction I32Rotl = new ReusableInstruction(OpCodes.I32rotl);
        internal static readonly Instruction I32RotR = new ReusableInstruction(OpCodes.I32rotr);
        internal static readonly Instruction I64Clz = new ReusableInstruction(OpCodes.I64clz);
        internal static readonly Instruction I64Ctz = new ReusableInstruction(OpCodes.I64ctz);
        internal static readonly Instruction I64PopCnt = new ReusableInstruction(OpCodes.I64popcnt);
        internal static readonly Instruction I64Add = new ReusableInstruction(OpCodes.I64add);
        internal static readonly Instruction I64Sub = new ReusableInstruction(OpCodes.I64sub);
        internal static readonly Instruction I64Mul = new ReusableInstruction(OpCodes.I64mul);
        internal static readonly Instruction I64DivS = new ReusableInstruction(OpCodes.I64div_s);
        internal static readonly Instruction I64DivU = new ReusableInstruction(OpCodes.I64div_u);
        internal static readonly Instruction I64RemS = new ReusableInstruction(OpCodes.I64rem_s);
        internal static readonly Instruction I64RemU = new ReusableInstruction(OpCodes.I64rem_u);
        internal static readonly Instruction I64And = new ReusableInstruction(OpCodes.I64and);
        internal static readonly Instruction I64Or = new ReusableInstruction(OpCodes.I64or);
        internal static readonly Instruction I64Xor = new ReusableInstruction(OpCodes.I64xor);
        internal static readonly Instruction I64Shl = new ReusableInstruction(OpCodes.I64shl);
        internal static readonly Instruction I64ShrS = new ReusableInstruction(OpCodes.I64shr_s);
        internal static readonly Instruction I64ShrU = new ReusableInstruction(OpCodes.I64shr_u);
        internal static readonly Instruction I64Rotl = new ReusableInstruction(OpCodes.I64rotl);
        internal static readonly Instruction I64Rotr = new ReusableInstruction(OpCodes.I64rotr);
        internal static readonly Instruction F32Abs = new ReusableInstruction(OpCodes.F32abs);
        internal static readonly Instruction F32Neg = new ReusableInstruction(OpCodes.F32neg);
        internal static readonly Instruction F32Ceil = new ReusableInstruction(OpCodes.F32ceil);
        internal static readonly Instruction F32Floor = new ReusableInstruction(OpCodes.F32floor);
        internal static readonly Instruction F32Trunc = new ReusableInstruction(OpCodes.F32trunc);
        internal static readonly Instruction F32Nearest = new ReusableInstruction(OpCodes.F32nearest);
        internal static readonly Instruction F32Sqrt = new ReusableInstruction(OpCodes.F32sqrt);
        internal static readonly Instruction F32Add = new ReusableInstruction(OpCodes.F32add);
        internal static readonly Instruction F32Sub = new ReusableInstruction(OpCodes.F32sub);
        internal static readonly Instruction F32Mul = new ReusableInstruction(OpCodes.F32mul);
        internal static readonly Instruction F32Div = new ReusableInstruction(OpCodes.F32div);
        internal static readonly Instruction F32Min = new ReusableInstruction(OpCodes.F32min);
        internal static readonly Instruction F32Max = new ReusableInstruction(OpCodes.F32max);
        internal static readonly Instruction F32CopySign = new ReusableInstruction(OpCodes.F32copysign);
        internal static readonly Instruction F64Abs = new ReusableInstruction(OpCodes.F64abs);
        internal static readonly Instruction F64Neg = new ReusableInstruction(OpCodes.F64neg);
        internal static readonly Instruction F64Ceil = new ReusableInstruction(OpCodes.F64ceil);
        internal static readonly Instruction F64Floor = new ReusableInstruction(OpCodes.F64floor);
        internal static readonly Instruction F64Trunc = new ReusableInstruction(OpCodes.F64Trunc);
        internal static readonly Instruction F64Nearest = new ReusableInstruction(OpCodes.F64nearest);
        internal static readonly Instruction F64Sqrt = new ReusableInstruction(OpCodes.F64sqrt);
        internal static readonly Instruction F64Add = new ReusableInstruction(OpCodes.F64add);
        internal static readonly Instruction F64Sub = new ReusableInstruction(OpCodes.F64sub);
        internal static readonly Instruction F64Mul = new ReusableInstruction(OpCodes.F64mul);
        internal static readonly Instruction F64Div = new ReusableInstruction(OpCodes.F64div);
        internal static readonly Instruction F64Min = new ReusableInstruction(OpCodes.F64min);
        internal static readonly Instruction F64Max = new ReusableInstruction(OpCodes.F64max);
        internal static readonly Instruction F64CopySign = new ReusableInstruction(OpCodes.F64copysign);
        internal static readonly Instruction I32WrapI64 = new ReusableInstruction(OpCodes.I32wrapI64);
        internal static readonly Instruction I32TruncSF32 = new ReusableInstruction(OpCodes.I32Trunc_sF32);
        internal static readonly Instruction I32TruncUF32 = new ReusableInstruction(OpCodes.I32Trunc_uF32);
        internal static readonly Instruction I32TruncSF64 = new ReusableInstruction(OpCodes.I32Trunc_sF64);
        internal static readonly Instruction I32TruncUF64 = new ReusableInstruction(OpCodes.I32Trunc_uF64);
        internal static readonly Instruction I64ExtendSI32 = new ReusableInstruction(OpCodes.I64Extend_sI32);
        internal static readonly Instruction I64ExtendUI32 = new ReusableInstruction(OpCodes.I64Extend_uI32);
        internal static readonly Instruction I64TruncSF32 = new ReusableInstruction(OpCodes.I64Trunc_sF32);
        internal static readonly Instruction I64TruncUF32 = new ReusableInstruction(OpCodes.I64Trunc_uF32);
        internal static readonly Instruction I64TruncSF64 = new ReusableInstruction(OpCodes.I64Trunc_sF64);
        internal static readonly Instruction I64TruncUF64 = new ReusableInstruction(OpCodes.I64Trunc_uF64);
        internal static readonly Instruction F32ConvertSI32 = new ReusableInstruction(OpCodes.F32Convert_sI32);
        internal static readonly Instruction F32ConvertUI32 = new ReusableInstruction(OpCodes.F32Convert_uI32);
        internal static readonly Instruction F32ConvertSI64 = new ReusableInstruction(OpCodes.F32Convert_sI64);
        internal static readonly Instruction F32ConvertUI64 = new ReusableInstruction(OpCodes.F32Convert_uI64);
        internal static readonly Instruction F32DemoteF64 = new ReusableInstruction(OpCodes.F32demoteF64);
        internal static readonly Instruction F64ConvertSI32 = new ReusableInstruction(OpCodes.F64Convert_sI32);
        internal static readonly Instruction F64ConvertUI32 = new ReusableInstruction(OpCodes.F64Convert_uI32);
        internal static readonly Instruction F64ConvertSI64 = new ReusableInstruction(OpCodes.F64Convert_sI64);
        internal static readonly Instruction F64ConvertUI64 = new ReusableInstruction(OpCodes.F64Convert_uI64);
        internal static readonly Instruction F64PromoteF32 = new ReusableInstruction(OpCodes.F64PromoteF32);
        internal static readonly Instruction I32ReinterpretF32 = new ReusableInstruction(OpCodes.I32ReinterpretF32);
        internal static readonly Instruction I64ReinterpretF64 = new ReusableInstruction(OpCodes.I64ReinterpretF64);
        internal static readonly Instruction F32ReinterpretI32 = new ReusableInstruction(OpCodes.F32ReinterpretI32);
        internal static readonly Instruction F64ReinterpretI64 = new ReusableInstruction(OpCodes.F64ReinterpretI64);

        internal static readonly ReadOnlyCollection<Instruction> FixedOpCodes;
    }
}
