using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class MemoryAccessInstruction : Instruction
    {
        private MemoryAccessInstruction(OpCodes opCode)
            : base(opCode)
        {
            return;
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

        internal static MemoryAccessInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            // uint localIndex = reader.ReadVarUint32();
            RawValueEncoding rawValue;
            bool store = false;
            switch (opcode)
            {
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
            return new MemoryAccessInstruction(opcode)
            {
                _rawValue = rawValue,
                IsStore = store,
                IsLoad = !store,
                Offset = offset,
                Align = flags
            };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}, o=0x{1:X8}, o=0x{2:X8}", (byte)_rawValue, Offset, Align);
        }

        internal override bool Validate(ValidationContext context)
        {
            throw new NotImplementedException();
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
}
