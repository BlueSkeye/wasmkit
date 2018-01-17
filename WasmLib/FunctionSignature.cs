using System.Collections.Generic;

namespace WasmLib
{
    internal class FunctionSignature
    {
        private FunctionSignature()
        {
        }

        internal static FunctionSignature Parse(BinaryParsingReader reader)
        {
            FunctionSignature result = new FunctionSignature();
            reader.ReadAndAssertLanguageType(-32);
            uint parametersCount = reader.ReadVarUint32();

            for(uint index = 0; index < parametersCount; index++) {
                result._parametersType.Add(reader.ReadValueType());
            }

            result._returnType = (1 == reader.ReadVarUint1()) ? reader.ReadValueType() : (sbyte)0;
            return result;
        }

        private List<sbyte> _parametersType = new List<sbyte>();
        private sbyte _returnType;
    }
}
