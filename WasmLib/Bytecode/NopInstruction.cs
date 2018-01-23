
namespace WasmLib.Bytecode
{
    internal class NopInstruction : Instruction
    {
        static NopInstruction()
        {
            Singleton = new NopInstruction();
        }

        private NopInstruction()
            : base(OpCodes.Nop)
        {
            return;
        }

        internal static NopInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            return true;
        }
    }
}
