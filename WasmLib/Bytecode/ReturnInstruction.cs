
namespace WasmLib.Bytecode
{
    internal class ReturnInstruction : Instruction
    {
        static ReturnInstruction()
        {
            Singleton = new ReturnInstruction();
        }

        private ReturnInstruction()
            : base (OpCodes.Return)
        {
        }

        internal static ReturnInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType expectedType = context.FunctionReturnType;
            if (BuiltinLanguageType.EmptyBlock != expectedType) {
                BuiltinLanguageType effectiveType = context.StackPeek(0);
                if (0 == effectiveType) { return false; }
                if (effectiveType != expectedType) {
                    context.AddError(string.Format(
                        "Expected function return type is {0}. Returned value is {1}.",
                        expectedType, effectiveType));
                    return false;
                }
                BuiltinLanguageType poppedType = context.StackPop();
                if (0 == poppedType) { return false; }
            }
            int stackSize = context.StackSize;
            if (0 != stackSize) {
                context.AddError(string.Format(
                    "{0} items remaining on stack on function return.",
                    stackSize));
                return false;
            }
            return true;
        }
    }
}
