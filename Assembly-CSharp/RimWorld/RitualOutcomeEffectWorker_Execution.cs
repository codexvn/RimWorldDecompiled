using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002770 RID: 10096
	public class RitualOutcomeEffectWorker_Execution : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E3 RID: 8675
		// (get) Token: 0x0600DD75 RID: 56693 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD76 RID: 56694 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Execution()
		{
		}

		// Token: 0x0600DD77 RID: 56695 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Execution(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD78 RID: 56696 RVA: 0x004130F8 File Offset: 0x004112F8
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			bool flag = false;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				if (pawn.IsSlave)
				{
					Need_Suppression need_Suppression;
					if (pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
					{
						need_Suppression.CurLevel = 1f;
					}
					flag = true;
				}
				else
				{
					base.GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
				}
			}
			string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text3.NullOrEmpty())
			{
				text2 = text2 + "\n\n" + text3;
			}
			if (flag)
			{
				text2 += "\n\n" + "RitualOutcomeExtraDesc_Execution".Translate();
			}
			if (text != null)
			{
				text2 = text2 + "\n\n" + text;
			}
			string text4;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
			if (text4 != null)
			{
				text2 = text2 + "\n\n" + text4;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
		}
	}
}
