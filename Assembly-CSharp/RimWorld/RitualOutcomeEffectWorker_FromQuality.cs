using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002767 RID: 10087
	public class RitualOutcomeEffectWorker_FromQuality : RitualOutcomeEffectWorker
	{
		// Token: 0x170021DE RID: 8670
		// (get) Token: 0x0600DD42 RID: 56642 RVA: 0x004114F4 File Offset: 0x0040F6F4
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x170021DF RID: 8671
		// (get) Token: 0x0600DD43 RID: 56643 RVA: 0x00411744 File Offset: 0x0040F944
		public virtual bool GivesDevelopmentPoints
		{
			get
			{
				return this.def.givesDevelopmentPoints;
			}
		}

		// Token: 0x0600DD44 RID: 56644 RVA: 0x00411463 File Offset: 0x0040F663
		public RitualOutcomeEffectWorker_FromQuality()
		{
		}

		// Token: 0x0600DD45 RID: 56645 RVA: 0x0041146B File Offset: 0x0040F66B
		public RitualOutcomeEffectWorker_FromQuality(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD46 RID: 56646 RVA: 0x00411754 File Offset: 0x0040F954
		public virtual RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			return (from o in this.def.outcomeChances
			where this.OutcomePossible(o, ritual)
			select o).RandomElementByWeight((RitualOutcomePossibility c) => RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(c, quality));
		}

		// Token: 0x0600DD47 RID: 56647 RVA: 0x004117AC File Offset: 0x0040F9AC
		public virtual float GetOutcomeChanceAtQuality(LordJob_Ritual ritual, RitualOutcomePossibility outcome, float quality)
		{
			float num = (from o in this.def.outcomeChances
			where this.OutcomePossible(o, ritual)
			select o).Sum((RitualOutcomePossibility c) => RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(c, quality));
			return RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(outcome, quality) / num;
		}

		// Token: 0x0600DD48 RID: 56648 RVA: 0x00411810 File Offset: 0x0040FA10
		protected static float ChanceWithQuality(RitualOutcomePossibility outcome, float quality)
		{
			if (!outcome.Positive)
			{
				return outcome.chance;
			}
			return Mathf.Max(outcome.chance * quality, 0f);
		}

		// Token: 0x0600DD49 RID: 56649 RVA: 0x00411834 File Offset: 0x0040FA34
		protected virtual void ApplyAttachableOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcomeChance, out string extraLetterText, ref LookTargets letterLookTargets)
		{
			extraLetterText = null;
			if (jobRitual.Ritual.attachableOutcomeEffect != null && jobRitual.Ritual.attachableOutcomeEffect.AppliesToOutcome(jobRitual.Ritual.outcomeEffect.def, outcomeChance))
			{
				jobRitual.Ritual.attachableOutcomeEffect.Worker.Apply(totalPresence, jobRitual, outcomeChance, out extraLetterText, ref letterLookTargets);
			}
		}

		// Token: 0x0600DD4A RID: 56650 RVA: 0x00411894 File Offset: 0x0040FA94
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = this.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text;
			this.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			string text2 = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text2, ref lookTargets);
			}
			string text3 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text4 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text4.NullOrEmpty())
			{
				text3 = text3 + "\n\n" + text4;
			}
			if (text != null)
			{
				text3 = text3 + "\n\n" + text;
			}
			if (text2 != null)
			{
				text3 = text3 + "\n\n" + text2;
			}
			text3 = text3 + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text5;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text5);
			if (text5 != null)
			{
				text3 = text3 + "\n\n" + text5;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text3, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				if (!outcome.roleIdsNotGainingMemory.NullOrEmpty<string>())
				{
					RitualRole ritualRole = jobRitual.assignments.RoleForPawn(keyValuePair.Key, true);
					if (ritualRole != null && outcome.roleIdsNotGainingMemory.Contains(ritualRole.id))
					{
						continue;
					}
				}
				if (outcome.memory != null)
				{
					this.GiveMemoryToPawn(keyValuePair.Key, outcome.memory, jobRitual);
				}
			}
		}

		// Token: 0x0600DD4B RID: 56651 RVA: 0x00411A84 File Offset: 0x0040FC84
		protected void GiveMemoryToPawn(Pawn pawn, ThoughtDef memory, LordJob_Ritual jobRitual)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.mood : null) != null)
			{
				Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn, jobRitual, memory);
				pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
		}

		// Token: 0x0600DD4C RID: 56652 RVA: 0x00411AD0 File Offset: 0x0040FCD0
		protected virtual void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
		}

		// Token: 0x0600DD4D RID: 56653 RVA: 0x00411AD8 File Offset: 0x0040FCD8
		protected virtual void ApplyDevelopmentPoints(Precept_Ritual ritual, RitualOutcomePossibility outcome, out string extraOutcomeDesc)
		{
			if (((ritual != null) ? ritual.ideo : null) != null && ritual.ideo.Fluid)
			{
				if (ritual.ideo.development.Points == ritual.ideo.development.NextReformationDevelopmentPoints)
				{
					extraOutcomeDesc = "RitualOutcomeExtraDesc_DevelopmentPointsAwardedCapped".Translate(ritual.ideo.development.NextReformationDevelopmentPoints);
					return;
				}
				int num = this.def.outcomeChances.IndexOf(outcome);
				int num2;
				if (num >= 0 && ritual.ideo.development.TryGainDevelopmentPointsForRitualOutcome(ritual, num, out num2))
				{
					if (num2 > 0)
					{
						extraOutcomeDesc = "RitualOutcomeExtraDesc_DevelopmentPointsAwarded".Translate(ritual.ideo.development.Points - num2, ritual.ideo.development.Points, num2.ToStringWithSign());
						return;
					}
					extraOutcomeDesc = "RitualOutcomeExtraDesc_NoDevelopmentPointsAwarded".Translate();
					return;
				}
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x0600DD4E RID: 56654 RVA: 0x00411BE0 File Offset: 0x0040FDE0
		protected float GetQuality(LordJob_Ritual jobRitual, float progress)
		{
			float num = this.def.startingQuality;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual))
				{
					num += ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp));
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				num += jobRitual.Ritual.RepeatQualityPenalty;
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			return Mathf.Clamp(num * Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress), this.def.minQuality, this.def.maxQuality);
		}

		// Token: 0x0600DD4F RID: 56655 RVA: 0x00411CE0 File Offset: 0x0040FEE0
		public static Tuple<ExpectationDef, float> GetExpectationsOffset(Map map, PreceptDef ritual)
		{
			if (ritual == null || !ritual.receivesExpectationsQualityOffset)
			{
				return null;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			if (Math.Abs(expectationDef.ritualQualityOffset) > 1E-45f)
			{
				return new Tuple<ExpectationDef, float>(expectationDef, expectationDef.ritualQualityOffset);
			}
			return null;
		}

		// Token: 0x0600DD50 RID: 56656 RVA: 0x00411D24 File Offset: 0x0040FF24
		public virtual string OutcomeQualityBreakdownDesc(float quality, float progress, LordJob_Ritual jobRitual)
		{
			TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, quality.ToStringPercent()).CapitalizeFirst() + ":\n";
			if (this.def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(this.def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual) && Mathf.Abs(ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp))) >= 1E-45f)
				{
					taggedString += "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + jobRitual.Ritual.RepeatQualityPenalty.ToStringPercent();
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
			}
			if (progress < 1f)
			{
				taggedString += "\n  - " + "RitualOutcomeProgress".Translate(jobRitual.Ritual.Label).CapitalizeFirst() + ": x" + Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress).ToStringPercent();
			}
			return taggedString;
		}

		// Token: 0x0600DD51 RID: 56657 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool OutcomePossible(RitualOutcomePossibility chance, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600DD52 RID: 56658 RVA: 0x00411F70 File Offset: 0x00410170
		public override string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			string text = "";
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality)
				{
					string desc = ritualOutcomeComp.GetDesc(null, null);
					if (!desc.NullOrEmpty())
					{
						text = text + "\n  - " + desc.CapitalizeFirst();
					}
				}
			}
			string text2 = ("RitualOutcomeQualityAbstract".Translate(ritual.Label).Resolve().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor) + text;
			return string.Concat(new string[]
			{
				text2,
				"\n  - ",
				"RitualOutcomeProgress".Translate(ritual.Label).Resolve().CapitalizeFirst(),
				": ",
				"OutcomeBonusDesc_QualitySingleOffset".Translate("x" + (RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min * 100f).ToString() + "-" + RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max.ToStringPercent()).Resolve(),
				"."
			});
		}

		// Token: 0x0400953A RID: 38202
		public static FloatRange ProgressToQualityMapping = new FloatRange(0.25f, 1f);
	}
}
