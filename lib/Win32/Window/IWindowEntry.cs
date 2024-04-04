using System;

namespace Win32.Window
{
    public interface IWindowEntry
    {
        IntPtr HWnd { get; set; }
        uint ProcessId { get; set; }
        string Title { get; set; }
        bool IsVisible { get; set; }

        bool Focus();
        bool IsForeground();
        bool IsSameWindow(IWindowEntry other);
    }
}
