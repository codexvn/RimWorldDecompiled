using System.Collections.Generic;
using UnityEngine;
using Waylith.Rendering;

namespace Verse;

public static class ShaderWarmup
{
	private static bool prewarmKicked;

	public static bool PrewarmEnabled { get; set; }

	public static void KickPrewarmOnce()
	{
		if (PrewarmEnabled && !prewarmKicked)
		{
			prewarmKicked = true;
			ShaderPrewarmer.WarmInBackground(BuildPlan());
		}
	}

	public static ShaderWarmupPlan BuildPlan()
	{
		return ShaderVariantEnumerator.Enumerate(CollectKeywordContributions(), CollectGlobalKeywordKeys());
	}

	private static List<ShaderKeywordContribution> CollectKeywordContributions()
	{
		List<ShaderKeywordContribution> list = new List<ShaderKeywordContribution>();
		foreach (MaterialDef allDef in DefDatabase<MaterialDef>.AllDefs)
		{
			if (allDef.shader != null && !allDef.keywords.NullOrEmpty())
			{
				list.Add(new ShaderKeywordContribution(allDef.shader.defName, string.Join(";", allDef.keywords)));
			}
		}
		return list;
	}

	private static List<string> CollectGlobalKeywordKeys()
	{
		return new List<string> { UnityEngine.Shader.GlobalKeywordKey };
	}
}
