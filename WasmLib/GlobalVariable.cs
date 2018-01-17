using System.Collections.Generic;

using WasmLib.Bytecode;

namespace WasmLib
{
    internal class GlobalVariable
    {
        internal GlobalVariable(BuiltinLanguageType type, bool mutable, List<Instruction> initializer)
        {
            Type = type;
            Mutable = mutable;
            Initializer = initializer;
        }

        internal List<Instruction> Initializer { get; private set; }

        internal bool Mutable { get; private set; }

        internal BuiltinLanguageType Type { get; private set; }
    }
}
