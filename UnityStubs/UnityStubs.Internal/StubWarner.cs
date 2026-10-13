using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace UnityStubs.Internal;

public static class StubWarner
{
	internal const string CategoryName = "UnityStubs";

	private static readonly LogCategory category = LogCategory.Make("UnityStubs");

	private static readonly ConcurrentDictionary<string, int> warnedStubs = new ConcurrentDictionary<string, int>();

	public static bool Suppressed { get; set; }

	public static void LogStub([CallerMemberName] string stubName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
	{
		if (Suppressed)
		{
			return;
		}
		string key = filePath + ":" + lineNumber;
		if (!warnedStubs.ContainsKey(key))
		{
			warnedStubs.TryAdd(key, 1);
			string value = Path.GetFileName(filePath) + ":" + lineNumber;
			LogCategory logCategory = category;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(44, 2, logCategory, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(stubName);
				message.AppendLiteral(" (");
				message.AppendFormatted(value);
				message.AppendLiteral("): Stub! You may not get expected results!");
			}
			logCategory.Warning(message, "LogStub", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Internal\\StubWarner.cs", 29);
		}
	}
}
