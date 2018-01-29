
namespace WasmLib.Bytecode
{
    internal class DropInstruction : Instruction
    {
        static DropInstruction()
        {
            Singleton = new DropInstruction();
        }

        private DropInstruction()
            : base(OpCodes.Drop)
        {
            return;
        }

        internal static DropInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            if (0 == context.StackPop()) {
                context.AddError("Drop instruction encountered while stack is empty.");
                return false;
            }
            return true;
        }
    }
}
