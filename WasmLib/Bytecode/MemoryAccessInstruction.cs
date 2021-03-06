﻿using System;
using System.Collections.Generic;

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

        internal static uint InstructionsCount { get; private set; }

        public bool IsLoad { get; private set; }

        public bool IsStore
        {
            get { return !IsLoad; }
        }

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
            InstructionsCount++;
            MemoryAccessInstruction result;

            // if (_knownInstructions.TryGetValue(opcode, out result)) { return result; }
            result = new MemoryAccessInstruction(opcode) {
                _rawValue = rawValue,
                IsLoad = !store,
                Offset = offset,
                Align = flags
            };
            // _knownInstructions.Add(opcode, result);
            return result;
        }

        // private static Dictionary<OpCodes, MemoryAccessInstruction> _knownInstructions = new Dictionary<OpCodes, MemoryAccessInstruction>();

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2}, o=0x{1:X8}, o=0x{2:X8}", (byte)_rawValue, Offset, Align);
        }

        internal override bool Validate(ValidationContext context)
        {
            // We assume mems[0] always exist.
            BuiltinLanguageType transferedValueType = 0;

            int maxAlignment;

            switch (OpCode) {
                case OpCodes.I32Load8_s:
                case OpCodes.I32Load8_u:
                case OpCodes.I32Store8:
                    transferedValueType = BuiltinLanguageType.I32;
                    maxAlignment = 1;
                    break;
                case OpCodes.I64Load8_s:
                case OpCodes.I64Load8_u:
                case OpCodes.I64Store8:
                    transferedValueType = BuiltinLanguageType.I64;
                    maxAlignment = 1;
                    break;
                case OpCodes.I32Load16_s:
                case OpCodes.I32Load16_u:
                case OpCodes.I32Store16:
                    transferedValueType = BuiltinLanguageType.I32;
                    maxAlignment = 2;
                    break;
                case OpCodes.I64Load16_s:
                case OpCodes.I64Load16_u:
                case OpCodes.I64Store16:
                    transferedValueType = BuiltinLanguageType.I64;
                    maxAlignment = 2;
                    break;
                case OpCodes.I32Load:
                case OpCodes.I32Store:
                    transferedValueType = BuiltinLanguageType.I32;
                    maxAlignment = 4;
                    break;
                case OpCodes.F32Load:
                case OpCodes.F32Store:
                    transferedValueType = BuiltinLanguageType.F32;
                    maxAlignment = 4;
                    break;
                case OpCodes.I64Load:
                case OpCodes.I64Store:
                case OpCodes.I64Load32_s:
                case OpCodes.I64Load32_u:
                case OpCodes.I64Store32:
                    transferedValueType = BuiltinLanguageType.I64;
                    maxAlignment = 8;
                    break;
                case OpCodes.F64Load:
                case OpCodes.F64Store:
                    transferedValueType = BuiltinLanguageType.F64;
                    maxAlignment = 8;
                    break;
                default:
                    throw new ApplicationException();
            }
            if ((0 < Align) && (2 << (int)(Align - 1)) > maxAlignment) {
                context.AddError(string.Format("Align value {0} exceeds max allowed value {1} for type {2}",
                    Align, maxAlignment, transferedValueType.ToString()));
                return false;
            }

            if (IsStore) {
                BuiltinLanguageType valueType = context.StackPop();
                if (0 == valueType) { return false; }
                if (transferedValueType != valueType) {
                    context.AddError(string.Format("Expected an {0} value on the stack. Found an {1}",
                        transferedValueType.ToString(), valueType.ToString()));
                    return false;
                }
            }
            BuiltinLanguageType memoryAddressType = context.StackPop();
            if (0 == memoryAddressType) { return false; }
            if (BuiltinLanguageType.I32 != memoryAddressType) {
                context.AddError(string.Format("Expected an I32 on the stack. Found an {0}",
                    memoryAddressType.ToString()));
                return false;
            }
            if (IsLoad) { context.StackPush(transferedValueType); }
            return true;
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
