using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Waylith.Profiling;
using Waylith.Profiling.Instrumentation;

namespace Verse.ProfilerUI;

public static class ProfilerPrefs
{
	private static readonly string[] DefaultTargets = new string[2] { "Assembly-CSharp!Verse.*::*", "Assembly-CSharp!RimWorld.*::*" };

	private static ProfilerPrefsData data;

	public static ProfilerPrefsData Current => data ?? (data = Load());

	private static string FilePath => Path.Combine(GenFilePaths.ConfigFolderPath, "Profiler.xml");

	private static ProfilerPrefsData Load()
	{
		ProfilerInstrumentation.ExcludedAssemblies = new string[3] { "0Harmony", "MonoMod.*", "Mono.Cecil*" };
		ProfilerPrefsData profilerPrefsData = null;
		try
		{
			if (File.Exists(FilePath))
			{
				profilerPrefsData = DirectXmlLoader.ItemFromXmlFile<ProfilerPrefsData>(FilePath, resolveCrossRefs: false);
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Could not load profiler prefs, using defaults: " + ex.Message);
		}
		if (profilerPrefsData == null)
		{
			profilerPrefsData = new ProfilerPrefsData();
		}
		ProfilerPrefsData profilerPrefsData2 = profilerPrefsData;
		if (profilerPrefsData2.instrumentationTargets == null)
		{
			profilerPrefsData2.instrumentationTargets = new List<string>(DefaultTargets);
		}
		return profilerPrefsData;
	}

	public static void Reset()
	{
		Rect windowRect = Current.windowRect;
		data = new ProfilerPrefsData
		{
			windowRect = windowRect,
			instrumentationTargets = new List<string>(DefaultTargets)
		};
		Apply();
		Save();
	}

	public static void Save()
	{
		try
		{
			DirectXmlSaver.SaveDataObject(Current, FilePath);
		}
		catch (Exception ex)
		{
			Log.Warning("Could not save profiler prefs: " + ex.Message);
		}
	}

	public static void Apply()
	{
		ProfilerPrefsData current = Current;
		FrameProfiler.SetHistoryCapacity(current.historyCapacity);
		current.historyCapacity = FrameProfiler.HistoryCapacity;
		if (FrameProfiler.AllocationTrackingSupported)
		{
			FrameProfiler.SetAllocationTracking(current.allocTracking);
		}
		current.allocTracking = FrameProfiler.AllocationTracking;
		FrameProfiler.SetMainThreadOnly(current.mainThreadOnly);
		current.mainThreadOnly = FrameProfiler.MainThreadOnly;
		FrameProfiler.IgnoreThrottle = current.ignoreThrottle;
		FrameProfiler.AutoMuteTrivialLabels = current.autoMuteTrivial;
		ProfilerInstrumentation.SetTargets(current.instrumentationTargets);
		current.instrumentationTargets = new List<string>(ProfilerInstrumentation.Targets);
		if (ProfilerInstrumentation.IsSupported)
		{
			ProfilerInstrumentation.SetEnabled(current.instrumentation);
		}
		current.instrumentation = ProfilerInstrumentation.IsEnabled;
		current.budgetMs = ((current.budgetMs <= 0f) ? 0f : Mathf.Clamp(current.budgetMs, 1f, 1000f));
	}
}
