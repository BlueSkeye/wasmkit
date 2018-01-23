using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

using WasmLib.Bytecode;

namespace WasmLib
{
    /// <summary>A binary reader that handling the specific encodings from a module
    /// binary file.</summary>
    /// <remarks>For LEB128 encoding (signed and unsigned), see :
    /// https://en.wikipedia.org/wiki/LEB128
    /// </remarks>
    internal class BinaryParsingReader : BinaryReader
    {
        internal BinaryParsingReader(Stream from)
            : base(from, Encoding.UTF8, true)
        {
            return;
        }

        /// <summary>Read a VarInt7 value and make sure it matches the expected type value,
        /// otherwise throw a parsing exception.</summary>
        /// <param name="expectedType"></param>
        internal void ReadAndAssertLanguageType(sbyte expectedType)
        {
            sbyte tag = this.ReadVarInt7();

            if (expectedType != tag) {
                throw new WasmParsingException(string.Format(
                    ParsingErrorMessages.InvalidTypeEncountered, expectedType, tag));
            }
        }

        internal List<Instruction> ReadFunctionBodyCode()
        {
            List<Instruction> result = new List<Instruction>();
            // This is to account for the End op-code that is the marker for body end.
            // TODO : Nothing is said about Block,If,Loop/End count matching. We should seek for a more
            // formal definition.
            uint depth = 1;
            while (true) {
                Instruction instruction = ReadInstruction();
                result.Add(instruction);
                if (OpCodes.Block == instruction.OpCode) { depth++; }
                else if (OpCodes.Loop == instruction.OpCode) { depth++; }
                else if (OpCodes.If == instruction.OpCode) { depth++; }
                if (OpCodes.End == instruction.OpCode) {
                    if (0 == --depth) { return result; }
                }
            }
        }

        internal BuiltinLanguageType ReadGlobaltype(out bool mutable)
        {
            sbyte contentType = this.ReadVarInt7();
            mutable = (0 != this.ReadVarUint1());
            return (BuiltinLanguageType)contentType;
        }

        internal List<Instruction> ReadInitializationExpression()
        {
            List<Instruction> result = new List<Instruction>();
            while (true) {
                Instruction instruction = ReadInstruction();
                result.Add(instruction);
                if (OpCodes.End == instruction.OpCode) { return result; }
            }
        }

        internal Instruction ReadInstruction()
        {
            OpCodes opcode = (OpCodes)this.ReadByte();
            Instruction result = Instruction.FixedOpCodes[(int)opcode];
            if (null != result) { return result; }
            switch (opcode) {
                case OpCodes.Block:
                    return BlockInstruction.Create(this);
                case OpCodes.Loop:
                    return LoopInstruction.Create(this);
                case OpCodes.If:
                    // block_type
                    return IfInstruction.Create(this);
                case OpCodes.Br:
                case OpCodes.BrIf:
                    // varuint32
                    return BranchInstruction.Create(this, opcode);
                case OpCodes.BrTable:
                    // custom
                    return BranchTableInstruction.Create(this);
                case OpCodes.Call:
                case OpCodes.CallIndirect:
                    // varuint32 + varuint1
                    return CallInstruction.Create(this, opcode);
                case OpCodes.GetLocal:
                case OpCodes.SetLocal:
                case OpCodes.TeeLocal:
                    // local_index
                    return LocalAccessorInstruction.Create(this, opcode);
                case OpCodes.GetGlobal:
                case OpCodes.SetGlobal:
                    // global_index
                    return GlobalAccessorInstruction.Create(this, (OpCodes.GetGlobal == opcode));
                case OpCodes.I32Load:
                case OpCodes.I64Load:
                case OpCodes.F32Load:
                case OpCodes.F64Load:
                case OpCodes.I32Load8_s:
                case OpCodes.I32Load8_u:
                case OpCodes.I32Load16_s:
                case OpCodes.I32Load16_u:
                case OpCodes.I64Load8_s:
                case OpCodes.I64Load8_u:
                case OpCodes.I64Load16_s:
                case OpCodes.I64Load16_u:
                case OpCodes.I64Load32_s:
                case OpCodes.I64Load32_u:
                case OpCodes.I32Store:
                case OpCodes.I64Store:
                case OpCodes.F32Store:
                case OpCodes.F64Store:
                case OpCodes.I32Store8:
                case OpCodes.I32Store16:
                case OpCodes.I64Store8:
                case OpCodes.I64Store16:
                case OpCodes.I64Store32:
                    // memory_immediate
                    return MemoryAccessInstruction.Create(this, opcode);
                case OpCodes.CurrentMemory:
                case OpCodes.GrowMemory:
                    // varuint1
                    return MemoryControlInstruction.Create(this, opcode);
                case OpCodes.I32Const:
                    // varint32
                    return ConstantValueInstruction<int>.Create(this, OpCodes.I32Const, this.ReadVarInt32());
                case OpCodes.I64Const:
                    // varint64
                    return ConstantValueInstruction<long>.Create(this, OpCodes.I64Const, this.ReadVarInt64());
                case OpCodes.F32Const:
                    // uint32
                    return ConstantValueInstruction<float>.Create(this, OpCodes.F32Const, this.ReadVarFloat32());
                case OpCodes.F64Const:
                    // uint64
                    return ConstantValueInstruction<double>.Create(this, OpCodes.F64Const, this.ReadVarFloat64());
                case OpCodes.F32eq:
                case OpCodes.F32ge:
                case OpCodes.F32gt:
                case OpCodes.F32le:
                case OpCodes.F32lt:
                case OpCodes.F32ne:
                case OpCodes.F64eq:
                case OpCodes.F64ge:
                case OpCodes.F64gt:
                case OpCodes.F64le:
                case OpCodes.F64lt:
                case OpCodes.F64ne:
                case OpCodes.I32eq:
                case OpCodes.I32ge_s:
                case OpCodes.I32ge_u:
                case OpCodes.I32gt_s:
                case OpCodes.I32gt_u:
                case OpCodes.I32le_s:
                case OpCodes.I32le_u:
                case OpCodes.I32lt_s:
                case OpCodes.I32lt_u:
                case OpCodes.I32ne:
                case OpCodes.I64eq:
                case OpCodes.I64ge_s:
                case OpCodes.I64ge_u:
                case OpCodes.I64gt_s:
                case OpCodes.I64gt_u:
                case OpCodes.I64le_s:
                case OpCodes.I64le_u:
                case OpCodes.I64lt_s:
                case OpCodes.I64lt_u:
                case OpCodes.I64ne:
                    return RelationalInstruction.Create(opcode);
                default:
                    throw new NotSupportedException();
            }
        }

