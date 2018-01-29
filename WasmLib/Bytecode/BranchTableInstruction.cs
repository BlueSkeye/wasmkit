using System;
using System.Text;

namespace WasmLib.Bytecode
{
    internal class BranchTableInstruction : Instruction
    {
        private BranchTableInstruction()
            : base(OpCodes.BrTable)
        {
            return;
        }

        internal uint DefaultTarget { get; private set; }

        internal uint[] Targets { get; private set; }

        internal static BranchTableInstruction Create(BinaryParsingReader reader)
        {
            uint targetCount = reader.ReadVarUint32();
            uint[] targets = new uint[targetCount];
            for (int index = 0; index < targetCount; index++)
            {
                targets[index] = reader.ReadVarUint32();
            }
            uint defaultTarget = reader.ReadVarUint32();
            return new BranchTableInstruction() { DefaultTarget = defaultTarget, Targets = targets };
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(OpCode.ToString() + " ");
            bool firstTarget = true;
            foreach(uint target in Targets) {
                if (firstTarget) { firstTarget = false; }
                else { builder.Append(","); }
                builder.Append(target.ToString());
            }
            return builder.ToString();
        }

        internal override bool Validate(ValidationContext context)
        {
            Tuple<BuiltinLanguageType, int, bool> label;
            uint defaultLabel = Targets[Targets.Length - 1];
            if (!context.GetLabel(defaultLabel, out label)) {
                context.AddError(string.Format("Table based branching as undefined default label {0}.", defaultLabel));
                return false;
            }
            BuiltinLanguageType defaultType = label.Item1;
            foreach(uint target in Targets) {
                if (!context.GetLabel(target, out label)) {
                    context.AddError(string.Format("Table based branching as undefined label {0}.", target.ToString()));
                    return false;
                }
                if (label.Item1 != defaultType) {
                    context.AddError(string.Format("Unconsistent label {0} typing. Default type {1}, current type {2}.",
                        target.ToString(), defaultType.ToString(), label.Item1.ToString()));
                    return false;
                }
            }
            BuiltinLanguageType poppedType = context.StackPop();
            if (0 == poppedType) {
                context.AddError("Table based branching found with empty stack.");
                return false;
            }
            if (BuiltinLanguageType.I32 != poppedType) {
                context.AddError("Table based branching requires I32 argument.");
                return false;
            }
            if (BuiltinLanguageType.EmptyBlock != defaultType) {
                poppedType = context.StackPop();
                if (0 == poppedType) {
                    context.AddError(string.Format("Table based branching found with missing {0} value on stack.",
                        defaultType.ToString()));
                    return false;
                }
                if (defaultType != poppedType) {
                    context.AddError(string.Format("Table based branching expected an {0} value on stack. Found a {1}",
                        defaultType.ToString(), poppedType.ToString()));
                    return false;
                }
            }
            return true;
        }
    }
}
