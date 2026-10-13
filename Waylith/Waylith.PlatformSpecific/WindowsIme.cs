using System;
using System.Runtime.InteropServices;

namespace Waylith.PlatformSpecific;

public static class WindowsIme
{
	[DllImport("user32.dll")]
	private static extern nint GetActiveWindow();

	[DllImport("imm32.dll")]
	private static extern nint ImmGetContext(nint hWnd);

	[DllImport("imm32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool ImmGetOpenStatus(nint hIMC);

	[DllImport("imm32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool ImmSetOpenStatus(nint hIMC, [MarshalAs(UnmanagedType.Bool)] bool fOpen);

	[DllImport("imm32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool ImmReleaseContext(nint hWnd, nint hIMC);

	public static void ResetToOff()
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return;
		}
		try
		{
			nint activeWindow = GetActiveWindow();
			if (activeWindow == IntPtr.Zero)
			{
				return;
			}
			nint num = ImmGetContext(activeWindow);
			if (num == IntPtr.Zero)
			{
				return;
			}
			try
			{
				if (ImmGetOpenStatus(num))
				{
					ImmSetOpenStatus(num, fOpen: false);
				}
			}
			finally
			{
				ImmReleaseContext(activeWindow, num);
			}
		}
		catch
		{
		}
	}
}
