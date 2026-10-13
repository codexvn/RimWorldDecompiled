using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.Utils;

public static class Shell
{
	private static readonly LogCategory Log = LogCategory.Make("Shell");

	public static void Open(string target)
	{
		if (string.IsNullOrWhiteSpace(target))
		{
			return;
		}
		try
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Process.Start(new ProcessStartInfo(target)
				{
					UseShellExecute = true
				});
			}
			else
			{
				Process.Start(new ProcessStartInfo(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "open" : "xdg-open")
				{
					ArgumentList = { target }
				});
			}
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(30, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Shell.Open failed to open '");
				message.AppendFormatted(target);
				message.AppendLiteral("': ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "Open", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Utils\\Shell.cs", 35);
		}
	}
}
