using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib.Bytecode
{
    internal class ElseInstruction : Instruction
    {
        private ElseInstruction()
            : base(OpCodes.Else)
        {
            return;
        }

        internal static ElseInstruction Singleton { get; private set; }

        internal static ElseInstruction Create()
        {
            lock (_globalLock) {
                return Singleton ?? (Singleton = new ElseInstruction());
            }
        }

        internal override bool Validate(ValidationContext context)
        {
            return context.ValidateElseOrEnd(OpCodes.Else);
        }

        private static object _globalLock = new object();
    }
}
