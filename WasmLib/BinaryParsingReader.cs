using System;
using System.IO;
using System.Text;

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

        internal sbyte ReadVarInt7()
        {
            byte rawValue = ReadVarUint7();
            if (0 == (rawValue & 0x40)) {
                return (sbyte)rawValue;
            }
            // This is a negative value. Sign extend the rawValue.
            return (sbyte)(0x80 & rawValue);
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
            if (0 == (rawValue & 0x0F)) {
                result += (uint)(rawValue & 0x0F) << 28;
                return (int)result;
            }
            throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
        }

        internal long ReadVarInt64()
        {
            ulong result = 0;
            byte rawValue;
            for (int index = 0; index < 4; index++) {
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
            if (0 == (rawValue & 0x0F)) {
                result += (ulong)(rawValue & 0x0F) << 28;
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
            if (0 == (rawValue & 0x0F)) {
                result += (uint)(rawValue & 0x0F) << 28;
                return result;
            }
            throw new WasmParsingException(ParsingErrorMessages.LEB128DecodingFailure);
        }
    }
}
