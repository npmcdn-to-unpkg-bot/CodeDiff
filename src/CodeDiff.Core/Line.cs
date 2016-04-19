using System;
using System.Diagnostics.Contracts;

namespace MsdrRu.CodeDiff
{
    public class Line
    {
        public Line(ulong lineNumber)
        {
            Contract.Requires<ArgumentException>(lineNumber > 0);
            this.LineNumber = lineNumber;
        }

        public ulong LineNumber { get; }

        public string Content { get; set; }

        public LineStatus Status { get; set; }
    }
}
