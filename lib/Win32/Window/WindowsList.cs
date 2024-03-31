using System.Collections.Generic;

namespace Win32.Window
{
    public class WindowsList
    {
        public WindowsList(IList<IWindowEntry> windows)
        {
            Windows = windows;
        }

        public IList<IWindowEntry> Windows { get; private set; }
    }
}
