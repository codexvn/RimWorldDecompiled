using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200276A RID: 10090
	public class RitualOutcomeEffectWorker_Speech : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E0 RID: 8672
		// (get) Token: 0x0600DD5A RID: 56666 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD5B RID: 56667 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Speech()
		{
		}

		// Token: 0x0600DD5C RID: 56668 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Speech(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD5D RID: 56669 RVA: 0x00412130 File Offset: 0x00410330
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			Pawn organizer = jobRitual.Organizer;
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			ThoughtDef memory = outcome.memory;
			LookTargets lookTargets = organizer;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string text2 = "";
			string text3 = "";
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				Pawn key = keyValuePair.Key;
				if (key != organizer && organizer.Position.InHorDistOf(key.Position, 18f))
				{
					Thought_Memory thought_Memory = base.MakeMemory(key, jobRitual, memory);
					thought_Memory.otherPawn = organizer;
					thought_Memory.moodPowerFactor = ((key.Ideo == organizer.Ideo) ? 1f : 0.5f);
					key.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
					if (memory == ThoughtDefOf.InspirationalSpeech)
					{
						if (Rand.Chance(RitualOutcomeEffectWorker_Speech.InspirationChanceFromInspirationalSpeech))
						{
							InspirationDef randomAvailableInspirationDef = key.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
							if (randomAvailableInspirationDef != null && key.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterSpeechInspiration".Translate(key.Named("PAWN"), organizer.Named("SPEAKER")), true))
							{
								text2 = text2 + "  - " + key.NameShortColored.Resolve() + "\n";
							}
						}
						if (ModsConfig.IdeologyActive && key.Ideo != organizer.Ideo && Rand.Chance(RitualOutcomeEffectWorker_Speech.ConversionChanceFromInspirationalSpeech))
						{
							key.ideo.SetIdeo(organizer.Ideo);
							text3 = text3 + "  - " + key.NameShortColored.Resolve() + "\n";
						}
					}
				}
			}
			TaggedString taggedString = "LetterFinishedSpeech".Translate(organizer.Named("ORGANIZER")).CapitalizeFirst() + " " + ("Letter" + memory.defName).Translate() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (!text3.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechConvertedListeners".Translate(organizer.Named("PAWN"), organizer.Ideo.Named("IDEO")).CapitalizeFirst() + ":\n\n" + text3.TrimEndNewlines();
			}
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + "LetterSpeechInspiredListeners".Translate() + "\n\n" + text2.TrimEndNewlines();
			}
			if (progress < 1f)
			{
				taggedString += "\n\n" + "LetterSpeechInterrupted".Translate(progress.ToStringPercent(), organizer.Named("ORGANIZER"));
			}
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			string text4;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
			if (text4 != null)
			{
				taggedString += "\n\n" + text4;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, RitualOutcomeEffectWorker_Speech.PositiveOutcome(memory) ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			if (jobRitual.Ritual.def == PreceptDefOf.ThroneSpeech)
			{
				Ability ability = organizer.abilities.GetAbility(AbilityDefOf.Speech, true);
				RoyalTitle mostSeniorTitle = organizer.royalty.MostSeniorTitle;
				if (ability != null && mostSeniorTitle != null)
				{
					ability.StartCooldown(mostSeniorTitle.def.speechCooldown.RandomInRange);
				}
			}
		}

		// Token: 0x0600DD5E RID: 56670 RVA: 0x00412560 File Offset: 0x00410760
		private static bool PositiveOutcome(ThoughtDef outcome)
		{
			return outcome == ThoughtDefOf.EncouragingSpeech || outcome == ThoughtDefOf.InspirationalSpeech;
		}

		// Token: 0x04009541 RID: 38209
		private static readonly float InspirationChanceFromInspirationalSpeech = 0.05f;

		// Token: 0x04009542 RID: 38210
		private static readonly float ConversionChanceFromInspirationalSpeech = 0.02f;
	}
}
