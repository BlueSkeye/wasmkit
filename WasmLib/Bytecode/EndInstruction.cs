
namespace WasmLib.Bytecode
{
    internal class EndInstruction : Instruction
    {
        private EndInstruction()
            : base(OpCodes.End)
        {
            return;
        }

        internal static EndInstruction Singleton { get; private set; }

        internal static EndInstruction Create()
        {
            lock (_globalLock) {
                return Singleton ?? (Singleton = new EndInstruction());
            }
        }

        internal override bool Validate(ValidationContext context)
        {
            return context.ValidateElseOrEnd(OpCodes.End);
        }

        private static object _globalLock = new object();
    }
}
