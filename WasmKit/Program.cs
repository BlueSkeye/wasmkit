using System.IO;

using WasmLib;

namespace WasmKit
{
    public class Program
    {
        public static int Main(string[] args)
        {
            using (FileStream input = File.Open(@".\sample.wasm", FileMode.Open, FileAccess.Read)) {
                WasmModule.Create(input);
            }
            return 0;
        }
    }
}
