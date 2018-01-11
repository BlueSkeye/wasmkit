using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WasmLib
{
    internal class WasmModuleSection
    {
        private WasmModuleSection()
        {
        }

        internal WasmModuleSection(int sectionId)
            : this()
        {
            Id = sectionId;
        }

        internal WasmModuleSection(string sectionName)
            : this()
        {
            if (string.IsNullOrEmpty(sectionName)) { throw new ArgumentNullException(); }
            Id = 0;
            Name = sectionName;
        }

        internal int Id { get; private set; }

        internal string Name { get; private set; }
    }
}
