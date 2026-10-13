using System;
using System.Linq;
using Brutal.Numerics;

namespace Waylith.CLI;

public static class CommandLineExtensions
{
	public static bool HasSwitch(this ICommandLine commandLine, string name)
	{
		return commandLine.Args.Any((string arg) => arg.Trim('-', '+').Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	public static string? GetValue(this ICommandLine commandLine, string name)
	{
		string prefix = name + "=";
		string text = commandLine.Args.FirstOrDefault((string a) => a.TrimStart('-', '+').StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
		return text?.Substring(text.IndexOf('=') + 1);
	}

	public static string? GetFollowingValue(this ICommandLine commandLine, string name)
	{
		string[] args = commandLine.Args;
		int num = Array.FindIndex(args, (string a) => a.Trim('-', '+').Equals(name, StringComparison.OrdinalIgnoreCase));
		if (num < 0 || num + 1 >= args.Length)
		{
			return null;
		}
		return args[num + 1];
	}

	public static int2? GetInt2(this ICommandLine commandLine, string name)
	{
		string[] array = commandLine.GetValue(name)?.Split(',');
		if (array != null && array.Length == 2)
		{
			string s = array[0];
			string s2 = array[1];
			if (int.TryParse(s, out var result) && int.TryParse(s2, out var result2))
			{
				return new int2(result, result2);
			}
		}
		return null;
	}
}
