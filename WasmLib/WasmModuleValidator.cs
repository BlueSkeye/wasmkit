#define TRACE_CODE
using System;

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

            Console.WriteLine("Validating functions body.");
            foreach (FunctionDefinition function in _module.EnumerateFunctions()) {
                result &= Validate(context, function);
            }
            return result;
        }

        private bool Validate(ValidationContext context, FunctionDefinition function)
        {
            bool trace = false;
            _functionIndex += 1;
            while (true) {
                context.Reset(function);
#if TRACE_CODE
                if (trace) {
                    Console.WriteLine("#{0} ==================================", _functionIndex);
                }
#endif
                foreach (Instruction instruction in function.EnumerateInstructions()) {
#if TRACE_CODE
                    if (trace) {
                        Console.Write((++GlobalInstructionCounter).ToString() + " : " + instruction.ToString() +
                            " | " + context.StackLabelBarrier());
                    }
#endif
                    // Must break immediately. Continuing is meaningless because the context is not
                    // accurate anymore.
                    if (!instruction.Validate(context)) { break; }
#if TRACE_CODE
                    if (trace) {
                        Console.WriteLine(" -> " + context.StackLabelBarrier());
                    }
#endif
                    context.LastInstructionWasReturn = (OpCodes.Return == instruction.OpCode);
                }
                if (0 == context.Errors.Count) { return true; }
#if TRACE_CODE
                if (!trace) {
                    trace = true;
                    continue;
                }
#endif
                Console.WriteLine();
                Console.WriteLine("Error encountered on function #{0}", function.Id);
                foreach(string error in context.Errors) {
                    Console.WriteLine(error);
                }
                return false;
            }
        }

        private static int GlobalInstructionCounter = 0;
        private static int _functionIndex = 0;
        private WasmModule _module;
    }
}
