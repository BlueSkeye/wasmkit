
namespace WasmLib.Bytecode
{
    internal class EndInstruction : Instruction
    {
        static EndInstruction()
        {
            Singleton = new EndInstruction();
        }

        private EndInstruction()
            : base(OpCodes.End)
        {
            return;
        }

        internal static EndInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            return context.ValidateElseOrEnd(OpCodes.End);
        }
    }
}
