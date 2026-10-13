using Brutal.Logging;

namespace Waylith.Logging;

public static class VerboseLog
{
	public const string CategoryName = "Verbose";

	public static readonly LogCategory Category = LogCategory.Make("Verbose");

	public static void Log(string message)
	{
		Category.Debug(message, "Log", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Logging\\VerboseLog.cs", 17);
	}
}
