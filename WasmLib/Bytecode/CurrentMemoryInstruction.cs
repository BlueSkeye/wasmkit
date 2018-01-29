
namespace WasmLib.Bytecode
{
    internal class CurrentMemoryInstruction : Instruction
    {
        static CurrentMemoryInstruction()
        {
            Singleton = new CurrentMemoryInstruction();
        }

        private CurrentMemoryInstruction()
            : base (OpCodes.CurrentMemory)
        {
        }

        internal static CurrentMemoryInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            context.StackPush(BuiltinLanguageType.I32);
            return true;
        }
    }
}
