﻿using System.IO;

using WasmLib;

namespace WasmKit
{
    public class Program
    {
        public static int Main(string[] args)
        {
            WasmModule module;
            using (FileStream input = File.Open(@".\sample.wasm", FileMode.Open, FileAccess.Read)) {
                module = WasmModule.Create(input);
            }
            bool validModule = module.Validate();
            using (FileStream output = File.Open(@".\sample.wat", FileMode.Create, FileAccess.Write)) {
                module.ToText(output);
            }
            return 0;
        }
    }
}
