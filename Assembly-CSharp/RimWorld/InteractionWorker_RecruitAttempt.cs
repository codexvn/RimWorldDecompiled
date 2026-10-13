using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002404 RID: 9220
	public class InteractionWorker_RecruitAttempt : InteractionWorker
	{
		// Token: 0x0600C7BC RID: 51132 RVA: 0x003A4278 File Offset: 0x003A2478
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			bool flag = recipient.AnimalOrWildMan();
			Pawn_RelationsTracker relations = recipient.relations;
			int num = (relations != null) ? relations.OpinionOf(initiator) : 0;
			bool flag2 = initiator.InspirationDef == InspirationDefOf.Inspired_Recruitment && !flag && recipient.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.ReduceResistance);
			if (DebugSettings.instantRecruit)
			{
				recipient.guest.resistance = 0f;
			}
			if (!flag)
			{
				if (recipient.guest.resistance > 0f && !flag2)
				{
					float num2 = InteractionWorker_RecruitAttempt.ResistanceImpactFactorCurve_Mood.Evaluate((recipient.needs.mood == null) ? 1f : recipient.needs.mood.CurInstantLevelPercentage);
					float num3 = InteractionWorker_RecruitAttempt.ResistanceImpactFactorCurve_Opinion.Evaluate((float)num);
					float statValue = initiator.GetStatValue(StatDefOf.NegotiationAbility, true, -1);
					float num4 = 1f;
					num4 *= statValue;
					num4 *= num2;
					num4 *= num3;
					float resistanceReduce = num4;
					num4 = Mathf.Min(num4, recipient.guest.resistance);
					float resistance = recipient.guest.resistance;
					recipient.guest.resistance = Mathf.Max(0f, recipient.guest.resistance - num4);
					float resistance2 = recipient.guest.resistance;
					if (recipient.guest.resistance <= 0f)
					{
						recipient.guest.SetLastResistanceReduceData(initiator, resistanceReduce, statValue, num2, num3);
					}
					float num5 = (resistance > 0f) ? Mathf.Max(0.1f, resistance) : 0f;
					float num6 = (recipient.guest.resistance > 0f) ? Mathf.Max(0.1f, recipient.guest.resistance) : 0f;
					string text = "TextMote_ResistanceReduced".Translate(num5.ToString("F1"), num6.ToString("F1"));
					if (recipient.needs.mood != null && recipient.needs.mood.CurLevelPercentage < 0.4f)
					{
						text += "\n(" + "lowMood".Translate() + ")";
					}
					if (recipient.relations != null && (float)recipient.relations.OpinionOf(initiator) < -0.01f)
					{
						text += "\n(" + "lowOpinion".Translate() + ")";
					}
					MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, text, 8f);
					if (recipient.guest.resistance == 0f)
					{
						TaggedString taggedString = "MessagePrisonerResistanceBroken".Translate(recipient.LabelShort, initiator.LabelShort, initiator.Named("WARDEN"), recipient.Named("PRISONER"));
						if (recipient.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.AttemptRecruit))
						{
							taggedString += " " + "MessagePrisonerResistanceBroken_RecruitAttempsWillBegin".Translate();
						}
						Messages.Message(taggedString, recipient, MessageTypeDefOf.PositiveEvent, true);
						return;
					}
				}
				else
				{
					InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, out letterLabel, out letterText, true, false);
					if (!letterLabel.NullOrEmpty())
					{
						letterDef = LetterDefOf.PositiveEvent;
					}
					lookTargets = new LookTargets(new TargetInfo[]
					{
						recipient,
						initiator
					});
					if (flag2)
					{
						initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Recruitment);
					}
					extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
					recipient.guest.SetRecruitmentData(initiator);
				}
				return;
			}
			float num7;
			if (initiator.InspirationDef == InspirationDefOf.Inspired_Taming)
			{
				num7 = 1f;
				initiator.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Taming);
			}
			else
			{
				num7 = initiator.GetStatValue(StatDefOf.TameAnimalChance, true, -1);
				float statValue2 = recipient.GetStatValue(StatDefOf.Wildness, true, -1);
				num7 *= InteractionWorker_RecruitAttempt.TameChanceFactorCurve_Wildness.Evaluate(statValue2);
				if (recipient.IsPrisonerInPrisonCell())
				{
					num7 *= 0.6f;
				}
				if (initiator.relations.DirectRelationExists(PawnRelationDefOf.Bond, recipient))
				{
					num7 *= 4f;
				}
				if (initiator.Ideo != null && initiator.Ideo.IsVeneratedAnimal(recipient))
				{
					num7 *= 2f;
				}
			}
			if (Rand.Chance(num7))
			{
				InteractionWorker_RecruitAttempt.DoRecruit(initiator, recipient, out letterLabel, out letterText, true, false);
				if (!letterLabel.NullOrEmpty())
				{
					letterDef = LetterDefOf.PositiveEvent;
				}
				lookTargets = new LookTargets(new TargetInfo[]
				{
					recipient,
					initiator
				});
				extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptAccepted);
				return;
			}
			TaggedString taggedString2 = "TextMote_TameFail".Translate(num7.ToStringPercent());
			MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, taggedString2, 8f);
			recipient.mindState.CheckStartMentalStateBecauseRecruitAttempted(initiator);
			extraSentencePacks.Add(RulePackDefOf.Sentence_RecruitAttemptRejected);
		}

		// Token: 0x0600C7BD RID: 51133 RVA: 0x003A479C File Offset: 0x003A299C
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, bool useAudiovisualEffects = true)
		{
			string text;
			string text2;
			InteractionWorker_RecruitAttempt.DoRecruit(recruiter, recruitee, out text, out text2, useAudiovisualEffects, true);
		}

		// Token: 0x0600C7BE RID: 51134 RVA: 0x003A47B8 File Offset: 0x003A29B8
		public static void DoRecruit(Pawn recruiter, Pawn recruitee, out string letterLabel, out string letter, bool useAudiovisualEffects = true, bool sendLetter = true)
		{
			letterLabel = null;
			letter = null;
			string value = recruitee.LabelIndefinite();
			Pawn recruiter2 = recruiter;
			Faction faction = ((recruiter2 != null) ? recruiter2.Faction : null) ?? Faction.OfPlayer;
			if (recruiter == null)
			{
				sendLetter = false;
				useAudiovisualEffects = false;
			}
			bool flag = recruitee.Name != null;
			RecruitUtility.Recruit(recruitee, faction, recruiter);
			if (recruitee.RaceProps.Humanlike)
			{
				if (useAudiovisualEffects)
				{
					letterLabel = "LetterLabelMessageRecruitSuccess".Translate() + ": " + recruitee.LabelShortCap;
					if (sendLetter)
					{
						Find.LetterStack.ReceiveLetter(letterLabel, "MessageRecruitSuccess".Translate(recruiter, recruitee, recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")), LetterDefOf.PositiveEvent, recruitee, null, null, null, null, 0, true);
					}
				}
				if (recruiter != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.Recruited, new object[]
					{
						recruiter,
						recruitee
					});
					recruiter.records.Increment(RecordDefOf.PrisonersRecruited);
					if (recruitee.needs.mood != null)
					{
						recruitee.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RecruitedMe, recruiter, null);
					}
				}
				QuestUtility.SendQuestTargetSignals(recruitee.questTags, "Recruited", recruitee.Named("SUBJECT"));
			}
			else
			{
				if (useAudiovisualEffects)
				{
					if (!flag)
					{
						Messages.Message("MessageTameAndNameSuccess".Translate(recruiter.LabelShort, value, recruitee.Name.ToStringFull, recruiter.Named("RECRUITER"), recruitee.Named("RECRUITEE")).AdjustedFor(recruitee, "PAWN", true), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					else
					{
						Messages.Message("MessageTameSuccess".Translate(recruiter.LabelShort, value, recruiter.Named("RECRUITER")), recruitee, MessageTypeDefOf.PositiveEvent, true);
					}
					if (recruiter.Spawned && recruitee.Spawned)
					{
						MoteMaker.ThrowText((recruiter.DrawPos + recruitee.DrawPos) / 2f, recruiter.Map, "TextMote_TameSuccess".Translate(), 8f);
					}
				}
				if (recruiter != null)
				{
					recruiter.records.Increment(RecordDefOf.AnimalsTamed);
					RelationsUtility.TryDevelopBondRelation(recruiter, recruitee, 0.01f);
					if (Rand.Chance(Mathf.Lerp(0.02f, 1f, recruitee.GetStatValue(StatDefOf.Wildness, true, -1))) || recruitee.IsWildMan())
					{
						TaleRecorder.RecordTale(TaleDefOf.TamedAnimal, new object[]
						{
							recruiter,
							recruitee
						});
					}
					if (PawnsFinder.AllMapsWorldAndTemporary_Alive.Count((Pawn p) => p.playerSettings != null && p.playerSettings.Master == recruiter) >= 5)
					{
						TaleRecorder.RecordTale(TaleDefOf.IncreasedMenagerie, new object[]
						{
							recruiter,
							recruitee
						});
					}
				}
			}
			if (recruitee.caller != null)
			{
				recruitee.caller.DoCall(false);
			}
		}

		// Token: 0x04008841 RID: 34881
		public const float BaseResistanceReductionPerInteraction = 1f;

		// Token: 0x04008842 RID: 34882
		private static readonly SimpleCurve ResistanceImpactFactorCurve_Mood = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.5f),
				true
			}
		};

		// Token: 0x04008843 RID: 34883
		private static readonly SimpleCurve ResistanceImpactFactorCurve_Opinion = new SimpleCurve
		{
			{
				new CurvePoint(-100f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.5f),
				true
			}
		};

		// Token: 0x04008844 RID: 34884
		private const float MaxMoodForWarning = 0.4f;

		// Token: 0x04008845 RID: 34885
		private const float MaxOpinionForWarning = -0.01f;

		// Token: 0x04008846 RID: 34886
		private const float WildmanPrisonerChanceFactor = 0.6f;

		// Token: 0x04008847 RID: 34887
		private const float VeneratedAnimalChanceFactor = 2f;

		// Token: 0x04008848 RID: 34888
		private static readonly SimpleCurve TameChanceFactorCurve_Wildness = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(0f, 2f),
				true
			}
		};

		// Token: 0x04008849 RID: 34889
		private const float TameChanceFactor_Bonded = 4f;

		// Token: 0x0400884A RID: 34890
		private const float ChanceToDevelopBondRelationOnTamed = 0.01f;

		// Token: 0x0400884B RID: 34891
		private const int MenagerieTaleThreshold = 5;
	}
}
