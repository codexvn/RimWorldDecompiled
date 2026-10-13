using System.Text.RegularExpressions;
using UnityStubs.Internal;

namespace UnityEngine.TestTools;

public static class LogAssert
{
	public static bool ignoreFailingMessages { get; set; }

	public static void Expect(LogType type, string message)
	{
		StubWarner.LogStub("Expect", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\UnityTestTools.cs", 8);
	}

	public static void Expect(LogType type, Regex message)
	{
		StubWarner.LogStub("Expect", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\UnityTestTools.cs", 9);
	}

	public static void NoUnexpectedReceived()
	{
		StubWarner.LogStub("NoUnexpectedReceived", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\UnityTestTools.cs", 10);
	}
}
