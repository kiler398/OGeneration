using System;
using System.Collections.Generic;
using System.Text;


namespace MyGeneration
{
    public interface IMyGenErrorList : IMyGenContent
    {
        void AddErrors(params Exception[] exceptions);
        void AddErrors(params IMyGenError[] errors);
        List<IMyGenError> Errors { get; }      
    }
}
