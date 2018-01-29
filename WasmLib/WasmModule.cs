// #define TRACE_CODE
using System;
using System.Collections.Generic;
using System.IO;

using WasmLib.Bytecode;

namespace WasmLib
{
    public class WasmModule
    {
        private WasmModule()
        {
            return;
        }

        internal int FunctionsCount
        {
            get { return _functionSignatures.Count; }
        }

        internal int GlobalsCount
        {
            get { return _globalVariables.Count; }
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

        private void DumpStatistics()
        {
            Console.WriteLine("Block reuse ratio = {0}.", BlockInstruction.ReuseRatio);
            Console.WriteLine("Branch reuse ratio = {0}.", BranchInstruction.ReuseRatio);
            Console.WriteLine("I32 constant reuse ratio = {0}.", ConstantValueInstruction<int>.ReuseRatio);
            Console.WriteLine("I64 constant reuse ratio = {0}.", ConstantValueInstruction<long>.ReuseRatio);
            Console.WriteLine("F32 constant reuse ratio = {0}.", ConstantValueInstruction<float>.ReuseRatio);
            Console.WriteLine("F64 constant reuse ratio = {0}.", ConstantValueInstruction<double>.ReuseRatio);
            Console.WriteLine("Global accessor reuse ratio = {0}.", GlobalAccessorInstruction.ReuseRatio);
            Console.WriteLine("If reuse ratio = {0}.", IfInstruction.ReuseRatio);
            Console.WriteLine("Local accessor reuse ratio = {0}.", LocalAccessorInstruction.ReuseRatio);
            Console.WriteLine("Loop reuse ratio = {0}.", LoopInstruction.ReuseRatio);
            Console.WriteLine("{0} memory access instructions.", MemoryAccessInstruction.InstructionsCount);
            Console.WriteLine("{0} memory control instructions.", MemoryControlInstruction.UseCount);
        }

        public IEnumerable<FunctionDefinition> EnumerateFunctions()
        {
            foreach (FunctionDefinition function in _functions) { yield return function; }
        }

        internal IEnumerable<ImportedFunctionDefinition> EnumerateFunctionImports()
        {
            foreach(ImportedFunctionDefinition function in EnumerateImports<ImportedFunctionDefinition>(ExternalKind.Function)) {
                yield return function;
            }
        }

        internal IEnumerable<ImportedGlobalDefinition> EnumerateGlobalImports()
        {
            foreach(ImportedGlobalDefinition global in EnumerateImports<ImportedGlobalDefinition>(ExternalKind.Global)) {
                yield return global;
            }
        }

        internal IEnumerable<ImportedTableDefinition> EnumerateGlobalTables()
        {
            foreach (ImportedTableDefinition table in EnumerateImports<ImportedTableDefinition>(ExternalKind.Table)) {
                yield return table;
            }
        }

        internal IEnumerable<GlobalVariable> EnumerateGlobalVariables()
        {
            foreach (GlobalVariable variable in _globalVariables) { yield return variable; }
        }

        private IEnumerable<T> EnumerateImports<T>(ExternalKind kind)
            where T : ImportedItemDefinition
        {
            foreach(List<ImportedItemDefinition> definitions in _perModuleImportedItems.Values) {
                foreach(ImportedItemDefinition definition in definitions) {
                    if (kind == definition.Kind) {
                        yield return (T)definition;
                    }
                }
            }
        }

        internal FunctionSignature GetFunctionSignature(uint index)
        {
            return (_functionSignatures.Count <= index) ? null : _functionSignatures[(int)index];
        }

        /// <summary></summary>
        /// <param name="from"></param>
        /// <remarks>Module specification is defined by :
        /// http://webassembly.org/docs/binary-encoding/#high-level-structure
        /// </remarks>
        private bool Parse(Stream from)
        {
            using (BinaryParsingReader reader = new BinaryParsingReader(from)) {
                _decoder = new InstructionDecoder(reader);
                ParseMagicNumberAndVersion(reader);
                while (ParseSection(reader)) ;
            }
            DumpStatistics();
            return true;
        }

        private void ParseCodeSection(BinaryParsingReader reader, uint payloadSize)
        {
            uint functionBodyCount = reader.ReadVarUint32();
            if (functionBodyCount != _functions.Count) {
                throw new WasmParsingException(
                    ParsingErrorMessages.FunctionDefVsBodyCountMismatch);
            }
            for (uint functionIndex = 0; functionIndex < functionBodyCount; functionIndex++) {
                ParseFunctionBody(reader, _functions[(int)functionIndex]);
            }
        }

        private void ParseDataSection(BinaryParsingReader reader, uint payloadSize)
        {
            uint dataSegmentsCount = reader.ReadVarUint32();
            _dataSegments = new List<DataSegment>();
            for (uint index = 0; index < dataSegmentsCount; index++) {
                _dataSegments.Add(DataSegment.Create(reader));
            }
            return;
        }

        private void ParseElementSection(BinaryParsingReader reader, uint payloadSize)
        {
            uint elementsCount = reader.ReadVarUint32();

            for (uint index = 0; index < elementsCount; index++) {
                uint tableIndex = reader.ReadVarUint32();
                List<Instruction> initializer = reader.ReadInitializationExpression();
                uint elementCount = reader.ReadVarUint32();
                for(int elementIndex = 0; elementIndex < elementCount; elementIndex++) {
                    uint element = reader.ReadVarUint32();
                }
            }
        }

        private void ParseExportSection(BinaryParsingReader reader, uint payloadSize)
        {
            long startPosition = payloadSize;
            uint exportsCount = reader.ReadVarUint32();

            for (uint index = 0; index < exportsCount; index++) {
                string exportName = reader.ReadLengthPrefixedUTF8String();
                // TODO : Interpret below fields.
                byte exportKind = reader.ReadByte();
                uint indexSpaceIndex = reader.ReadVarUint32();
            }
        }

        private void ParseFunctionBody(BinaryParsingReader reader, FunctionDefinition target)
        {
            uint bodySize = reader.ReadVarUint32();
            long startPosition = reader.BaseStream.Position;
            uint localsEntryCount = reader.ReadVarUint32();
            lock (_currentFunctionLocals) {
                if (0 < localsEntryCount) {
                    _currentFunctionLocals.Clear();
                    for (int localEntryIndex = 0; localEntryIndex < localsEntryCount; localEntryIndex++) {
                        uint variablesCount = reader.ReadVarUint32();
                        BuiltinLanguageType valuesType = (BuiltinLanguageType)reader.ReadValueType();
                        for(uint index = 0; index < variablesCount; index++) {
                            _currentFunctionLocals.Add(valuesType);
                        }
                    }
                    target.SetLocalTypes(_currentFunctionLocals.ToArray());
                }
            }
            List<Instruction> instructions = reader.ReadFunctionBodyCode();
            long endPosition = reader.BaseStream.Position;
            uint trueSize = (uint)(endPosition - startPosition);
#if TRACE_CODE
            foreach(Instruction instruction in instructions) {
                Console.WriteLine(instruction.ToString());
            }
            Console.WriteLine("{0} ================================================", target.Id);
#endif
            if (bodySize != trueSize) {
                throw new WasmParsingException(string.Format(
                    ParsingErrorMessages.FunctionBodyLengthMismatch, bodySize, trueSize));
            }
            target.SetBody(instructions);
            return;
        }

        private void ParseFunctionSection(BinaryParsingReader reader, uint payloadSize)
        {
            long startPosition = payloadSize;
            uint functionsCount = reader.ReadVarUint32();

            for(uint index = 0; index < functionsCount; index++) {
                uint signatureIndex = reader.ReadVarUint32();
                _functions.Add(new FunctionDefinition(_functionSignatures[(int)signatureIndex]));
            }
        }

        private void ParseGlobalSection(BinaryParsingReader reader, uint payloadSize)
        {
            uint globalVariablesCount = reader.ReadVarUint32();

            for (uint index = 0; index < globalVariablesCount; index++) {
                bool mutable;
                BuiltinLanguageType contentType = reader.ReadGlobaltype(out mutable);
                List<Instruction> initializer = reader.ReadInitializationExpression();

                GlobalVariable variable = new GlobalVariable(contentType, mutable, initializer);
                _globalVariables.Add(variable);
            }
        }

        private void ParseImportSection(BinaryParsingReader reader, uint payloadSize)
        {
            long startPosition = payloadSize;
            uint importsCount = reader.ReadVarUint32();

            for(uint index = 0; index < importsCount; index++) {
                string moduleName = reader.ReadLengthPrefixedUTF8String();
                List<ImportedItemDefinition> imports;
                if (!_perModuleImportedItems.TryGetValue(moduleName, out imports)) {
                    imports = new List<ImportedItemDefinition>();
                    _perModuleImportedItems.Add(moduleName, imports);
                }
                string fieldName = reader.ReadLengthPrefixedUTF8String();
                ExternalKind kind = (ExternalKind)reader.ReadVarUint7();
                bool maxPresent;
                uint initialLength;
                uint maximumLength;
                switch (kind) {
                    case ExternalKind.Function:
                        imports.Add(new ImportedFunctionDefinition(fieldName, reader.ReadVarUint32()));
                        continue;
                    case ExternalKind.Global:
                        bool mutable;
                        BuiltinLanguageType contentType = reader.ReadGlobaltype(out mutable);
                        imports.Add(new ImportedGlobalDefinition(fieldName, contentType, mutable));
                        continue;
                    case ExternalKind.Memory:
                        maxPresent = (0 != reader.ReadVarUint1());
                        initialLength = reader.ReadVarUint32();
                        maximumLength = maxPresent ? reader.ReadVarUint32() : 0;
                        imports.Add(new ImportedMemoryDefinition(fieldName, initialLength, maxPresent ? (uint?)maximumLength : null));
                        continue;
                    case ExternalKind.Table:
                        BuiltinLanguageType elementType = (BuiltinLanguageType)reader.ReadVarInt7();
                        if (BuiltinLanguageType.AnyFunc != elementType) {
                            throw new WasmParsingException(string.Format(ParsingErrorMessages.UnexpectedBuiltinType,
                                elementType, BuiltinLanguageType.AnyFunc));
                        }
                        maxPresent = (0 != reader.ReadVarUint1());
                        initialLength = reader.ReadVarUint32();
                        maximumLength = maxPresent ? reader.ReadVarUint32() : 0;
                        imports.Add(new ImportedTableDefinition(fieldName, elementType, initialLength,
                            maxPresent ? (uint?)maximumLength : null));
                        continue;
                    default:
                        throw new WasmParsingException(string.Format(
                            ParsingErrorMessages.UnrecognizedExternalKind, kind));
                }
            }
            // TODO Check for match between payloadSize and current stream position.
            return;
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
        /// <returns>false if we reached end of file. If result is true, the caller is
        /// expected to invoke us again for next section discovery.</returns>
        private bool ParseSection(BinaryParsingReader reader)
        {
            SectionTypes sectionType;

            try {
                sectionType = (SectionTypes)reader.ReadVarUint7();
                if (!Enum.IsDefined(typeof(SectionTypes), sectionType)) {
                    throw new WasmParsingException(string.Format(
                        ParsingErrorMessages.UnknownSectionType, sectionType));
                }
                Console.WriteLine("Parsing {0} section.", sectionType);
            }
            catch (EndOfStreamException e) {
                // This is expected. Section parsing is complete.
                return false;
            }
            try {
                ParseSection(reader, sectionType);
                return true;
            }
            catch (EndOfStreamException e) {
                // This is unexpected. We are stopping in the midst of a section.
                throw new WasmParsingException(ParsingErrorMessages.IncompleteSectionEncountered);
            }
        }

        private WasmModuleSection ParseSection(BinaryParsingReader reader, SectionTypes type)
        {
            WasmModuleSection result = null;
            // The specification is not very clear about the fact that the value of the 
            // payload_len field from section header doesn't include neither this field
            // length, nor the previous section code. We have to read the payload_data
            // field definition to understand this.
            uint payloadSize = reader.ReadVarUint32();

            string sectionName = null;
            if (0 == type) {
                long initialStreamPosition = reader.BaseStream.Position;
                sectionName = reader.ReadLengthPrefixedUTF8String();
                payloadSize -= (uint)(reader.BaseStream.Position - initialStreamPosition);
                result = new WasmModuleSection(sectionName);

                byte[] sectionData = new byte[payloadSize];
                reader.Read(sectionData, 0, sectionData.Length);
                result.SetRawData(sectionData);
                _customSections.Add(result);
                return result;
            }
            // This test will ensure a non custom section can't be parsed twice as per
            // the specification. We seems to be more stringent here than the specification
            // that seems to state that the section COULD be ignored.
            if (type <= _lastParsedSectionType) {
                throw new WasmParsingException(string.Format(ParsingErrorMessages.InvalidSectionDefinitionOrder,
                    type, _lastParsedSectionType));
            }
            switch (type) {
                case SectionTypes.Code:
                    ParseCodeSection(reader, payloadSize);
                    break;
                case SectionTypes.Data:
                    ParseDataSection(reader, payloadSize);
                    break;
                case SectionTypes.Element:
                    ParseElementSection(reader, payloadSize);
                    break;
                case SectionTypes.Export:
                    ParseExportSection(reader, payloadSize);
                    break;
                case SectionTypes.Function:
                    ParseFunctionSection(reader, payloadSize);
                    break;
                case SectionTypes.Global:
                    ParseGlobalSection(reader, payloadSize);
                    break;
                case SectionTypes.Import:
                    ParseImportSection(reader, payloadSize);
                    break;
                case SectionTypes.Memory:
                    throw new NotImplementedException();
                case SectionTypes.Start:
                    throw new NotImplementedException();
                case SectionTypes.Table:
                    throw new NotImplementedException();
                case SectionTypes.Type:
                    ParseTypeSection(reader, payloadSize);
                    break;
                default:
                    throw new NotImplementedException();
            }
            _lastParsedSectionType = type;
            return result;
        }

        private void ParseTypeSection(BinaryParsingReader reader, uint payloadSize)
        {
            long startPosition = payloadSize;
            uint functionsCount = reader.ReadVarUint32();

            for (uint index = 0; index < functionsCount; index++) {
                FunctionSignature function = FunctionSignature.Parse(reader);
                _functionSignatures.Add(function);
            }
            // TODO Check for match between payloadSize and current stream position.
            return;
        }

        public void ToText(Stream output)
        {
            new WasmModuleTextWriter(this).WriteTo(output);
        }

        /// <summary>Applies the validation process on this module.</summary>
        /// <returns>True if module is considered valid. False otherwise.</returns>
        public bool Validate()
        {
            return new WasmModuleValidator(this).Validate();
        }

        private const int MagicModuleNumber = 0x6D736100; // '\0asm'
        private const int MaximumSupportedModuleFormatVersion = 1;
        private const int MinimumSupportedModuleFormatVersion = 1;
        private List<BuiltinLanguageType> _currentFunctionLocals = new List<BuiltinLanguageType>();
        private List<WasmModuleSection> _customSections = new List<WasmModuleSection>();
        private List<DataSegment> _dataSegments;
        private InstructionDecoder _decoder;
        private List<FunctionDefinition> _functions = new List<FunctionDefinition>();
        private List<FunctionSignature> _functionSignatures = new List<FunctionSignature>();
        private List<GlobalVariable> _globalVariables = new List<GlobalVariable>();
        private SectionTypes _lastParsedSectionType = SectionTypes.Custom;
        private Dictionary<string, List<ImportedItemDefinition>> _perModuleImportedItems =
            new Dictionary<string, List<ImportedItemDefinition>>();

        internal class DataSegment
        {
            private DataSegment()
            {
                return;
            }

            internal byte[] Data { get; private set; }

            internal uint LinearMemoryIndex { get; private set; }

            internal List<Instruction> OffsetExpression { get; private set; }

            internal static DataSegment Create(BinaryParsingReader reader)
            {
                uint linearMemoryIndex = reader.ReadVarUint32();
                List<Instruction> offsetExpression = reader.ReadInitializationExpression();
                uint dataSize = reader.ReadVarUint32();
                byte[] data = reader.ReadBytes((int)dataSize);
                return new DataSegment() { LinearMemoryIndex = linearMemoryIndex, Data = data, OffsetExpression = offsetExpression };
            }
        }

        internal class Element
        {
            private Element()
            {
                return;
            }

            internal uint[] Elements { get; private set; }

            internal uint TableIndex { get; private set; }

            internal List<Instruction> OffsetExpression { get; private set; }

            internal static Element Create(BinaryParsingReader reader)
            {
                uint tableIndex = reader.ReadVarUint32();
                List<Instruction> offsetExpression = reader.ReadInitializationExpression();
                uint elementsCount = reader.ReadVarUint32();
                uint[] elements = new uint[elementsCount];
                for(int index = 0; index < elementsCount; index++) {
                    elements[index] = reader.ReadVarUint32();
                }
                return new Element() { Elements = elements, OffsetExpression = offsetExpression, TableIndex = tableIndex };
            }
        }
    }
}
