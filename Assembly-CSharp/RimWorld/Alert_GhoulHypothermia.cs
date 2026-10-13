using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD2 RID: 12242
	public class Alert_GhoulHypothermia : Alert_Critical
	{
		// Token: 0x0601140D RID: 70669 RVA: 0x004F1A1D File Offset: 0x004EFC1D
		public Alert_GhoulHypothermia()
		{
			this.defaultLabel = "AlertGhoulHypothermia".Translate();
		}

		// Token: 0x17002CC2 RID: 11458
		// (get) Token: 0x0601140E RID: 70670 RVA: 0x004F1A48 File Offset: 0x004EFC48
		private List<Pawn> HypothermiaDangerGhouls
		{
			get
			{
				this.hypothermiaDangerGhoulsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended)
				{
					if (pawn.IsGhoul && !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerGhoulsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerGhoulsResult;
			}
		}

		// Token: 0x0601140F RID: 70671 RVA: 0x004F1AF4 File Offset: 0x004EFCF4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.hypothermiaDangerGhoulsResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertGhoulHypothermiaDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x06011410 RID: 70672 RVA: 0x004F1B80 File Offset: 0x004EFD80
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerGhouls);
		}

		// Token: 0x0400B301 RID: 45825
		private List<Pawn> hypothermiaDangerGhoulsResult = new List<Pawn>();
	}
}
