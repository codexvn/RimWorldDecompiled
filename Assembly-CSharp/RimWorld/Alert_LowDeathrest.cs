using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003047 RID: 12359
	public class Alert_LowDeathrest : Alert
	{
		// Token: 0x060115DD RID: 71133 RVA: 0x004FB7DA File Offset: 0x004F99DA
		public Alert_LowDeathrest()
		{
			this.requireBiotech = true;
		}

		// Token: 0x060115DE RID: 71134 RVA: 0x004FB800 File Offset: 0x004F9A00
		public override string GetLabel()
		{
			if (this.targets.Count == 1)
			{
				return "AlertLowDeathrestPawn".Translate(this.targetLabels[0].Named("PAWN"));
			}
			return "AlertLowDeathrestPawns".Translate(this.targetLabels.Count.ToStringCached().Named("NUMCULPRITS"));
		}

		// Token: 0x060115DF RID: 71135 RVA: 0x004FB86C File Offset: 0x004F9A6C
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				Need_Deathrest need_Deathrest;
				if (pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer && pawn.needs != null && pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest) && need_Deathrest.CurLevel <= 0.1f && !pawn.Deathresting)
				{
					this.targets.Add(pawn);
					this.targetLabels.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x060115E0 RID: 71136 RVA: 0x004FB938 File Offset: 0x004F9B38
		public override TaggedString GetExplanation()
		{
			return "AlertLowDeathrestDesc".Translate(this.targetLabels.ToLineList("  - ").Named("CULPRITS"));
		}

		// Token: 0x060115E1 RID: 71137 RVA: 0x004FB95E File Offset: 0x004F9B5E
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3D3 RID: 46035
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3D4 RID: 46036
		private List<string> targetLabels = new List<string>();
	}
}
