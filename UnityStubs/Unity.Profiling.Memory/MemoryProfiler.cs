using System;
using UnityStubs.Internal;

namespace Unity.Profiling.Memory;

public static class MemoryProfiler
{
	public static void TakeSnapshot(string path, Action<string, bool> finishCallback, CaptureFlags captureFlags = CaptureFlags.ManagedObjects | CaptureFlags.NativeObjects)
	{
		StubWarner.LogStub("TakeSnapshot", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\MemoryProfiler.cs", 5);
	}
}
