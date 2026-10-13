using System;
using System.Runtime.CompilerServices;

namespace Waylith.CLI;

public class EnvironmentCommandLine : ICommandLine
{
	[CompilerGenerated]
	private string[] _003CArgs_003Ek__BackingField;

	public string[] Args => _003CArgs_003Ek__BackingField ?? (_003CArgs_003Ek__BackingField = Environment.GetCommandLineArgs());
}
