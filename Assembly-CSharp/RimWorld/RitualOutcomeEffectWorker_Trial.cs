using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200276E RID: 10094
	public class RitualOutcomeEffectWorker_Trial : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E1 RID: 8673
		// (get) Token: 0x0600DD6B RID: 56683 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD6C RID: 56684 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Trial()
		{
		}

		// Token: 0x0600DD6D RID: 56685 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Trial(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD6E RID: 56686 RVA: 0x00412CE4 File Offset: 0x00410EE4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			Pawn pawn = jobRitual.PawnWithRole("leader");
			Pawn pawn2 = jobRitual.PawnWithRole("convict");
			LookTargets lookTargets = pawn2;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string str = pawn2.LabelShort + " " + outcome.label;
			TaggedString taggedString = outcome.description.Formatted(pawn2.Named("PAWN"), pawn.Named("PROSECUTOR"));
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + text2;
			}
			taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			if (outcome.Positive)
			{
				ChoiceLetter let = LetterMaker.MakeLetter(str, taggedString, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null);
				Find.LetterStack.ReceiveLetter(let, null, 0, true);
				pawn2.guilt.Notify_Guilty(900000);
				pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialConvicted, null, null);
				return;
			}
			Find.LetterStack.ReceiveLetter(str, taggedString, LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialFailed, null, null);
			pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialExonerated, null, null);
		}

		// Token: 0x0600DD6F RID: 56687 RVA: 0x00412E9C File Offset: 0x0041109C
		public override RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			if (!Rand.Chance(quality))
			{
				return this.def.outcomeChances[0];
			}
			return this.def.outcomeChances[1];
		}

		// Token: 0x0600DD70 RID: 56688 RVA: 0x00412EC9 File Offset: 0x004110C9
		public override string ExpectedQualityLabel()
		{
			return "ExpectedConvictionChance".Translate();
		}

		// Token: 0x04009548 RID: 38216
		public const int ConvictGuiltyForDays = 15;
	}
}
