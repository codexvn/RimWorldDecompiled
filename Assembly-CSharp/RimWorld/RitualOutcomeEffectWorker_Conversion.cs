using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200276F RID: 10095
	public class RitualOutcomeEffectWorker_Conversion : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E2 RID: 8674
		// (get) Token: 0x0600DD71 RID: 56689 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD72 RID: 56690 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Conversion()
		{
		}

		// Token: 0x0600DD73 RID: 56691 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Conversion(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD74 RID: 56692 RVA: 0x00412EDC File Offset: 0x004110DC
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
			Pawn pawn = jobRitual.PawnWithRole("moralist");
			Pawn pawn2 = jobRitual.PawnWithRole("convertee");
			float ideoCertaintyOffset = outcome.ideoCertaintyOffset;
			if (ideoCertaintyOffset <= -1f)
			{
				pawn2.ideo.SetIdeo(pawn.Ideo);
			}
			else
			{
				pawn2.ideo.OffsetCertainty(ideoCertaintyOffset);
			}
			foreach (Pawn pawn3 in totalPresence.Keys)
			{
				if (pawn3 != pawn && pawn3 != pawn2 && outcome.memory != null)
				{
					Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn3, jobRitual, outcome.memory);
					pawn3.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
				}
			}
			TaggedString taggedString = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + text2;
			}
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text3);
			if (text3 != null)
			{
				taggedString += "\n\n" + text3;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
		}
	}
}
