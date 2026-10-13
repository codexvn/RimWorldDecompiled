using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC6 RID: 12230
	public class Alert_Hypothermia : Alert_Critical
	{
		// Token: 0x060113CF RID: 70607 RVA: 0x004F0528 File Offset: 0x004EE728
		public Alert_Hypothermia()
		{
			this.defaultLabel = "AlertHypothermia".Translate();
		}

		// Token: 0x17002CB0 RID: 11440
		// (get) Token: 0x060113D0 RID: 70608 RVA: 0x004F0550 File Offset: 0x004EE750
		private List<Pawn> HypothermiaDangerColonists
		{
			get
			{
				this.hypothermiaDangerColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (!pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
						{
							this.hypothermiaDangerColonistsResult.Add(pawn);
						}
					}
				}
				return this.hypothermiaDangerColonistsResult;
			}
		}

		// Token: 0x060113D1 RID: 70609 RVA: 0x004F05F4 File Offset: 0x004EE7F4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.hypothermiaDangerColonistsResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertHypothermiaDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x060113D2 RID: 70610 RVA: 0x004F0680 File Offset: 0x004EE880
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HypothermiaDangerColonists);
		}

		// Token: 0x0400B2F2 RID: 45810
		private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
	}
}
