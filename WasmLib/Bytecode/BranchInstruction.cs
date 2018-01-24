using System;

namespace WasmLib.Bytecode
{
    internal class BranchInstruction : Instruction
    {
        private BranchInstruction(OpCodes opcode)
            : base(opcode)
        {
            return;
        }

        internal bool Conditional { get; private set; }

        internal uint Depth { get; private set; }

        internal static float ReuseRatio
        {
            get { return (float)_reuseCount / (float)(_reuseCount + _perDepthBranch.Length + _perDepthConditionalBranch.Length); }
        }

        internal static BranchInstruction Create(BinaryParsingReader reader, OpCodes opcode)
        {
            uint depth = reader.ReadVarUint32();
            bool conditional = (OpCodes.BrIf == opcode);

            if (MaxReusableDepth < depth) {
                return new BranchInstruction(opcode) { Conditional = conditional, Depth = depth };
            }
            BranchInstruction[] reuseTargets = conditional ? _perDepthConditionalBranch : _perDepthBranch;
            BranchInstruction result = reuseTargets[depth];

            if (null != result) { _reuseCount++; }
            else {
                result = new BranchInstruction(opcode) { Conditional = conditional, Depth = depth };
                reuseTargets[depth] = result;
            }
            return result;
        }

        public override string ToString()
        {
            return OpCode.ToString() + string.Format(" 0x{0:X2} {1}", Depth, Conditional ? "(C)" : string.Empty);
        }

        internal override bool Validate(ValidationContext context)
        {
            BuiltinLanguageType poppedType;
            if (Conditional) {
                poppedType = context.StackPop();
                if (BuiltinLanguageType.I32 != poppedType) {
                    context.AddError(string.Format(
                        "Expected an I32 for conditional branch evaluation. Found an {0}",
                        poppedType.ToString()));
                    return false;
                }
            }
            Tuple<BuiltinLanguageType, int, bool> label;
            if (!context.GetLabel(Depth, out label)) {
                context.AddError(string.Format(
                    "Attempt to retrieve label having relative index {0} while there is only {1} labels",
                    Depth, context.LabelsCount));
                return false;
            }
            if (BuiltinLanguageType.EmptyBlock != label.Item1) {
                poppedType = context.StackPop();
                if (label.Item1 != poppedType) {
                    context.AddError(string.Format(
                        "Attempt to exit label having relative index {0} with value having type {1} on top of stack while expecting type {2}",
                        Depth, poppedType.ToString(), label));
                    return false;
                }
            }
            return true;
        }

        private const int MaxReusableDepth = 255;
        private static int _reuseCount = 0;
        private static BranchInstruction[] _perDepthBranch = new BranchInstruction[MaxReusableDepth + 1];
        private static BranchInstruction[] _perDepthConditionalBranch = new BranchInstruction[MaxReusableDepth + 1];
    }
}
