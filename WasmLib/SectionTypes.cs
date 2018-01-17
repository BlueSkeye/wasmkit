namespace WasmLib
{
    public enum SectionTypes : byte
    {
        Custom = 0,
        /// <summary>Function signature declarations</summary>
        Type = 1,
        /// <summary>Import declarations</summary>
        Import = 2,
        /// <summary>Function declarations</summary>
        Function = 3,
        /// <summary>Indirect function table and other tables</summary>
        Table = 4,
        /// <summary>Memory attributes</summary>
        Memory = 5,
        /// <summary>Global declarations</summary>
        Global = 6,
        /// <summary>Exports</summary>
        Export = 7,
        /// <summary>Start function declaration</summary>
        Start = 8,
        /// <summary>Elements section</summary>
        Element = 9,
        /// <summary>Function bodies (code)</summary>
        Code = 10,
        /// <summary>Data segments</summary>
        Data = 11,
    }
}
