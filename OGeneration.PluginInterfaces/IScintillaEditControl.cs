using System;
using System.Collections.Generic;
using System.Text;

namespace MyGeneration
{
    public interface IScintillaEditControl : IMyGenDocument
    {
        IScintillaNet ScintillaEditor { get; }
    }
}
