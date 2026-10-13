using System;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace Unity.Profiling;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ProfilerRecorder : IDisposable
{
	public const ProfilerRecorderOptions SharedRecorder = (ProfilerRecorderOptions)128;

	public long CurrentValue
	{
		get
		{
			StubWarner.LogStub("CurrentValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 7);
			return 0L;
		}
	}

	public long LastValue
	{
		get
		{
			StubWarner.LogStub("LastValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 8);
			return 0L;
		}
	}

	public bool Valid
	{
		get
		{
			StubWarner.LogStub("Valid", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 9);
			return false;
		}
	}

	public void Dispose()
	{
		StubWarner.LogStub("Dispose", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 5);
	}

	public void Start()
	{
		StubWarner.LogStub("Start", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 10);
	}

	public static ProfilerRecorder StartNew(ProfilerCategory category, string statName, int capacity = 1, ProfilerRecorderOptions options = ProfilerRecorderOptions.Default)
	{
		StubWarner.LogStub("StartNew", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 11);
		return default(ProfilerRecorder);
	}

	public void Stop()
	{
		StubWarner.LogStub("Stop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ProfilerRecorder.cs", 12);
	}
}
