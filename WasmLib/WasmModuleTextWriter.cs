using System;
using System.IO;
using System.Text;

namespace WasmLib
{
    internal class WasmModuleTextWriter
    {
        internal WasmModuleTextWriter(WasmModule module)
        {
            _module = module ?? throw new ArgumentNullException();
            return;
        }

        internal void WriteTo(Stream output)
        {
            if (null == output) { throw new ArgumentNullException(); }
            if (!output.CanWrite) { throw new ArgumentException(); }

            using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8, 65536, true)) {
                writer.WriteLine("(module");
                // There is no requirement for items order inside a module.
                for(int index = 0; index < _module.FunctionsCount; index++) {
                    writer.Write("(type ");
                    FunctionSignature signature = _module.GetFunctionSignature((uint)index);
                    WriteTo(writer, signature);
                    writer.WriteLine(")");
                }
                writer.WriteLine(")");
            }
            return;
        }

        internal void WriteTo(StreamWriter writer, FunctionSignature signature)
        {
            writer.Write("(func ");
            foreach(BuiltinLanguageType parameterType in signature.EnumerateParameters()) {
                writer.Write("(param ");
                WriteTo(writer, parameterType);
                writer.Write(") ");
            }
            if (BuiltinLanguageType.EmptyBlock != signature.ReturnType) {
                writer.Write("(result ");
                WriteTo(writer, signature.ReturnType);
                writer.Write(") ");
            }
            writer.WriteLine(")");
        }

        internal void WriteTo(StreamWriter writer, BuiltinLanguageType type)
        {
            switch (type) {
                case BuiltinLanguageType.I32:
                    writer.Write("i32");
                    break;
                case BuiltinLanguageType.I64:
                    writer.Write("i64");
                    break;
                case BuiltinLanguageType.F32:
                    writer.Write("f32");
                    break;
                case BuiltinLanguageType.F64:
                    writer.Write("f64");
                    break;
                default: throw new ArgumentException();
            }
        }

        private WasmModule _module;
    }
}
