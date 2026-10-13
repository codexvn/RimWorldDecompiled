using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DBE RID: 15806
	public static class SettlementNameGenerator
	{
		// Token: 0x060158BF RID: 88255 RVA: 0x00674074 File Offset: 0x00672274
		public static string GenerateSettlementName(WorldObject worldObject, RulePackDef rulePack = null)
		{
			if (rulePack == null)
			{
				Faction faction = worldObject.Faction;
				if (((faction != null) ? faction.def.settlementNameMaker : null) == null)
				{
					return worldObject.def.label;
				}
				rulePack = worldObject.Faction.def.settlementNameMaker;
			}
			SettlementNameGenerator.usedNames.Clear();
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Name != null)
				{
					SettlementNameGenerator.usedNames.Add(settlement.Name);
				}
			}
			return NameGenerator.GenerateName(rulePack, SettlementNameGenerator.usedNames, true, null);
		}

		// Token: 0x0400EF95 RID: 61333
		private static readonly List<string> usedNames = new List<string>();
	}
}
