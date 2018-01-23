
namespace WasmLib.Bytecode
{
    internal class EndInstruction : Instruction
    {
        internal EndInstruction()
            : base(OpCodes.End)
        {
        }

        internal override bool Validate(ValidationContext context)
        {
            return context.ValidateElseOrEnd(OpCodes.End);
        }
    }
}
