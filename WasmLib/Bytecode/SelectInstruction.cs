
namespace WasmLib.Bytecode
{
    internal class SelectInstruction : Instruction
    {
        static SelectInstruction()
        {
            Singleton = new SelectInstruction();
        }

        private SelectInstruction()
            : base (OpCodes.Select)
        {
            return;
        }

        internal static SelectInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            if (BuiltinLanguageType.I32 != poppedType) {
                context.AddError(string.Format(
                    "Expected an I32 index on the stack. Found an {0}.",
                    poppedType));
                return false;
            }
            poppedType = context.StackPop();
            if (0 == poppedType) { return false; }
            BuiltinLanguageType otherArgument = context.StackPeek(0);
            if (0 == otherArgument) { return false; }
            if (otherArgument != poppedType) {
                context.AddError(string.Format(
                    "Unconsistent select instruction parameters. Found an {0} and an {1}.",
                    poppedType, otherArgument));
                return false;
            }
            return true;
        }
    }
}
