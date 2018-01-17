using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WasmLib.Bytecode;

namespace WasmLib
{
    /// <summary>Applies validation rules on a module.</summary>
    internal class WasmModuleValidator
    {
        internal WasmModuleValidator(WasmModule module)
        {
            if (null == module) { throw new ArgumentNullException("module"); }
            _module = module;
        }

        internal bool Validate()
        {
            bool result = true;


            foreach(FunctionDefinition function in _module.EnumerateFunctions()) {
                result &= Validate(function);
            }
            return result;
        }

        private bool Validate(FunctionDefinition function)
        {
            throw new NotImplementedException();
            //foreach(Instruction instruction in function.EnumerateInstructions()) {
            //}
        }

        private WasmModule _module;
    }
}
