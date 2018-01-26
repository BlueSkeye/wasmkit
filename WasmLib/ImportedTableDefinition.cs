
namespace WasmLib
{
    internal class ImportedTableDefinition : ImportedItemDefinition
    {
        internal ImportedTableDefinition(string name, BuiltinLanguageType elementType,
            uint initialLength, uint? maximumLength)
            : base(name, ExternalKind.Table)
        {
            ElementType = elementType;
            InitialLength = initialLength;
            MaximumLength = maximumLength;
        }

        public BuiltinLanguageType ElementType { get; protected set; }

        public uint InitialLength { get; protected set; }

        public uint? MaximumLength { get; protected set; }
    }
}
