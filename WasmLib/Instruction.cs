using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WasmLib
{
    internal class Instruction
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

        internal static readonly Instruction Unreachable = new Instruction(OpCodes.Unreachable);
        internal static readonly Instruction Nop = new Instruction(OpCodes.Nop); // No operation
        internal static readonly Instruction Else = new Instruction(OpCodes.Else); // begin else expression of if
        internal static readonly Instruction End = new Instruction(OpCodes.End); // end a block, loop, or if
        internal static readonly Instruction Return = new Instruction(OpCodes.Return); // return zero or one value from this function
        internal static readonly Instruction Drop = new Instruction(OpCodes.Drop); // ignore value
        internal static readonly Instruction Select = new Instruction(OpCodes.Select); // select one of two values based on condition
        internal static readonly Instruction CurrentMemory = new Instruction(OpCodes.CurrentMemory); // reserved : varuint1 query the size of memory
        internal static readonly Instruction GrowMemory = new Instruction(OpCodes.GrowMemory); // reserved : varuint1 grow the size of memory
        internal static readonly Instruction I32Eqz = new Instruction(OpCodes.I32eqz);
        internal static readonly Instruction I32Eq = new Instruction(OpCodes.I32eq);
        internal static readonly Instruction I32Ne = new Instruction(OpCodes.I32ne);
        internal static readonly Instruction I32LtS = new Instruction(OpCodes.I32lt_s);
        internal static readonly Instruction I32LtU = new Instruction(OpCodes.I32lt_u);
        internal static readonly Instruction I32GtS = new Instruction(OpCodes.I32gt_s);
        internal static readonly Instruction I32GtU = new Instruction(OpCodes.I32gt_u);
        internal static readonly Instruction I32LeS = new Instruction(OpCodes.I32le_s);
        internal static readonly Instruction I32LeU = new Instruction(OpCodes.I32le_u);
        internal static readonly Instruction I32GeS = new Instruction(OpCodes.I32ge_s);
        internal static readonly Instruction I32GeU = new Instruction(OpCodes.I32ge_u);
        internal static readonly Instruction I64EqZ = new Instruction(OpCodes.I64eqz);
        internal static readonly Instruction I64Eq = new Instruction(OpCodes.I64eq);
        internal static readonly Instruction I64Ne = new Instruction(OpCodes.I64ne);
        internal static readonly Instruction I64LtS = new Instruction(OpCodes.I64lt_s);
        internal static readonly Instruction I64LtU = new Instruction(OpCodes.I64lt_u);
        internal static readonly Instruction I64GtS = new Instruction(OpCodes.I64gt_s);
        internal static readonly Instruction I64GtU = new Instruction(OpCodes.I64gt_u);
        internal static readonly Instruction I64LeS = new Instruction(OpCodes.I64le_s);
        internal static readonly Instruction I64LeU = new Instruction(OpCodes.I64le_u);
        internal static readonly Instruction I64GeS = new Instruction(OpCodes.I64ge_s);
        internal static readonly Instruction I64GeU = new Instruction(OpCodes.I64ge_u);
        internal static readonly Instruction F32Eq = new Instruction(OpCodes.F32eq);
        internal static readonly Instruction F32Ne = new Instruction(OpCodes.F32ne);
        internal static readonly Instruction F32Lt = new Instruction(OpCodes.F32lt);
        internal static readonly Instruction F32Gt = new Instruction(OpCodes.F32gt);
        internal static readonly Instruction F32Le = new Instruction(OpCodes.F32le);
        internal static readonly Instruction F32Ge = new Instruction(OpCodes.F32ge);
        internal static readonly Instruction F64Eq = new Instruction(OpCodes.F64eq);
        internal static readonly Instruction F64Ne = new Instruction(OpCodes.F64ne);
        internal static readonly Instruction F64Lt = new Instruction(OpCodes.F64lt);
        internal static readonly Instruction F64Gt = new Instruction(OpCodes.F64gt);
        internal static readonly Instruction F64Le = new Instruction(OpCodes.F64le);
        internal static readonly Instruction F64Ge = new Instruction(OpCodes.F64ge);
        internal static readonly Instruction I32Clz = new Instruction(OpCodes.I32clz);
        internal static readonly Instruction I32Ctz = new Instruction(OpCodes.I32ctz);
        internal static readonly Instruction I32PopCnt = new Instruction(OpCodes.I32popcnt);
        internal static readonly Instruction I32Add = new Instruction(OpCodes.I32add);
        internal static readonly Instruction I32Sub = new Instruction(OpCodes.I32sub);
        internal static readonly Instruction I32Mul = new Instruction(OpCodes.I32mul);
        internal static readonly Instruction I32DivS = new Instruction(OpCodes.I32div_s);
        internal static readonly Instruction I32DivU = new Instruction(OpCodes.I32div_u);
        internal static readonly Instruction I32RemS = new Instruction(OpCodes.I32rem_s);
        internal static readonly Instruction I32RemU = new Instruction(OpCodes.I32rem_u);
        internal static readonly Instruction I32And = new Instruction(OpCodes.I32and);
        internal static readonly Instruction I32Or = new Instruction(OpCodes.I32or);
        internal static readonly Instruction I32Xor = new Instruction(OpCodes.I32xor);
        internal static readonly Instruction I32Shl = new Instruction(OpCodes.I32shl);
        internal static readonly Instruction I32ShrS = new Instruction(OpCodes.I32shr_s);
        internal static readonly Instruction I32ShrU = new Instruction(OpCodes.I32shr_u);
        internal static readonly Instruction I32Rotl = new Instruction(OpCodes.I32rotl);
        internal static readonly Instruction I32RotR = new Instruction(OpCodes.I32rotr);
        internal static readonly Instruction I64Clz = new Instruction(OpCodes.I64clz);
        internal static readonly Instruction I64Ctz = new Instruction(OpCodes.I64ctz);
        internal static readonly Instruction I64PopCnt = new Instruction(OpCodes.I64popcnt);
        internal static readonly Instruction I64Add = new Instruction(OpCodes.I64add);
        internal static readonly Instruction I64Sub = new Instruction(OpCodes.I64sub);
        internal static readonly Instruction I64Mul = new Instruction(OpCodes.I64mul);
        internal static readonly Instruction I64DivS = new Instruction(OpCodes.I64div_s);
        internal static readonly Instruction I64DivU = new Instruction(OpCodes.I64div_u);
        internal static readonly Instruction I64RemS = new Instruction(OpCodes.I64rem_s);
        internal static readonly Instruction I64RemU = new Instruction(OpCodes.I64rem_u);
        internal static readonly Instruction I64And = new Instruction(OpCodes.I64and);
        internal static readonly Instruction I64Or = new Instruction(OpCodes.I64or);
        internal static readonly Instruction I64Xor = new Instruction(OpCodes.I64xor);
        internal static readonly Instruction I64Shl = new Instruction(OpCodes.I64shl);
        internal static readonly Instruction I64ShrS = new Instruction(OpCodes.I64shr_s);
        internal static readonly Instruction I64ShrU = new Instruction(OpCodes.I64shr_u);
        internal static readonly Instruction I64Rotl = new Instruction(OpCodes.I64rotl);
        internal static readonly Instruction I64Rotr = new Instruction(OpCodes.I64rotr);
        internal static readonly Instruction F32Abs = new Instruction(OpCodes.F32abs);
        internal static readonly Instruction F32Neg = new Instruction(OpCodes.F32neg);
        internal static readonly Instruction F32Ceil = new Instruction(OpCodes.F32ceil);
        internal static readonly Instruction F32Floor = new Instruction(OpCodes.F32floor);
        internal static readonly Instruction F32Trunc = new Instruction(OpCodes.F32trunc);
        internal static readonly Instruction F32Nearest = new Instruction(OpCodes.F32nearest);
        internal static readonly Instruction F32Sqrt = new Instruction(OpCodes.F32sqrt);
        internal static readonly Instruction F32Add = new Instruction(OpCodes.F32add);
        internal static readonly Instruction F32Sub = new Instruction(OpCodes.F32sub);
        internal static readonly Instruction F32Mul = new Instruction(OpCodes.F32mul);
        internal static readonly Instruction F32Div = new Instruction(OpCodes.F32div);
        internal static readonly Instruction F32Min = new Instruction(OpCodes.F32min);
        internal static readonly Instruction F32Max = new Instruction(OpCodes.F32max);
        internal static readonly Instruction F32CopySign = new Instruction(OpCodes.F32copysign);
        internal static readonly Instruction F64Abs = new Instruction(OpCodes.F64abs);
        internal static readonly Instruction F64Neg = new Instruction(OpCodes.F64neg);
        internal static readonly Instruction F64Ceil = new Instruction(OpCodes.F64ceil);
        internal static readonly Instruction F64Floor = new Instruction(OpCodes.F64floor);
        internal static readonly Instruction F64Trunc = new Instruction(OpCodes.F64Trunc);
        internal static readonly Instruction F64Nearest = new Instruction(OpCodes.F64nearest);
        internal static readonly Instruction F64Sqrt = new Instruction(OpCodes.F64sqrt);
        internal static readonly Instruction F64Add = new Instruction(OpCodes.F64add);
        internal static readonly Instruction F64Sub = new Instruction(OpCodes.F64sub);
        internal static readonly Instruction F64Mul = new Instruction(OpCodes.F64mul);
        internal static readonly Instruction F64Div = new Instruction(OpCodes.F64div);
        internal static readonly Instruction F64Min = new Instruction(OpCodes.F64min);
        internal static readonly Instruction F64Max = new Instruction(OpCodes.F64max);
        internal static readonly Instruction F64CopySign = new Instruction(OpCodes.F64copysign);
        internal static readonly Instruction I32WrapI64 = new Instruction(OpCodes.I32wrapI64);
        internal static readonly Instruction I32TruncSF32 = new Instruction(OpCodes.I32Trunc_sF32);
        internal static readonly Instruction I32TruncUF32 = new Instruction(OpCodes.I32Trunc_uF32);
        internal static readonly Instruction I32TruncSF64 = new Instruction(OpCodes.I32Trunc_sF64);
        internal static readonly Instruction I32TruncUF64 = new Instruction(OpCodes.I32Trunc_uF64);
        internal static readonly Instruction I64ExtendSI32 = new Instruction(OpCodes.I64Extend_sI32);
        internal static readonly Instruction I64ExtendUI32 = new Instruction(OpCodes.I64Extend_uI32);
        internal static readonly Instruction I64TruncSF32 = new Instruction(OpCodes.I64Trunc_sF32);
        internal static readonly Instruction I64TruncUF32 = new Instruction(OpCodes.I64Trunc_uF32);
        internal static readonly Instruction I64TruncSF64 = new Instruction(OpCodes.I64Trunc_sF64);
        internal static readonly Instruction I64TruncUF64 = new Instruction(OpCodes.I64Trunc_uF64);
        internal static readonly Instruction F32ConvertSI32 = new Instruction(OpCodes.F32Convert_sI32);
        internal static readonly Instruction F32ConvertUI32 = new Instruction(OpCodes.F32Convert_uI32);
        internal static readonly Instruction F32ConvertSI64 = new Instruction(OpCodes.F32Convert_sI64);
        internal static readonly Instruction F32ConvertUI64 = new Instruction(OpCodes.F32Convert_uI64);
        internal static readonly Instruction F32DemoteF64 = new Instruction(OpCodes.F32demoteF64);
        internal static readonly Instruction F64ConvertSI32 = new Instruction(OpCodes.F64Convert_sI32);
        internal static readonly Instruction F64ConvertUI32 = new Instruction(OpCodes.F64Convert_uI32);
        internal static readonly Instruction F64ConvertSI64 = new Instruction(OpCodes.F64Convert_sI64);
        internal static readonly Instruction F64ConvertUI64 = new Instruction(OpCodes.F64Convert_uI64);
        internal static readonly Instruction F64PromoteF32 = new Instruction(OpCodes.F64PromoteF32);
        internal static readonly Instruction I32ReinterpretF32 = new Instruction(OpCodes.I32ReinterpretF32);
        internal static readonly Instruction I64ReinterpretF64 = new Instruction(OpCodes.I64ReinterpretF64);
        internal static readonly Instruction F32ReinterpretI32 = new Instruction(OpCodes.F32ReinterpretI32);
        internal static readonly Instruction F64ReinterpretI64 = new Instruction(OpCodes.F64ReinterpretI64);

        internal static readonly ReadOnlyCollection<Instruction> FixedOpCodes;
    }

    internal class BlockInstruction : Instruction
    {
        private BlockInstruction()
            : base (OpCodes.Block)
        {
            return;
        }

        internal sbyte BlockType { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _perTypeBlock.Length); }
        }

        internal static BlockInstruction Create(BinaryParsingReader reader)
        {
            sbyte blockType = reader.ReadVarInt7();
            BlockInstruction result = _perTypeBlock[blockType + 128]; 

            if (null != result) {
                _reuseCount++;
            }
            else {
                result = new BlockInstruction() { BlockType = blockType };
                _perTypeBlock[blockType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", BlockType);
        }

        private static int _reuseCount = 0;
        private static BlockInstruction[] _perTypeBlock = new BlockInstruction[256];
    }

    internal class BranchInstruction : Instruction
    {
        private BranchInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal bool Conditional { get; private set; }

        internal uint Depth { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _perDepthBranch.Length + _perDepthConditionalBranch.Length); }
        }

        internal static BranchInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint depth = reader.ReadVarUint32();
            bool conditional = (OpCodes.BrIf == opcode);

            if (MaxReusableDepth < depth) {
                return new BranchInstruction(opcode) { Conditional = conditional, Depth = depth };
            }
            BranchInstruction[] reuseTargets = conditional ? _perDepthBranch : _perDepthConditionalBranch;
            BranchInstruction result = reuseTargets[depth];

            if (null != result) { _reuseCount++; }
            else {
                result = new BranchInstruction(opcode) { Conditional = true, Depth = depth };
                reuseTargets[depth] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2} {1}", Depth, Conditional ? "(C)" : string.Empty);
        }

        private const int MaxReusableDepth = 255;
        private static int _reuseCount = 0;
        private static BranchInstruction[] _perDepthBranch = new BranchInstruction[MaxReusableDepth + 1];
        private static BranchInstruction[] _perDepthConditionalBranch = new BranchInstruction[MaxReusableDepth + 1];
    }

    internal class BranchTableInstruction : Instruction
    {
        private BranchTableInstruction()
            : base(OpCodes.BrTable)
        {
            return;
        }

        internal uint DefaultTarget { get; private set; }

        internal uint[] Targets { get; private set; }

        internal static BranchTableInstruction Create(BinaryParsingReader reader)
        {
            uint targetCount = reader.ReadVarUint32();
            uint[] targets = new uint[targetCount];
            for(int index = 0; index < targetCount; index++) {
                targets[index] = reader.ReadVarUint32();
            }
            uint defaultTarget = reader.ReadVarUint32();
            return new BranchTableInstruction() { DefaultTarget = defaultTarget, Targets = targets };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", Targets);
        }
    }

    internal class CallInstruction : Instruction
    {
        private CallInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal byte ExpectedSignature { get; private set; }

        internal bool Indirect { get; private set; }

        internal uint ItemIndex { get; private set; }

        internal static CallInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint itemIndex = reader.ReadVarUint32();
            bool indirect = (OpCodes.CallIndirect == opcode);
            byte expectedSIgnature = indirect ? reader.ReadVarUint1(): (byte)0;
            return new CallInstruction(opcode) { ExpectedSignature = expectedSIgnature, Indirect = indirect, ItemIndex = itemIndex };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X8} {1}", ItemIndex, Indirect ? "(I)" : string.Empty);
        }
    }

    internal class ConstantValueInstruction<T> : Instruction
    {
        private ConstantValueInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownValues.Count); }
        }

        internal T Value { get; private set; }

        internal static ConstantValueInstruction<T> Create(BinaryParsingReader reader, OpCodes opcode, T value)
        {
            ConstantValueInstruction<T> result;

            if (_knownValues.TryGetValue(value, out result)) {
                _reuseCount++;
                return result;
            }
            result = new ConstantValueInstruction<T>(opcode) { Value = value };
            _knownValues.Add(value, result);
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + " " + Value.ToString();
        }

        private static int _reuseCount = 0;
        private static SortedList<T, ConstantValueInstruction<T>> _knownValues = new SortedList<T, ConstantValueInstruction<T>>();
    }

    internal class GlobalAccessorInstruction : Instruction
    {
        private GlobalAccessorInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
        }

        public uint GlobalIndex { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownGlobalGetters.Length+ _knownGlobalSetters.Length); }
        }

        internal static GlobalAccessorInstruction Create(BinaryParsingReader reader, bool getter)
        {
            uint globalIndex = reader.ReadVarUint32();
            OpCodes opcode = getter ? OpCodes.GetGlobal : OpCodes.SetGlobal;

            if (MaxReusableIndex < globalIndex) {
                return new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
            }

            GlobalAccessorInstruction[] reusableAccessors = getter ? _knownGlobalGetters : _knownGlobalSetters;
            GlobalAccessorInstruction result = reusableAccessors[globalIndex];

            if (null != result) { _reuseCount++; }
            else {
                result = new GlobalAccessorInstruction(opcode) { GlobalIndex = globalIndex };
                reusableAccessors[globalIndex] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", GlobalIndex);
        }

        private const int MaxReusableIndex = 1024;
        private static int _reuseCount = 0;
        private static GlobalAccessorInstruction[] _knownGlobalGetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
        private static GlobalAccessorInstruction[] _knownGlobalSetters = new GlobalAccessorInstruction[MaxReusableIndex + 1];
    }

    internal class IfInstruction : Instruction
    {
        private IfInstruction()
            : base(OpCodes.If)
        {
            return;
        }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownTests.Length); }
        }

        internal sbyte ValueType { get; private set; }

        internal static IfInstruction Create(BinaryParsingReader reader)
        {
            sbyte valueType = reader.ReadValueType(true);
            IfInstruction result = _knownTests[valueType + 128];

            if (null != result) { _reuseCount++; }
            else {
                result = new IfInstruction() { ValueType = valueType };
                _knownTests[valueType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", ValueType);
        }

        private static int _reuseCount = 0;
        private static IfInstruction[] _knownTests = new IfInstruction[256];
    }

    internal class LocalAccessorInstruction : Instruction
    {
        private LocalAccessorInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
        }

        public uint LocalIndex { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownAccessors.Length); }
        }

        internal static LocalAccessorInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint localIndex = reader.ReadVarUint32();

            if (MaxReusablIndex < localIndex) {
                return new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
            }
            LocalAccessorInstruction result = _knownAccessors[localIndex];

            if (null != result) { _reuseCount++; }
            else {
                result = new LocalAccessorInstruction(opcode) { LocalIndex = localIndex };
                _knownAccessors[localIndex] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" {0}", LocalIndex);
        }

        private static int MaxReusablIndex = 255;
        private static int _reuseCount = 0;
        private static LocalAccessorInstruction[] _knownAccessors = new LocalAccessorInstruction[MaxReusablIndex + 1];
    }

    internal class LoopInstruction : Instruction
    {
        private LoopInstruction()
            : base(OpCodes.Loop)
        {
            return;
        }

        internal sbyte BlockType { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _knownLoops.Length); }
        }

        internal static LoopInstruction Create(BinaryParsingReader reader)
        {
            sbyte blockType = reader.ReadVarInt7();
            LoopInstruction result = _knownLoops[blockType + 128];

            if (null != result) { _reuseCount++; }
            else {
                result = new LoopInstruction() { BlockType = blockType };
                _knownLoops[blockType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", BlockType);
        }

        private static int _reuseCount = 0;
        private static LoopInstruction[] _knownLoops = new LoopInstruction[256];
    }

    internal class MemoryAccessInstruction : Instruction
    {
        private MemoryAccessInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
        }

        internal static MemoryAccessInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            // uint localIndex = reader.ReadVarUint32();
            RawValueEncoding rawValue;
            bool store = false;
            switch (opcode) {
                case OpCodes.I32Store:
                    store = true;
                    goto case OpCodes.I32Load;
                case OpCodes.I32Load:
                    rawValue = RawValueEncoding.I32;
                    break;
                case OpCodes.I64Store:
                    store = true;
                    goto case OpCodes.I64Load;
                case OpCodes.I64Load:
                    rawValue = RawValueEncoding.I64;
                    break;
                case OpCodes.F32Store:
                    store = true;
                    goto case OpCodes.F32Load;
                case OpCodes.F32Load:
                    rawValue = RawValueEncoding.F32;
                    break;
                case OpCodes.F64Store:
                    store = true;
                    goto case OpCodes.F64Load;
                case OpCodes.F64Load:
                    rawValue = RawValueEncoding.F64;
                    break;
                case OpCodes.I32Load8_s:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.ByteStorageSize | RawValueEncoding.Signed;
                    break;
                case OpCodes.I32Load8_u:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.ByteStorageSize | RawValueEncoding.Unsigned;
                    break;
                case OpCodes.I32Load16_s:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.WordStorageSize | RawValueEncoding.Signed;
                    break;
                case OpCodes.I32Load16_u:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.WordStorageSize | RawValueEncoding.Unsigned;
                    break;
                case OpCodes.I64Load8_s:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.ByteStorageSize | RawValueEncoding.Signed;
                    break;
                case OpCodes.I64Load8_u:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.ByteStorageSize | RawValueEncoding.Unsigned;
                    break;
                case OpCodes.I64Load16_s:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.WordStorageSize | RawValueEncoding.Signed;
                    break;
                case OpCodes.I64Load16_u:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.WordStorageSize | RawValueEncoding.Unsigned;
                    break;
                case OpCodes.I64Load32_s:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.DoubleWordStorageSize | RawValueEncoding.Signed;
                    break;
                case OpCodes.I64Load32_u:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.DoubleWordStorageSize | RawValueEncoding.Unsigned;
                    break;
                case OpCodes.I32Store8:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.ByteStorageSize;
                    store = true;
                    break;
                case OpCodes.I32Store16:
                    rawValue = RawValueEncoding.I32 | RawValueEncoding.WordStorageSize;
                    store = true;
                    break;
                case OpCodes.I64Store8:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.ByteStorageSize;
                    store = true;
                    break;
                case OpCodes.I64Store16:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.WordStorageSize;
                    store = true;
                    break;
                case OpCodes.I64Store32:
                    rawValue = RawValueEncoding.I64 | RawValueEncoding.DoubleWordStorageSize;
                    store = true;
                    break;
                default:
                    throw new NotSupportedException();
            }
            uint flags = reader.ReadVarUint32();
            uint offset = reader.ReadVarUint32();
            return new MemoryAccessInstruction(opcode) { _rawValue = rawValue, IsStore = store,
                IsLoad = !store, Offset = offset, Align = flags };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}, o=0x{1:X8}, o=0x{2:X8}", (byte)_rawValue, Offset, Align);
        }

        public uint Align { get; private set; }

        public bool IsLoad { get; private set; }

        public bool IsStore { get; private set; }

        public uint Offset { get; private set; }

        internal RawValueEncoding Signedness
        {
            get { return _rawValue & RawValueEncoding.SignMask; }
        }

        internal RawValueEncoding StorageSize
        {
            get { return _rawValue & RawValueEncoding.StorageSizeMask; }
        }

        internal sbyte ValueType
        {
            get { return (sbyte)(-((int)(_rawValue & RawValueEncoding.ValueTypeMask) - 1)); }
        }
        
        private RawValueEncoding _rawValue;

        [Flags()]
        internal enum RawValueEncoding
        {
            // Used for ValueType property.
            I32 = 0x00,
            I64 = 0x01,
            F32 = 0x02,
            F64 = 0x03,

            // Used for StorageSize property
            SameStorageSize = 0x00,
            ByteStorageSize = 0x04,
            WordStorageSize = 0x08,
            DoubleWordStorageSize = 0x0C,

            // Used for Signedness
            Unsigned = 0x00,
            Signed = 0x10,

            // Masks
            ValueTypeMask = 0x03,
            StorageSizeMask = 0x0C,
            SignMask = 0x10,
        }
    }

    internal class MemoryControlInstruction : Instruction
    {
        private MemoryControlInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal static uint UseCount { get; private set; }

        internal byte Reserved { get; private set; }

        internal static MemoryControlInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            byte reserved = reader.ReadVarUint1();
            UseCount++;
            return new MemoryControlInstruction(opcode) { Reserved = reserved };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}", Reserved);
        }
    }
}
