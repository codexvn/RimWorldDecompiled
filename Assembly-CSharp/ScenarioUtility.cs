using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

// Token: 0x0200003F RID: 63
public static class ScenarioUtility
{
	// Token: 0x06000150 RID: 336 RVA: 0x00009320 File Offset: 0x00007520
	public static bool AllowsChildSelection(Scenario scenario)
	{
		List<ScenPart> parts = scenario.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs scenPart_ConfigPage_ConfigureStartingPawns_KindDefs = parts[i] as ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs;
			if (scenPart_ConfigPage_ConfigureStartingPawns_KindDefs != null)
			{
				using (List<PawnKindCount>.Enumerator enumerator = scenPart_ConfigPage_ConfigureStartingPawns_KindDefs.kindCounts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!ScenarioUtility.CanBeChild(enumerator.Current.kindDef))
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000093A8 File Offset: 0x000075A8
	private static bool CanBeChild(PawnKindDef kindDef)
	{
		if (!kindDef.apparelRequired.NullOrEmpty<ThingDef>())
		{
			for (int i = 0; i < kindDef.apparelRequired.Count; i++)
			{
				if (kindDef.apparelRequired[i].IsApparel && !kindDef.apparelRequired[i].apparel.developmentalStageFilter.Juvenile())
				{
					return false;
				}
			}
		}
		return true;
	}
}
