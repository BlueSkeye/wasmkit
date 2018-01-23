
namespace WasmLib
{
    internal class ImportedGlobalDefinition : ImportedItemDefinition
    {
        internal ImportedGlobalDefinition(string name, BuiltinLanguageType type, bool mutable)
            : base(name, ExternalKind.Global)
        {
            Type = type;
            Mutable = mutable;
        }

        public bool Mutable { get; protected set; }

        public BuiltinLanguageType Type { get; protected set; }
    }
}
