using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Brutal;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Utils;

namespace Waylith.Diagnostics;

internal static class CrashDialog
{
	private static int _shown;

	private const uint MB_OK = 0u;

	private const uint MB_YESNO = 4u;

	private const uint MB_ICONERROR = 16u;

	private const uint MB_SETFOREGROUND = 65536u;

	private const uint MB_TOPMOST = 262144u;

	private const int IDYES = 6;

	public static bool Enabled { get; set; } = true;

	public static string ReportInstructions { get; set; } = "Please include the newest report from that folder when reporting the crash.";

	public static void TryShow(Exception? exception, string product, string? reportPath)
	{
		if (!Enabled || Interlocked.Exchange(ref _shown, 1) != 0 || (Platform.IsLinux && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DISPLAY")) && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY"))))
		{
			return;
		}
		try
		{
			string text = ((reportPath != null) ? Path.GetDirectoryName(reportPath) : null);
			string title = product + " has crashed!";
			string message = BuildMessage(exception, product, reportPath);
			if (Show(title, message, text != null) && text != null)
			{
				Shell.Open(text);
			}
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("Crash dialog failed: " + ex.Message);
		}
	}

	private static string BuildMessage(Exception? exception, string product, string? reportPath)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(product).AppendLine(" has encountered an unrecoverable error and must shut down.").AppendLine();
		stringBuilder.AppendLine(Describe(exception)).AppendLine();
		if (reportPath != null)
		{
			stringBuilder.AppendLine("A crash report was written to:").AppendLine(reportPath).AppendLine();
		}
		else
		{
			stringBuilder.AppendLine("No crash report could be written, please include the game log instead.").AppendLine();
		}
		stringBuilder.Append(ReportInstructions);
		return stringBuilder.ToString();
	}

	public static string Describe(Exception? exception)
	{
		if (exception == null)
		{
			return "The game hit an unknown fatal error.";
		}
		List<Exception> list = new List<Exception>();
		Exception? ex3;
		for (Exception ex = exception; ex != null; ex = ex3)
		{
			list.Add(ex);
			if (ex is AggregateException ex2)
			{
				ReadOnlyCollection<Exception> innerExceptions = ex2.InnerExceptions;
				if (innerExceptions != null && innerExceptions.Count > 0)
				{
					ex3 = ex2.InnerExceptions[0];
					continue;
				}
			}
			ex3 = ex.InnerException;
		}
		list.Reverse();
		foreach (Exception item in list)
		{
			if (item is VkResultError vkResultError)
			{
				switch (vkResultError.Result)
				{
				case VkResult.ErrorDeviceLost:
					return "The graphics device was lost (GPU crash or driver reset). This often indicates a bug in the game, engine, or any running mods, but could indicate a graphics driver problem, overheating or an unstable overclock. Updating the graphics driver is the first thing to try.";
				case VkResult.ErrorOutOfDeviceMemory:
					break;
				case VkResult.ErrorOutOfHostMemory:
					goto IL_0149;
				case VkResult.ErrorIncompatibleDriver:
				case VkResult.ErrorInitializationFailed:
					return "The graphics driver could not start Vulkan. The game requires a Vulkan capable GPU with an up-to-date driver.";
				default:
					return $"An unexpected graphics error occurred ({vkResultError.Result}). Make sure your graphics driver is up to date.";
				}
				goto IL_013d;
			}
			if (item is VmaAllocationException { Result: var result } ex4)
			{
				if (result == VkResult.ErrorOutOfDeviceMemory)
				{
					goto IL_013d;
				}
				if (result != VkResult.ErrorOutOfHostMemory)
				{
					return $"A graphics memory allocation failed for an unknown reason ({ex4.Result}).";
				}
			}
			else if (!(item is OutOfMemoryException))
			{
				if (!(item is DllNotFoundException) && !(item is BadImageFormatException) && !(item is EntryPointNotFoundException))
				{
					if (!(item is IOException) && !(item is UnauthorizedAccessException))
					{
						continue;
					}
					return "A file operation failed: " + FirstLine(item.Message);
				}
				return "A required native library is missing or damaged. Verifying the game files should fix it. Otherwise, confirm that any antivirus software is not restricting the game.";
			}
			goto IL_0149;
			IL_013d:
			return "The game ran out of video memory (VRAM). If you're using texture mods, try removing (some of) them. Otherwise, trying a lower resolution, or closing other GPU heavy programs will help.";
			IL_0149:
			return "The game ran out of system memory (RAM). If using mods, this may indicate your mod list is too large for your hardware, or one of the mods has a problem. Closing other programs or trimming the mod list will help.";
		}
		Exception ex5 = list[0];
		string text = ModAssembly(ex5);
		string text2 = ex5.GetType().Name + ": " + FirstLine(ex5.Message);
		if (text == null)
		{
			return "An unexpected error occurred. " + text2;
		}
		return "An error occurred in code from '" + text + "', which looks like a mod. " + text2;
	}

	private static string? ModAssembly(Exception e)
	{
		try
		{
			StackTrace stackTrace = new StackTrace(e, fNeedFileInfo: false);
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				string text = (stackTrace.GetFrame(i)?.GetMethod())?.DeclaringType?.Assembly.GetName().Name;
				if (text == null)
				{
					continue;
				}
				bool flag = text.StartsWith("System", StringComparison.Ordinal) || text.StartsWith("Microsoft", StringComparison.Ordinal) || text.StartsWith("Waylith", StringComparison.Ordinal) || text.StartsWith("Brutal", StringComparison.Ordinal);
				if (!flag)
				{
					bool flag2;
					switch (text)
					{
					case "UnityStubs":
					case "Assembly-CSharp":
					case "RimWorld":
					case "Main":
						flag2 = true;
						break;
					default:
						flag2 = false;
						break;
					}
					flag = flag2;
				}
				if (!flag && !text.StartsWith("0Harmony", StringComparison.Ordinal) && !text.StartsWith("MonoMod", StringComparison.Ordinal))
				{
					return text;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static string FirstLine(string? message)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			return "(no message)";
		}
		ReadOnlySpan<char> span = message.AsSpan().Trim();
		int num = span.IndexOfAny('\r', '\n');
		if (num >= 0)
		{
			span = span.Slice(0, num);
		}
		if (span.Length <= 300)
		{
			return span.ToString();
		}
		return string.Concat(span.Slice(0, 300), "...".AsSpan());
	}

	private static bool Show(string title, string message, bool offerOpenFolder)
	{
		if (Platform.IsWindows)
		{
			return ShowWindows(title, message, offerOpenFolder);
		}
		if (Platform.IsMacOS)
		{
			return ShowMac(title, message, offerOpenFolder);
		}
		return ShowLinux(title, message, offerOpenFolder);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode)]
	private static extern int MessageBoxW(nint hWnd, string text, string caption, uint type);

	private static bool ShowWindows(string title, string message, bool offerOpenFolder)
	{
		string text = (offerOpenFolder ? (message + "\n\nOpen the crash report folder now?") : message);
		int num = MessageBoxW(0, text, title, (uint)((offerOpenFolder ? 4 : 0) | 0x10 | 0x10000 | 0x40000));
		if (offerOpenFolder)
		{
			return num == 6;
		}
		return false;
	}

	private static bool ShowMac(string title, string message, bool offerOpenFolder)
	{
		string value = (offerOpenFolder ? "{\"Open Folder\", \"Close\"}" : "{\"Close\"}");
		string text = $"display dialog {AppleScriptString(message)} with title {AppleScriptString(title)} buttons {value} default button 1 with icon stop";
		string text2 = RunAndRead("osascript", new string[2] { "-e", text });
		if (offerOpenFolder && text2 != null)
		{
			return text2.Contains("Open Folder", StringComparison.Ordinal);
		}
		return false;
	}

	private static string AppleScriptString(string s)
	{
		return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
	}

	private static bool ShowLinux(string title, string message, bool offerOpenFolder)
	{
		if (offerOpenFolder)
		{
			int? num = Run("zenity", new string[11]
			{
				"--question", "--icon=dialog-error", "--title", title, "--text", message, "--ok-label", "Open Folder", "--cancel-label", "Close",
				"--no-wrap"
			}) ?? Run("kdialog", new string[8] { "--title", title, "--warningyesno", message, "--yes-label", "Open Folder", "--no-label", "Close" });
			if (num.HasValue)
			{
				return num == 0;
			}
		}
		if (!(Run("zenity", new string[6] { "--error", "--title", title, "--text", message, "--no-wrap" }) ?? Run("kdialog", new string[4] { "--title", title, "--error", message }) ?? Run("xmessage", new string[4] { "-center", "-title", title, message })).HasValue)
		{
			Console.Error.WriteLine(message);
		}
		return false;
	}

	private static int? Run(string file, string[] args)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(file)
			{
				UseShellExecute = false
			};
			foreach (string item in args)
			{
				processStartInfo.ArgumentList.Add(item);
			}
			using Process process = Process.Start(processStartInfo);
			if (process == null)
			{
				return null;
			}
			process.WaitForExit();
			return process.ExitCode;
		}
		catch
		{
			return null;
		}
	}

	private static string? RunAndRead(string file, string[] args)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo(file)
			{
				UseShellExecute = false,
				RedirectStandardOutput = true
			};
			foreach (string item in args)
			{
				processStartInfo.ArgumentList.Add(item);
			}
			using Process process = Process.Start(processStartInfo);
			if (process == null)
			{
				return null;
			}
			string result = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			return result;
		}
		catch
		{
			return null;
		}
	}
}
