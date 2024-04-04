using System;

namespace Win32.Window
{
	public class WindowEntry : IWindowEntry
	{
		public IntPtr HWnd { get; set; }
		public uint ProcessId { get; set; }
		public string Title { get; set; }
		public bool IsVisible { get; set; }

		public bool Focus()
		{
			return WindowToForeground.ForceWindowToForeground(HWnd);
		}

		public bool IsForeground()
		{
			return WindowToForeground.GetForegroundWindow() == HWnd;
		}

		public bool IsSameWindow(IWindowEntry other)
		{
			if (other == null)
				return false;

			return ProcessId == other.ProcessId && HWnd == other.HWnd;
		}

		public override string ToString()
		{
			return string.Format("{0}: \"{1}\"", ProcessId, Title);
		}
	}
}
