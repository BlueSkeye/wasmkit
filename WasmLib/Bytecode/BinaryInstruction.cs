
namespace WasmLib.Bytecode
{
    internal class BinaryInstruction : Instruction
    {
        internal BinaryInstruction(OpCodes opcode)
            : base(opcode)
        {
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType resultType = GetResultType();
            if (!context.ValidatePopableItemsCount(2)) { return false; }
            BuiltinLanguageType arg1type = context.StackPeek(0);
            BuiltinLanguageType arg2type = context.StackPeek(1);
            if ((resultType != arg1type) || (resultType != arg2type)) {
                context.AddError(string.Format(
                    "Invalid stack content for binary operator {0}. Stack item types {1}, {2}.",
                    OpCode, arg1type, arg2type));
                return false;
            }
            context.StackPop();
            return true;
        }
    }
}
