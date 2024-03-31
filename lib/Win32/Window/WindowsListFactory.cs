using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32.Window
{
    /// <remarks>
    /// Thanks to Tommy Carlier for how to get the list of windows: http://blog.tcx.be/2006/05/getting-list-of-all-open-windows.html
    /// </remarks>
    internal static class WindowsListFactory
    {
        private const int MaxLastActivePopupIterations = 50;

        delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);

        public enum GetAncestorFlags
        {
            GetParent = 1,
            GetRoot = 2,
            GetRootOwner = 3
        }

        [DllImport("user32.dll")]
        static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetShellWindow();

        [DllImport("user32.dll", ExactSpelling = true)]
        static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlags flags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        public static WindowsList Load()
        {
            var lShellWindow = GetShellWindow();
            var windows = new List<IWindowEntry>();
            var currentProcessId = Process.GetCurrentProcess().Id;

            EnumWindows((hWnd, lParam) =>
            {
                if (!EligibleForActivation(hWnd, lShellWindow))
                    return true;
                var window = WindowEntryFactory.Create(hWnd);
                if (window == null || window.ProcessId == currentProcessId || window.Title == null)
                    return true;
                if (IsKnownException(window))
                    return true;
                windows.Add(window);
                return true;
            }, 0);

            return new WindowsList(windows);
        }

        private static bool IsKnownException(IWindowEntry window)
        {
            if (window.Title == "SSFiddlerMsgWin")
                return true;
            if (window.Title.Contains(Godot.ProjectSettings.GetSetting("application/config/name").AsString()))
                return true;
            if (window.Title == "Settings" || window.Title == "Microsoft Text Input Application") // Todo get the class and filter this one properly
                return true;
            return false;
        }

        private static readonly string[] WindowsClassNamesToSkip =
        {
            "Shell_TrayWnd",
            "DV2ControlHost",
            "MsgrIMEWindowClass",
            "SysShadow",
            "Button"
        };

        private static bool EligibleForActivation(IntPtr hWnd, IntPtr lShellWindow)
        {
            // http://stackoverflow.com/questions/210504/enumerate-windows-like-alt-tab-does

            if (hWnd == lShellWindow)
                return false;
            var root = GetAncestor(hWnd, GetAncestorFlags.GetRootOwner);
            if (GetLastVisibleActivePopUpOfWindow(root) != hWnd)
                return false;
            var classNameStringBuilder = new StringBuilder(256);
            var length = GetClassName(hWnd, classNameStringBuilder, classNameStringBuilder.Capacity);
            if (length == 0)
                return false;
            var className = classNameStringBuilder.ToString();
            if (Array.IndexOf(WindowsClassNamesToSkip, className) > -1)
                return false;
            if (className.StartsWith("WMP9MediaBarFlyout")) //WMP's "now playing" taskbar-toolbar
                return false;
            return true;
        }

        private static IntPtr GetLastVisibleActivePopUpOfWindow(IntPtr window)
        {
            var level = MaxLastActivePopupIterations;
            var currentWindow = window;
            while (level-- > 0)
            {
                var lastPopUp = GetLastActivePopup(currentWindow);
                if (IsWindowVisible(lastPopUp))
                    return lastPopUp;
                if (lastPopUp == currentWindow)
                    return IntPtr.Zero;
                currentWindow = lastPopUp;
            }
            return IntPtr.Zero;
        }
    }
}
