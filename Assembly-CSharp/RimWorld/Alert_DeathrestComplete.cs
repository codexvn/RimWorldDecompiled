using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003055 RID: 12373
	public class Alert_DeathrestComplete : Alert
	{
		// Token: 0x06011614 RID: 71188 RVA: 0x004FC5B4 File Offset: 0x004FA7B4
		public Alert_DeathrestComplete()
		{
			this.defaultLabel = "AlertDeathrestComplete".Translate();
			this.defaultExplanation = "AlertDeathrestCompleteDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x06011615 RID: 71189 RVA: 0x004FC603 File Offset: 0x004FA803
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x06011616 RID: 71190 RVA: 0x004FC618 File Offset: 0x004FA818
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + ":\n" + (from p in this.targets
			select p.NameShortColored.Resolve()).ToLineList("  - ", false);
		}

		// Token: 0x06011617 RID: 71191 RVA: 0x004FC670 File Offset: 0x004FA870
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].genes != null && list[j].Deathresting)
					{
						Gene_Deathrest firstGeneOfType = list[j].genes.GetFirstGeneOfType<Gene_Deathrest>();
						if (firstGeneOfType != null && firstGeneOfType.ShowWakeAlert)
						{
							this.targets.Add(list[j]);
						}
					}
				}
			}
		}

		// Token: 0x0400B3EB RID: 46059
		private List<Pawn> targets = new List<Pawn>();
	}
}
