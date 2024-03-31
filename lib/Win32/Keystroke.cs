using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;

namespace Win32
{
	internal static class Win32Api
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern uint MapVirtualKey(uint uCode, uint uMapType);
	}

	public static class KeyStroke
	{
		private static long MakeLParam(uint scanCode, bool isDown)
		{
			long keyRepeatCount = 0x001;

			long extendedKeyFlag = 0x000;
			long reserved = 0x000; // 3 bits reserved
			long contextCode = 0x000; // if ALT is down 1 else 0
			long previousKeyStateFlag = isDown ? 0x001 : 0x000; // was the key previously up (1) or down (0)
			long transitionStateFlag = isDown ? 0x000 : 0x001; // key down is (0) key up is (1)

			return keyRepeatCount | (scanCode << 16) | (extendedKeyFlag << 24) | (reserved << 25)
				| (contextCode << 29) | (previousKeyStateFlag << 30) | (transitionStateFlag << 31);
		}

		private static uint ScanCodeToVirtualKeyCode(uint vk)
		{
			var MAPVK_VSC_TO_VK = 1u;
			var sc = Win32Api.MapVirtualKey(vk, MAPVK_VSC_TO_VK);
			return sc;
		}

		public static void SendKeystroke(Window.IWindowEntry window, uint godotKeyCode, bool isDown)
		{
			const uint WM_KEYDOWN = 0x100;
			const uint WM_KEYUP = 0x101;
			const uint WM_CHAR = 0x102;

			var scanCode = KeyCodes.GodotKeyCodeToWin32ScanCode(godotKeyCode);
			var vkcode = ScanCodeToVirtualKeyCode(scanCode);
			_ = Win32Api.SendMessage(window.HWnd, isDown ? WM_KEYDOWN : WM_KEYUP,
				(IntPtr)vkcode, (IntPtr)MakeLParam(scanCode, isDown));
			_ = Win32Api.SendMessage(window.HWnd, WM_CHAR,
				(IntPtr)vkcode, (IntPtr)MakeLParam(scanCode, isDown));
		}

	}

}
