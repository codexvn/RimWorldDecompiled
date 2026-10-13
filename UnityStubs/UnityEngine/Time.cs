using System;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Time
{
	private static float _smoothDeltaTime;

	private static int _smoothDeltaFrame = -1;

	public static float deltaTime => Math.Min(UnityRuntime.Current?.Time.DeltaTime ?? 0f, maximumDeltaTime);

	public static float smoothDeltaTime
	{
		get
		{
			int num = frameCount;
			if (num != _smoothDeltaFrame)
			{
				_smoothDeltaFrame = num;
				_smoothDeltaTime = ((_smoothDeltaTime <= 0f) ? deltaTime : (_smoothDeltaTime + (deltaTime - _smoothDeltaTime) * 0.2f));
			}
			return _smoothDeltaTime;
		}
	}

	public static int frameCount => UnityRuntime.Current?.Time.FrameCount ?? 0;

	public static float realtimeSinceStartup => UnityRuntime.Current?.Time.RealtimeSinceStartup ?? 0f;

	public static double realtimeSinceStartupAsDouble => UnityRuntime.Current?.Time.RealtimeSinceStartupAsDouble ?? 0.0;

	public static float time => UnityRuntime.Current?.Time.Time ?? 0f;

	public static float timeSinceLevelLoad => time;

	public static float timeScale
	{
		get
		{
			return 1f;
		}
		set
		{
			StubWarner.LogStub("timeScale", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Time.cs", 36);
		}
	}

	public static float unscaledDeltaTime => UnityRuntime.Current?.Time.UnscaledDeltaTime ?? 0f;

	public static float unscaledTime => UnityRuntime.Current?.Time.UnscaledTime ?? 0f;

	public static float fixedUnscaledTime => unscaledTime;

	public static float maximumDeltaTime { get; set; } = 1f / 3f;
}
