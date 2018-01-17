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
            ValidationContext context = new ValidationContext(_module);

            foreach (FunctionDefinition function in _module.EnumerateFunctions()) {
                result &= Validate(context, function);
            }
            return result;
        }

        private bool Validate(ValidationContext context, FunctionDefinition function)
        {
            foreach (Instruction instruction in function.EnumerateInstructions()) {
                context.Reset(function);
                instruction.Validate(context);
            }
            if (0 == context.Errors.Count) { return true; }
            Console.WriteLine("Error encountered on function #{0}", function.Id);
            foreach(string error in context.Errors) {
                Console.WriteLine(error);
            }
            return false;
        }

        private WasmModule _module;
    }
}
