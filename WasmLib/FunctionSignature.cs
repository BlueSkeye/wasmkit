using System.Collections.Generic;

namespace WasmLib
{
    internal class FunctionSignature
    {
        private FunctionSignature()
        {
        }

        internal BuiltinLanguageType ReturnType { get; private set; }

        internal IEnumerable<BuiltinLanguageType> EnumerateParameters()
        {
            foreach(BuiltinLanguageType type in _parametersType) { yield return type; }
        }

        internal static FunctionSignature Parse(BinaryParsingReader reader)
        {
            FunctionSignature result = new FunctionSignature();
            reader.ReadAndAssertLanguageType(-32);
            uint parametersCount = reader.ReadVarUint32();

            for(uint index = 0; index < parametersCount; index++) {
                result._parametersType.Add((BuiltinLanguageType)reader.ReadValueType());
            }

            result.ReturnType = (1 == reader.ReadVarUint1()) ? (BuiltinLanguageType)reader.ReadValueType() : 0;
            return result;
        }

        private List<BuiltinLanguageType> _parametersType = new List<BuiltinLanguageType>();
    }
}
