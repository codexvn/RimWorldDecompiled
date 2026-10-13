using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD5 RID: 12245
	public class Alert_LowOxygen : Alert_Critical
	{
		// Token: 0x06011418 RID: 70680 RVA: 0x004F1D03 File Offset: 0x004EFF03
		public Alert_LowOxygen()
		{
			this.defaultLabel = "AlertVacuumExposure".Translate();
		}

		// Token: 0x17002CC3 RID: 11459
		// (get) Token: 0x06011419 RID: 70681 RVA: 0x004F1D2C File Offset: 0x004EFF2C
		private List<Pawn> Pawns
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return this.pawns;
				}
				this.pawns.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
				{
					if (pawn.Position.GetVacuum(pawn.Map) >= 0.5f)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VacuumExposure, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 2)
						{
							this.pawns.Add(pawn);
						}
					}
				}
				return this.pawns;
			}
		}

		// Token: 0x0601141A RID: 70682 RVA: 0x004F1DE4 File Offset: 0x004EFFE4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.pawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertVacuumExposureDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x0601141B RID: 70683 RVA: 0x004F1E70 File Offset: 0x004F0070
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Pawns);
		}

		// Token: 0x0400B305 RID: 45829
		private readonly List<Pawn> pawns = new List<Pawn>();
	}
}
