
namespace WasmLib
{
    internal enum BuiltinLanguageType : sbyte
    {
        I32 = -1,
        I64 = -2,
        F32 = -3,
        F64 = -4,
        AnyFunc = -16,
        Func = -32,
        EmptyBlock = -64
    }
}
