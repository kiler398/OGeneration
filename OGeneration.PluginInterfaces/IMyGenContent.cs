using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MyGeneration
{
    public interface IMyGenContent
    {
        ToolBar ToolStrip { get; }
        void ProcessAlert(IMyGenContent sender, string command, params object[] args);
        bool CanClose(bool allowPrevent);
        DockContent DockContent { get; }
    }
}
