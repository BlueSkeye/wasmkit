namespace WasmLib
{
    internal enum ExternalKind : byte
    {
        Function = 0,  // Function import or definition
        Table = 1, // Table import or definition
        Memory = 2, // Memory import or definition
        Global = 3, // Global import or definition
    }
}
