﻿using System;

namespace WasmLib.Bytecode
{
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

        internal BuiltinLanguageType ValueType { get; private set; }

        internal static IfInstruction Create(BinaryParsingReader reader)
        {
            BuiltinLanguageType valueType = reader.ReadValueType(true);
            IfInstruction result = _knownTests[(int)valueType + 128];

            if (null != result) { _reuseCount++; }
            else {
                result = new IfInstruction() { ValueType = valueType };
                _knownTests[(int)valueType + 128] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() +  " " + ValueType.ToString();
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            if (BuiltinLanguageType.I32 != poppedType) {
                context.AddError(string.Format(
                    "Expected an I32 on top of the stack for if decision. Found an {0}.",
                    poppedType.ToString()));
                return false;
            }
            context.CreateLabel(ValueType, true);
            return true;
        }

        private static int _reuseCount = 0;
        private static IfInstruction[] _knownTests = new IfInstruction[256];
    }
}
