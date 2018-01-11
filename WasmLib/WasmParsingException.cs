using System;

namespace WasmLib
{
    public class WasmParsingException : ApplicationException
    {
        internal WasmParsingException()
            : base()
        {
        }

        internal WasmParsingException(string message)
            : base(message)
        {
        }

        internal WasmParsingException(string message, int offset)
            : this(message)
        {
            Offset = offset;
        }

        internal WasmParsingException(string message, Exception e, int offset)
            : base(message, e)
        {
            Offset = offset;
        }

        public int Offset { get; private set; }
    }
}
