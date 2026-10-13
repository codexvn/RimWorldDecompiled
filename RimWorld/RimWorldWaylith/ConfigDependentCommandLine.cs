using System;
using System.Runtime.CompilerServices;
using Verse.Steam;
using Waylith.CLI;

namespace RimWorldWaylith;

public class ConfigDependentCommandLine : ICommandLine
{
	[CompilerGenerated]
	private string[] _003CArgs_003Ek__BackingField;

	public string[] Args => _003CArgs_003Ek__BackingField ?? (_003CArgs_003Ek__BackingField = ReadArgs());

	public ConfigDependentCommandLine()
	{
		SteamManager.InitIfNeeded();
	}

	private static string[] ReadArgs()
	{
		return Environment.GetCommandLineArgs();
	}
}
