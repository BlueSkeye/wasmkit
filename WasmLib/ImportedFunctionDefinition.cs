
namespace WasmLib
{
    internal class ImportedFunctionDefinition : ImportedItemDefinition
    {
        internal ImportedFunctionDefinition(string name, uint functionSignatureTypeIndex)
            : base(name, ExternalKind.Function)
        {
            FunctionSignatureIndex = functionSignatureTypeIndex;
            return;
        }

        internal uint FunctionSignatureIndex { get; private set; }
    }
}
