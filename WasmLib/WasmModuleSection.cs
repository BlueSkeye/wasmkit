using System;

namespace WasmLib
{
    // TODO : Make this class an abstract one and define additional per type concrete ones.
    public class WasmModuleSection
    {
        private WasmModuleSection()
        {
        }

        internal WasmModuleSection(SectionTypes type)
            : this()
        {
            if (!Enum.IsDefined(typeof(SectionTypes), type)) {
                throw new ArgumentException();
            }
            Type = type;
        }

        internal WasmModuleSection(string sectionName)
            : this()
        {
            if (string.IsNullOrEmpty(sectionName)) { throw new ArgumentNullException(); }
            Type = 0;
            Name = sectionName;
        }

        internal string Name { get; private set; }

        internal SectionTypes Type { get; private set; }

        internal void SetRawData(byte[] rawData)
        {
            _rawData = rawData;
        }

        private byte[] _rawData;
    }
}
