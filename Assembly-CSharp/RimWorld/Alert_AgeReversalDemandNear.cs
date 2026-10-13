using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200303E RID: 12350
	public class Alert_AgeReversalDemandNear : Alert
	{
		// Token: 0x060115B6 RID: 71094 RVA: 0x004FAB5C File Offset: 0x004F8D5C
		public Alert_AgeReversalDemandNear()
		{
			this.defaultLabel = "AlertAgeReversalDemandNear".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x060115B7 RID: 71095 RVA: 0x004FAB8C File Offset: 0x004F8D8C
		private void CalcPawnsNearDeadline()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				Ideo ideo = pawn.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded) && ThoughtWorker_AgeReversalDemanded.CanHaveThought(pawn))
				{
					float ageReversalDemandedDeadlineTicks = (float)pawn.ageTracker.AgeReversalDemandedDeadlineTicks;
					float num = CompBiosculpterPod.HasBiotunedAutoAgeReversePod(pawn) ? -60000f : 300000f;
					if (ageReversalDemandedDeadlineTicks <= num)
					{
						this.targets.Add(pawn);
					}
				}
			}
		}

		// Token: 0x060115B8 RID: 71096 RVA: 0x004FAC34 File Offset: 0x004F8E34
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertAgeReversalDemandDesc".Translate();
			foreach (Pawn pawn in this.targets)
			{
				long num = pawn.ageTracker.AgeReversalDemandedDeadlineTicks;
				string key;
				if (num > 0L)
				{
					if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.Recruited)
					{
						key = "AlertAgeReversalDemandDesc_Recruit";
					}
					else if (pawn.ageTracker.LastAgeReversalReason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
					{
						key = "AlertAgeReversalDemandDesc_Next";
					}
					else
					{
						key = "AlertAgeReversalDemandDesc_Initial";
					}
				}
				else
				{
					num = -num;
					key = "AlertAgeReversalDemandDesc_Overdue";
				}
				taggedString += "\n  - " + key.Translate(pawn.Named("PAWN"), pawn.Faction.Named("FACTION"), ((int)num).ToStringTicksToPeriodVerbose(true, true).Named("DURATION"));
			}
			return taggedString;
		}

		// Token: 0x060115B9 RID: 71097 RVA: 0x004FAD2C File Offset: 0x004F8F2C
		public override AlertReport GetReport()
		{
			this.CalcPawnsNearDeadline();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3BF RID: 46015
		private const float WarnWhenCloserThanTicks = 300000f;

		// Token: 0x0400B3C0 RID: 46016
		private const float WarnWhenCloserThanTicksAutoAgeReverse = -60000f;

		// Token: 0x0400B3C1 RID: 46017
		private List<Pawn> targets = new List<Pawn>();
	}
}
