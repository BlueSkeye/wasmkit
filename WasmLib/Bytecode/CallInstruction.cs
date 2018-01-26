using System;

namespace WasmLib.Bytecode
{
    internal class CallInstruction : Instruction
    {
        private CallInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal byte ExpectedSignature { get; private set; }

        internal bool Indirect { get; private set; }

        internal uint ItemIndex { get; private set; }

        internal static CallInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint itemIndex = reader.ReadVarUint32();
            bool indirect = (OpCodes.CallIndirect == opcode);
            byte expectedSignature = indirect ? reader.ReadVarUint1() : (byte)0;
            return new CallInstruction(opcode) { ExpectedSignature = expectedSignature, Indirect = indirect, ItemIndex = itemIndex };
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X8} {1}", ItemIndex, Indirect ? "(I)" : string.Empty);
        }

        internal override bool Validate(ValidationContext context)
        {
            FunctionSignature signature;
            if (Indirect) {
                signature = context.GetIndirectFunctionSignature(ItemIndex);
                if (null == signature) { return false; }
                BuiltinLanguageType poppedType = context.StackPop();
                if (BuiltinLanguageType.I32 != poppedType) {
                    context.AddError(string.Format("Type {0} on top of stack doesn't match expected I32 type.", poppedType));
                    return false;
                }
            }
            else {
                signature = context.GetFunctionSignature(ItemIndex);
                if (null == signature) { return false; }
            }
            int parameterIndex = 0;
            foreach(BuiltinLanguageType parameterType in signature.EnumerateParameters()) {
                BuiltinLanguageType valueType = context.StackPeek(parameterIndex++);
                if (parameterType != valueType) {
                    context.AddError(string.Format(
                        "Parameter #{0} value has type {1} which doesn't match expected type {2}",
                        parameterIndex, valueType, parameterType));
                    return false;
                }
            }
            // Pop parameter values
            while (0 < parameterIndex--) { context.StackPop(); }
            // Push return type.
            BuiltinLanguageType returnType = signature.ReturnType;
            if (BuiltinLanguageType.EmptyBlock != returnType) { context.StackPush(returnType); }
            return true;
        }
    }
}
