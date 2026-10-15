using System.Collections.Generic;
using UnityEngine;

namespace Verse.ProfilerUI;

public sealed class ProfilerPrefsData
{
	public bool startOnBoot;

	public bool keepRunningWhenClosed;

	public int historyCapacity = 2000;

	public bool allocTracking;

	public bool mainThreadOnly = true;

	public bool ignoreThrottle = true;

	public List<string> instrumentationTargets;

	public bool instrumentation;

	public bool autoMuteTrivial = true;

	public float budgetMs;

	public bool foldRecursion = true;

	public Rect windowRect;
}
