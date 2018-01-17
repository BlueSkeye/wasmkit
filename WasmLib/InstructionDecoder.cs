using System.Collections.Generic;

namespace WasmLib
{
    internal class InstructionDecoder
    {
        internal InstructionDecoder(BinaryParsingReader reader)
        {
            _reader = reader;
        }

        //internal List<Instruction> DecodeInitializationExpression()
        //{
        //}

        private BinaryParsingReader _reader;
    }
}
