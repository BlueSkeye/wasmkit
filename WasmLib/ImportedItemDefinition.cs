using System;

namespace WasmLib
{
    internal class ImportedItemDefinition
    {
        internal ImportedItemDefinition(string name, ExternalKind kind)
        {
            Kind = kind;
            ItemName = name ?? throw new ArgumentNullException("name");
        }

        public ExternalKind Kind { get; protected set; }

        public string ItemName { get; protected set; }
    }
}
