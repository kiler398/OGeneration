using System;
using System.Collections.Generic;
using System.Text;

namespace MyGeneration
{
    public interface IMyGenDocument : IMyGenContent
    {
        string DocumentIndentity { get; }
        string TextContent { get; }
    }
}
