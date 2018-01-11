using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib
{
    public class WasmModule
    {
        private WasmModule()
        {
        }

        /// <summary>Create a module from an input stream.</summary>
        /// <param name="from">A readable, seekable module.</param>
        /// <returns>The new module instance.</returns>
        public static WasmModule Create(Stream from)
        {
            if (null == from) { throw new ArgumentNullException("from"); }
            if (!from.CanRead) { throw new ArgumentException("non readable stream"); }
            if (!from.CanSeek) { throw new ArgumentException("non seekable stream"); }
            WasmModule result = new WasmModule();
            return result.Parse(from) ? result : null;
        }

        /// <summary></summary>
        /// <param name="from"></param>
        /// <remarks>Module specification is defined by :
        /// http://webassembly.org/docs/binary-encoding/#high-level-structure
        /// </remarks>
        private bool Parse(Stream from)
        {
            using (BinaryParsingReader reader = new BinaryParsingReader(from)) {
                ParseMagicNumberAndVersion(reader);
                WasmModuleSection section;
                while (ParseSection(reader, out section)) {
                    RegisterParsedSection(section);
                }
            }
            return true;
        }

        private void ParseMagicNumberAndVersion(BinaryParsingReader reader)
        {
            int magicNumber = reader.ReadInt32();
            if (MagicModuleNumber != magicNumber) {
                throw new WasmParsingException(string.Format(
                    ParsingErrorMessages.MissingModuleMagicNumber, MagicModuleNumber, magicNumber));
            }
            int version = reader.ReadInt32();
            if ((MaximumSupportedModuleFormatVersion < version)
                || (MinimumSupportedModuleFormatVersion > version))
            {
                throw new WasmParsingException(string.Format(
                    ParsingErrorMessages.UnsupportedModuleFormatVersion,
                    version, MinimumSupportedModuleFormatVersion, MaximumSupportedModuleFormatVersion));
            }
        }

        /// <summary></summary>
        /// <param name="reader"></param>
        /// <param name="newSection"></param>
        /// <returns>false if we reached end of file. If false is returned, <paramref name="newSection"/>
        /// is guaranteed to be a null reference on return. If result is true, the caller is
        /// expected to invoke us again for next section discovery.</returns>
        private bool ParseSection(BinaryParsingReader reader, out WasmModuleSection newSection)
        {
            byte sectionCode;

            try { sectionCode = reader.ReadVarUint7(); }
            catch (EndOfStreamException e) {
                // This is expected. Section parsing is complete.
                newSection = null;
                return false;
            }
            try {
                // The specification is not very clear about the fact that the value of the 
                // payload_len field from section header doesn't include neither this field
                // length, nor the previous section code.
                uint remainingBytesCount = reader.ReadVarUint32();
                string sectionName = null;
                if (0 == sectionCode) {
                    long initialStreamPosition = reader.BaseStream.Position;
                    uint sectionNameLength = reader.ReadVarUint32();
                    byte[] rawName = new byte[sectionNameLength];
                    reader.Read(rawName, 0, (int)sectionNameLength);
                    sectionName = Encoding.UTF8.GetString(rawName);
                    remainingBytesCount -= (uint)(reader.BaseStream.Position - initialStreamPosition);
                    newSection = new WasmModuleSection(sectionName);
                }
                else { newSection = new WasmModuleSection(sectionCode); }
                byte[] sectionData = new byte[remainingBytesCount];
                reader.Read(sectionData, 0, sectionData.Length);
                return true;
            }
            catch (EndOfStreamException e) {
                // This is unexpected. We are stopping in the midst of a section.
                throw new WasmParsingException(ParsingErrorMessages.IncompleteSectionEncountered);
            }
        }

        /// <summary>Register a new section in the module. This method is only valid for an
        /// existing wasm module because we enforce the expected section order defined in the
        /// specification.</summary>
        /// <param name="section">The section to be registered.</param>
        private void RegisterParsedSection(WasmModuleSection section)
        {
            // TODO : Implement section ordering.
            _sections.Add(section);
        }

        private const int MagicModuleNumber = 0x6D736100; // '\0asm'
        private const int MaximumSupportedModuleFormatVersion = 1;
        private const int MinimumSupportedModuleFormatVersion = 1;
        private List<WasmModuleSection> _sections = new List<WasmModuleSection>();
    }
}
