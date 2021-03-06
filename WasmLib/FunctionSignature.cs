﻿using System.Collections.Generic;

namespace WasmLib
{
    internal class FunctionSignature
    {
        private FunctionSignature()
        {
        }

        internal BuiltinLanguageType ReturnType { get; private set; }

        internal IEnumerable<BuiltinLanguageType> EnumerateParameters(bool reverseOrder = false)
        {
            if(0 == _parametersType.Count) { yield break; }
            int startIndex = reverseOrder ? _parametersType.Count - 1 : 0;
            int endIndex = reverseOrder ? 0 : _parametersType.Count - 1;
            int increment = reverseOrder ? -1 : 1;
            for(int index = startIndex; ; index += increment) {
                yield return _parametersType[index];
                if (endIndex == index) { break; }
            }
            yield break;
        }

        internal static FunctionSignature Parse(BinaryParsingReader reader)
        {
            FunctionSignature result = new FunctionSignature();
            reader.ReadAndAssertLanguageType(-32);
            uint parametersCount = reader.ReadVarUint32();

            for(uint index = 0; index < parametersCount; index++) {
                result._parametersType.Add((BuiltinLanguageType)reader.ReadValueType());
            }

            result.ReturnType = (1 == reader.ReadVarUint1())
                ? (BuiltinLanguageType)reader.ReadValueType()
                : BuiltinLanguageType.EmptyBlock;
            return result;
        }

        private List<BuiltinLanguageType> _parametersType = new List<BuiltinLanguageType>();
    }
}
