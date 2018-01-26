using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class UnreachableInstruction : Instruction
    {
        static UnreachableInstruction()
        {
            Singleton = new UnreachableInstruction();
        }

        private UnreachableInstruction()
            : base(OpCodes.Unreachable)
        {
        }

        internal static UnreachableInstruction Singleton { get; private set; }

        internal override bool Validate(ValidationContext context)
        {
            return true; ;
        }
    }
}
