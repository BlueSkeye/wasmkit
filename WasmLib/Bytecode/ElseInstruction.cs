
namespace WasmLib.Bytecode
{
    internal class ElseInstruction : Instruction
    {
        static ElseInstruction()
        {
            Singleton = new ElseInstruction();
        }

        private ElseInstruction()
            : base(OpCodes.Else)
        {
            return;
        }

        internal static ElseInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            return context.ValidateElseOrEnd(OpCodes.Else);
        }
    }
}
