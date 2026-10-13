using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003008 RID: 12296
	public class Alert_PollutedTerrain : Alert
	{
		// Token: 0x17002CDA RID: 11482
		// (get) Token: 0x060114E5 RID: 70885 RVA: 0x004F60E0 File Offset: 0x004F42E0
		public List<Pawn> Targets
		{
			get
			{
				this.targets.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.3f && pawn.Position.IsPolluted(map))
						{
							this.targets.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x060114E6 RID: 70886 RVA: 0x004F61C8 File Offset: 0x004F43C8
		public Alert_PollutedTerrain()
		{
			this.defaultLabel = "AlertPollutedTerrain".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x060114E7 RID: 70887 RVA: 0x004F61FE File Offset: 0x004F43FE
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060114E8 RID: 70888 RVA: 0x004F620C File Offset: 0x004F440C
		public override TaggedString GetExplanation()
		{
			string value = (from p in this.targets
			select p.NameShortColored.Resolve()).ToLineList("  - ", false);
			return "AlertPollutedTerrainDesc".Translate(value);
		}

		// Token: 0x0400B35E RID: 45918
		private const float ToxicBuildupSeverityThreshold = 0.3f;

		// Token: 0x0400B35F RID: 45919
		private List<Pawn> targets = new List<Pawn>();
	}
}