        internal string ReadLengthPrefixedUTF8String()
        {
            uint stringRawDataLength = ReadVarUint32();

            if (_stringDataBuffer.Length < stringRawDataLength) {
                // TODO : Implement some kind of 'garbage' collection where the data buffer
                // length is unexpectedly high.
                _stringDataBuffer = new byte[stringRawDataLength];
            }
            base.Read(_stringDataBuffer, 0, (int)stringRawDataLength);
            return Encoding.UTF8.GetString(_stringDataBuffer);
        }

        internal BuiltinLanguageType ReadValueType(bool allowVoid = false)
        {
            BuiltinLanguageType tag = (BuiltinLanguageType)this.ReadVarInt7();

            switch (tag) {
                case BuiltinLanguageType.I32:
                case BuiltinLanguageType.I64:
                case BuiltinLanguageType.F32:
                case BuiltinLanguageType.F64:
                    return tag;
                default:
                    if (allowVoid && (BuiltinLanguageType.EmptyBlock == tag)) { return tag; }
                    throw new WasmParsingException(string.Format(
                        ParsingErrorMessages.InvalidValueTypeEncountered, tag.ToString()));
            }
        }

        internal float ReadVarFloat32()
        {
            uint rawValue = ReadUInt32();
            unsafe { return *(float*)(void*)&rawValue; }
        }

        internal double ReadVarFloat64()
        {
            ulong rawValue = ReadUInt64();
            unsafe { return *(double*)(void*)&rawValue; }
        }

        internal sbyte ReadVarInt7()
        {
            byte rawValue = ReadVarUint7();
            if (0 == (rawValue & 0x40)) {
                return (sbyte)rawValue;
            }
            // This is a negative value. Sign extend the rawValue.
            return (sbyte)(0x80 | rawValue);
        }

        internal int ReadVarInt32()
        {
            uint result = 0;
            byte rawValue;
            for (int index = 0; index < 4; index++) {
                rawValue = ReadByte();
                result += (uint)(rawValue & 0x7F) << (7 * index);
                if (0 == (rawValue & 0x80)) {
                    if (0 == (0x40 & rawValue)) {
                        return (int)result;
                    }
                    return (int)(result | (uint)((int)-1 << (7 * ++index)));
                }
            }
            rawValue = ReadByte();
            if (0 == (rawValue & 0x80)) {
                result += (uint)(rawValue & 0x7F) << 28;
                return (int)result;
            }
            throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
        }

        internal long ReadVarInt64()
        {
            ulong result = 0;
            byte rawValue;
            for (int index = 0; index < 9; index++) {
                rawValue = ReadByte();
                result += (ulong)(rawValue & 0x7F) << (7 * index);
                if (0 == (rawValue & 0x80)) {
                    if (0 == (0x40 & rawValue)) {
                        return (long)result;
                    }
                    return (long)(result | (ulong)((long)-1 << (7 * ++index)));
                }
            }
            rawValue = ReadByte();
            if (0 == (rawValue & 0x80)) {
                result += (ulong)(rawValue & 0x7F) << 28;
                return (long)result;
            }
            throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
        }

        internal byte ReadVarUint1()
        {
            byte rawValue = ReadByte();
            if (0!= (rawValue & 0x80)) {
                throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
            }
            return (byte)(rawValue & (byte)0x01);
        }

        internal byte ReadVarUint7()
        {
            byte rawValue = ReadByte();
            if (0!= (rawValue & 0x80)) {
                throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
            }
            return (byte)(rawValue & (byte)0x7F);
        }

        internal uint ReadVarUint32()
        {
            uint result = 0;
            byte rawValue;
            for (int index = 0; index < 4; index++) {
                rawValue = ReadByte();
                result += (uint)(rawValue & 0x7F) << (7 * index);
                if (0 == (rawValue & 0x80)) {
                    return result;
                }
            }
            rawValue = ReadByte();
            if (0 == (rawValue & 0xF0)) {
                result += (uint)(rawValue & 0x0F) << 28;
                return result;
            }
            throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
        }

        private byte[] _stringDataBuffer = new byte[0];
    }
}
