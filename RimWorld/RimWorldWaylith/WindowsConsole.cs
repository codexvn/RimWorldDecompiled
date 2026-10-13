using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace RimWorldWaylith;

internal static class WindowsConsole
{
	private const int StdInput = -10;

	private const int StdOutput = -11;

	private const int StdError = -12;

	private const uint GenericReadWrite = 3221225472u;

	private const uint ShareReadWrite = 3u;

	private const uint OpenExisting = 3u;

	private const uint EnableVirtualTerminalProcessing = 4u;

	private const uint AttachParentProcess = uint.MaxValue;

	public static void OpenIfRequested(string[] args)
	{
		if (!OperatingSystem.IsWindows() || !args.Any(delegate(string a)
		{
			string text = a.Trim('-', '+');
			return text.Equals("console", StringComparison.OrdinalIgnoreCase) || text.Equals("headless", StringComparison.OrdinalIgnoreCase);
		}) || GetConsoleWindow() != 0 || IsValid(GetStdHandle(-11)))
		{
			return;
		}
		bool flag = !AttachConsole(uint.MaxValue);
		if (!flag || AttachViaChild() || AllocConsole())
		{
			nint handle = CreateFileW("CONOUT$", 3221225472u, 3u, 0, 3u, 0u, 0);
			nint handle2 = CreateFileW("CONIN$", 3221225472u, 3u, 0, 3u, 0u, 0);
			SetStdHandle(-11, handle);
			SetStdHandle(-12, handle);
			SetStdHandle(-10, handle2);
			if (GetConsoleMode(handle, out var mode))
			{
				SetConsoleMode(handle, mode | 4);
			}
			if (flag)
			{
				SetConsoleTitleW("RimWorld");
			}
		}
	}

	private static bool AttachViaChild()
	{
		try
		{
			using Process process = Process.Start(new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "cmd.exe"))
			{
				UseShellExecute = false,
				ArgumentList = { "/d", "/c", "pause>nul" }
			});
			if (process == null)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < 100; i++)
			{
				if (flag)
				{
					break;
				}
				if (process.HasExited)
				{
					break;
				}
				Thread.Sleep(20);
				flag = AttachConsole((uint)process.Id);
			}
			process.Kill();
			return flag;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private static bool IsValid(nint handle)
	{
		if (handle != 0)
		{
			return handle != -1;
		}
		return false;
	}

	[DllImport("kernel32")]
	private static extern bool AllocConsole();

	[DllImport("kernel32")]
	private static extern bool AttachConsole(uint processId);

	[DllImport("kernel32")]
	private static extern nint GetConsoleWindow();

	[DllImport("kernel32")]
	private static extern nint GetStdHandle(int which);

	[DllImport("kernel32")]
	private static extern bool SetStdHandle(int which, nint handle);

	[DllImport("kernel32")]
	private static extern bool GetConsoleMode(nint handle, out uint mode);

	[DllImport("kernel32")]
	private static extern bool SetConsoleMode(nint handle, uint mode);

	[DllImport("kernel32", CharSet = CharSet.Unicode)]
	private static extern bool SetConsoleTitleW(string title);

	[DllImport("kernel32", CharSet = CharSet.Unicode)]
	private static extern nint CreateFileW(string name, uint access, uint share, nint security, uint disposition, uint flags, nint template);
}
