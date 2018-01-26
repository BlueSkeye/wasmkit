
namespace WasmLib
{
    internal class ImportedMemoryDefinition : ImportedItemDefinition
    {
        internal ImportedMemoryDefinition(string name, uint initialLength, uint? maximumLength)
            : base(name, ExternalKind.Memory)
        {
            InitialLength = initialLength;
            MaximumLength = maximumLength;
        }

        public uint InitialLength { get; protected set; }

        public uint? MaximumLength { get; protected set; }
    }
}
