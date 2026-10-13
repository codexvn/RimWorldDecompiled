using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200251A RID: 9498
	public class Pawn_InteractionsTracker : IExposable
	{
		// Token: 0x17001FE0 RID: 8160
		// (get) Token: 0x0600CF89 RID: 53129 RVA: 0x003CE86C File Offset: 0x003CCA6C
		public InteractionDef LastInteractionDef
		{
			get
			{
				return this.lastInteractionDef;
			}
		}

		// Token: 0x17001FE1 RID: 8161
		// (get) Token: 0x0600CF8A RID: 53130 RVA: 0x003CE874 File Offset: 0x003CCA74
		private RandomSocialMode CurrentSocialMode
		{
			get
			{
				if (!SocialInteractionUtility.CanInitiateRandomInteraction(this.pawn))
				{
					return RandomSocialMode.Off;
				}
				RandomSocialMode randomSocialMode = RandomSocialMode.Normal;
				JobDriver curDriver = this.pawn.jobs.curDriver;
				if (curDriver != null)
				{
					randomSocialMode = curDriver.DesiredSocialMode();
				}
				PawnDuty duty = this.pawn.mindState.duty;
				if (duty != null && duty.SocialModeMax < randomSocialMode)
				{
					randomSocialMode = duty.SocialModeMax;
				}
				if (this.pawn.Drafted && randomSocialMode > RandomSocialMode.Quiet)
				{
					randomSocialMode = RandomSocialMode.Quiet;
				}
				if (this.pawn.InMentalState && randomSocialMode > this.pawn.MentalState.SocialModeMax())
				{
					randomSocialMode = this.pawn.MentalState.SocialModeMax();
				}
				return randomSocialMode;
			}
		}

		// Token: 0x0600CF8B RID: 53131 RVA: 0x003CE916 File Offset: 0x003CCB16
		public Pawn_InteractionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF8C RID: 53132 RVA: 0x003CE948 File Offset: 0x003CCB48
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.wantsRandomInteract, "wantsRandomInteract", false, false);
			Scribe_Values.Look<string>(ref this.lastInteraction, "lastInteraction", null, false);
			Scribe_Values.Look<int>(ref this.lastInteractionTime, "lastInteractionTime", -9999, false);
			Scribe_Values.Look<int>(ref this.lastOccultInteraction, "lastOccultInteraction", -9999, false);
			Scribe_Values.Look<int>(ref this.lastRambling, "lastRambling", -9999, false);
			Scribe_Defs.Look<InteractionDef>(ref this.lastInteractionDef, "lastInteractionDef");
		}

		// Token: 0x0600CF8D RID: 53133 RVA: 0x003CE9CC File Offset: 0x003CCBCC
		public void InteractionsTrackerTickInterval(int delta)
		{
			RandomSocialMode currentSocialMode = this.CurrentSocialMode;
			if (currentSocialMode == RandomSocialMode.Off)
			{
				this.wantsRandomInteract = false;
				return;
			}
			if (currentSocialMode == RandomSocialMode.Quiet)
			{
				this.wantsRandomInteract = false;
			}
			if (!this.wantsRandomInteract)
			{
				if (Find.TickManager.TicksGame > this.lastInteractionTime + 320 && this.pawn.IsHashIntervalTick(60, delta))
				{
					int num = 0;
					switch (currentSocialMode)
					{
					case RandomSocialMode.Quiet:
						num = 22000;
						break;
					case RandomSocialMode.Normal:
						num = 6600;
						break;
					case RandomSocialMode.SuperActive:
						num = 550;
						break;
					}
					if (Rand.MTBEventOccurs((float)num, 1f, 60f) && !this.TryInteractRandomly())
					{
						this.wantsRandomInteract = true;
						return;
					}
				}
			}
			else if (this.pawn.IsHashIntervalTick(91, delta) && this.TryInteractRandomly())
			{
				this.wantsRandomInteract = false;
			}
		}

		// Token: 0x0600CF8E RID: 53134 RVA: 0x003CEA99 File Offset: 0x003CCC99
		public bool InteractedTooRecentlyToInteract()
		{
			return Find.TickManager.TicksGame < this.lastInteractionTime + 120;
		}

		// Token: 0x0600CF8F RID: 53135 RVA: 0x003CEAB0 File Offset: 0x003CCCB0
		public bool CanInteractNowWith(Pawn recipient, InteractionDef interactionDef = null)
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!this.pawn.IsCarryingPawn(recipient))
			{
				if (!recipient.Spawned)
				{
					return false;
				}
				if (!SocialInteractionUtility.IsGoodPositionForInteraction(this.pawn, recipient))
				{
					return false;
				}
			}
			return SocialInteractionUtility.CanInitiateInteraction(this.pawn, interactionDef) && SocialInteractionUtility.CanReceiveInteraction(recipient, interactionDef);
		}

		// Token: 0x0600CF90 RID: 53136 RVA: 0x003CEB0C File Offset: 0x003CCD0C
		public bool TryInteractWith(Pawn recipient, InteractionDef intDef)
		{
			if (DebugSettings.alwaysSocialFight)
			{
				intDef = InteractionDefOf.Insult;
			}
			if (this.pawn == recipient)
			{
				Pawn pawn = this.pawn;
				Log.Warning(((pawn != null) ? pawn.ToString() : null) + " tried to interact with self, interaction=" + intDef.defName);
				return false;
			}
			if (!this.CanInteractNowWith(recipient, intDef))
			{
				return false;
			}
			if (!intDef.ignoreTimeSinceLastInteraction && this.InteractedTooRecentlyToInteract())
			{
				Log.Error(string.Format("{0} tried to do interaction {1} to {2} only {3} ticks since last interaction {4} (min is {5}).", new object[]
				{
					this.pawn,
					intDef,
					recipient,
					Find.TickManager.TicksGame - this.lastInteractionTime,
					this.lastInteraction.ToStringSafe<string>(),
					120
				}));
				return false;
			}
			List<RulePackDef> list = new List<RulePackDef>();
			if (intDef.initiatorThought != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(this.pawn, recipient, intDef.initiatorThought);
			}
			if (intDef.recipientThought != null && recipient.needs.mood != null)
			{
				Pawn_InteractionsTracker.AddInteractionThought(recipient, this.pawn, intDef.recipientThought);
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				this.pawn.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false, false);
			}
			if (intDef.recipientXpGainSkill != null && recipient.RaceProps.Humanlike)
			{
				recipient.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false, false);
			}
			Pawn_IdeoTracker ideo = recipient.ideo;
			if (ideo != null)
			{
				ideo.IncreaseIdeoExposureIfBaby(this.pawn.Ideo, 0.5f);
			}
			bool flag = false;
			if (recipient.RaceProps.Humanlike && recipient.Spawned)
			{
				flag = recipient.interactions.CheckSocialFightStart(intDef, this.pawn);
			}
			string text;
			string str;
			LetterDef letterDef;
			LookTargets lookTargets;
			if (!flag)
			{
				intDef.Worker.Interacted(this.pawn, recipient, list, out text, out str, out letterDef, out lookTargets);
			}
			else
			{
				text = null;
				str = null;
				letterDef = null;
				lookTargets = null;
			}
			MoteMaker.MakeInteractionBubble(this.pawn, recipient, intDef.interactionMote, intDef.GetSymbol(this.pawn.Faction, this.pawn.Ideo), intDef.GetSymbolColor(this.pawn.Faction));
			this.lastInteractionTime = Find.TickManager.TicksGame;
			this.lastInteraction = intDef.defName;
			this.lastInteractionDef = intDef;
			if (recipient.interactions != null)
			{
				recipient.interactions.lastInteractionTime = Find.TickManager.TicksGame;
				recipient.interactions.lastInteraction = intDef.defName;
				recipient.interactions.lastInteractionDef = intDef;
			}
			if (flag)
			{
				list.Add(RulePackDefOf.Sentence_SocialFightStarted);
			}
			PlayLogEntry_Interaction playLogEntry_Interaction = new PlayLogEntry_Interaction(intDef, this.pawn, recipient, list);
			Find.PlayLog.Add(playLogEntry_Interaction);
			if (letterDef != null)
			{
				string text2 = playLogEntry_Interaction.ToGameStringFromPOV(this.pawn, false);
				if (!text.NullOrEmpty())
				{
					text2 = text2 + "\n\n" + text;
				}
				Find.LetterStack.ReceiveLetter(str, text2, letterDef, lookTargets ?? this.pawn, null, null, null, null, 0, true);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (intDef == InteractionDefOf.OccultTeaching)
				{
					Find.ResearchManager.ApplyKnowledge(KnowledgeCategoryDefOf.Basic, 1f);
					this.lastOccultInteraction = GenTicks.TicksAbs;
				}
				this.MetalhorrorDetectionCheck(recipient);
			}
			return true;
		}

		// Token: 0x0600CF91 RID: 53137 RVA: 0x003CEE38 File Offset: 0x003CD038
		private void MetalhorrorDetectionCheck(Pawn recipient)
		{
			bool flag = MetalhorrorUtility.IsInfected(this.pawn);
			Hediff_MetalhorrorImplant firstHediff = recipient.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (!flag && firstHediff != null && !firstHediff.Visible && (Rand.Chance(0.001f) || firstHediff.debugDiscoverNextInteraction))
			{
				TaggedString taggedString = "MetalhorrorReasonInteraction".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString2 = "MetalhorrorNoticedDetailsAppended".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString3 = "InteractionDetectedDesc".Translate(this.pawn.Named("PAWN"), recipient.Named("INFECTED"));
				taggedString3 += string.Format("\n\n{0}", taggedString2);
				MetalhorrorUtility.Detect(recipient, taggedString, taggedString3, 0.3f);
			}
		}

		// Token: 0x0600CF92 RID: 53138 RVA: 0x003CEF30 File Offset: 0x003CD130
		public static void AddInteractionThought(Pawn pawn, Pawn otherPawn, ThoughtDef thoughtDef)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			float statValue = otherPawn.GetStatValue(StatDefOf.SocialImpact, true, -1);
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(thoughtDef);
			thought_Memory.moodPowerFactor = statValue;
			Thought_MemorySocial thought_MemorySocial = thought_Memory as Thought_MemorySocial;
			if (thought_MemorySocial != null)
			{
				thought_MemorySocial.opinionOffset *= statValue;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, otherPawn);
		}

		// Token: 0x0600CF93 RID: 53139 RVA: 0x003CEFA0 File Offset: 0x003CD1A0
		private bool TryInteractRandomly()
		{
			if (this.InteractedTooRecentlyToInteract())
			{
				return false;
			}
			if (!SocialInteractionUtility.CanInitiateRandomInteraction(this.pawn))
			{
				return false;
			}
			List<Pawn> collection = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction);
			Pawn_InteractionsTracker.workingList.Clear();
			Pawn_InteractionsTracker.workingList.AddRange(collection);
			Pawn_InteractionsTracker.workingList.Shuffle<Pawn>();
			List<InteractionDef> allDefsListForReading = DefDatabase<InteractionDef>.AllDefsListForReading;
			for (int i = 0; i < Pawn_InteractionsTracker.workingList.Count; i++)
			{
				Pawn p = Pawn_InteractionsTracker.workingList[i];
				if (p != this.pawn && this.CanInteractNowWith(p, null) && SocialInteractionUtility.CanReceiveRandomInteraction(p) && !this.pawn.HostileTo(p))
				{
					InteractionDef interactionDef = null;
					if (ModsConfig.AnomalyActive)
					{
						if (this.pawn.story.traits.HasTrait(TraitDefOf.Occultist) && (float)GenTicks.TicksAbs >= (float)this.lastOccultInteraction + 60000f)
						{
							interactionDef = InteractionDefOf.OccultTeaching;
						}
						else if (this.pawn.story.IsDisturbing)
						{
							if (Rand.MTBEventOccurs(1f, 60000f, (float)(GenTicks.TicksGame - this.lastRambling)))
							{
								MentalState_InhumanRambling.DoInhumanRambling(this.pawn);
								this.lastRambling = GenTicks.TicksGame;
								Pawn_InteractionsTracker.workingList.Clear();
								return true;
							}
							interactionDef = InteractionDefOf.DisturbingChat;
						}
					}
					if ((interactionDef == null || this.CanInteractNowWith(p, interactionDef)) && (interactionDef != null || allDefsListForReading.TryRandomElementByWeight(delegate(InteractionDef x)
					{
						if (!this.CanInteractNowWith(p, x))
						{
							return 0f;
						}
						return x.Worker.RandomSelectionWeight(this.pawn, p);
					}, out interactionDef)))
					{
						if (this.TryInteractWith(p, interactionDef))
						{
							Pawn_InteractionsTracker.workingList.Clear();
							return true;
						}
						Pawn pawn = this.pawn;
						string str = (pawn != null) ? pawn.ToString() : null;
						string str2 = " failed to interact with ";
						Pawn p2 = p;
						Log.Error(str + str2 + ((p2 != null) ? p2.ToString() : null));
					}
				}
			}
			Pawn_InteractionsTracker.workingList.Clear();
			return false;
		}

		// Token: 0x0600CF94 RID: 53140 RVA: 0x003CF1B8 File Offset: 0x003CD3B8
		public bool CheckSocialFightStart(InteractionDef interaction, Pawn initiator)
		{
			if (!DebugSettings.enableRandomMentalStates)
			{
				return false;
			}
			if (this.pawn.needs.mood == null || TutorSystem.TutorialMode)
			{
				return false;
			}
			if (DebugSettings.alwaysSocialFight || Rand.Value < this.SocialFightChance(interaction, initiator))
			{
				this.StartSocialFight(initiator, "MessageSocialFight");
				return true;
			}
			return false;
		}

		// Token: 0x0600CF95 RID: 53141 RVA: 0x003CF210 File Offset: 0x003CD410
		public void StartSocialFight(Pawn otherPawn, string messageKey = "MessageSocialFight")
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(otherPawn))
			{
				Messages.Message(messageKey.Translate(this.pawn.LabelShort, otherPawn.LabelShort, this.pawn.Named("PAWN1"), otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.ThreatSmall, true);
			}
			this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, false, otherPawn, false, false, false);
			otherPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting, null, false, false, false, this.pawn, false, false, false);
			TaleRecorder.RecordTale(TaleDefOf.SocialFight, new object[]
			{
				this.pawn,
				otherPawn
			});
		}

		// Token: 0x0600CF96 RID: 53142 RVA: 0x003CF2F0 File Offset: 0x003CD4F0
		public bool SocialFightPossible(Pawn otherPawn)
		{
			if (!this.pawn.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!SocialInteractionUtility.HasAnyVerbForSocialFight(this.pawn) || !SocialInteractionUtility.HasAnyVerbForSocialFight(otherPawn))
			{
				return false;
			}
			if (this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			if (otherPawn.Downed || this.pawn.Downed)
			{
				return false;
			}
			if (this.pawn.IsPrisoner && !otherPawn.IsPrisoner)
			{
				return false;
			}
			if (this.pawn.IsSlave && !otherPawn.IsSlave)
			{
				return false;
			}
			DevelopmentalStage developmentalStage = this.pawn.ageTracker.CurLifeStage.developmentalStage;
			return developmentalStage != DevelopmentalStage.Baby && (Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - otherPawn.ageTracker.AgeBiologicalYears) <= 6 || developmentalStage != DevelopmentalStage.Child) && (developmentalStage != DevelopmentalStage.Adult || otherPawn.ageTracker.AgeBiologicalYears >= 13) && (this.pawn.genes == null || this.pawn.genes.SocialFightChanceFactor > 0f) && (otherPawn.genes == null || otherPawn.genes.SocialFightChanceFactor > 0f);
		}

		// Token: 0x0600CF97 RID: 53143 RVA: 0x003CF428 File Offset: 0x003CD628
		public float SocialFightChance(InteractionDef interaction, Pawn initiator)
		{
			if (!this.SocialFightPossible(initiator))
			{
				return 0f;
			}
			float num = interaction.socialFightBaseChance;
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation));
			num *= Mathf.InverseLerp(0.3f, 1f, this.pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].CurStage != null)
				{
					num *= hediffs[i].CurStage.socialFightChanceFactor;
				}
			}
			float num2 = (float)this.pawn.relations.OpinionOf(initiator);
			if (num2 < 0f)
			{
				num *= GenMath.LerpDouble(-100f, 0f, 4f, 1f, num2);
			}
			else
			{
				num *= GenMath.LerpDouble(0f, 100f, 1f, 0.6f, num2);
			}
			if (this.pawn.RaceProps.Humanlike)
			{
				List<Trait> allTraits = this.pawn.story.traits.allTraits;
				for (int j = 0; j < allTraits.Count; j++)
				{
					if (!allTraits[j].Suppressed)
					{
						num *= allTraits[j].CurrentData.socialFightChanceFactor;
					}
				}
			}
			int num3 = Mathf.Abs(this.pawn.ageTracker.AgeBiologicalYears - initiator.ageTracker.AgeBiologicalYears);
			if (num3 > 10)
			{
				if (num3 > 50)
				{
					num3 = 50;
				}
				num *= GenMath.LerpDouble(10f, 50f, 1f, 0.25f, (float)num3);
			}
			if (this.pawn.IsSlave)
			{
				num *= 0.5f;
			}
			if (this.pawn.genes != null)
			{
				num *= this.pawn.genes.SocialFightChanceFactor;
			}
			if (initiator.genes != null)
			{
				num *= initiator.genes.SocialFightChanceFactor;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x04008D3F RID: 36159
		private Pawn pawn;

		// Token: 0x04008D40 RID: 36160
		private bool wantsRandomInteract;

		// Token: 0x04008D41 RID: 36161
		private int lastInteractionTime = -9999;

		// Token: 0x04008D42 RID: 36162
		private string lastInteraction;

		// Token: 0x04008D43 RID: 36163
		private InteractionDef lastInteractionDef;

		// Token: 0x04008D44 RID: 36164
		private int lastOccultInteraction = -9999;

		// Token: 0x04008D45 RID: 36165
		private int lastRambling = -9999;

		// Token: 0x04008D46 RID: 36166
		private const int RandomInteractMTBTicks_Quiet = 22000;

		// Token: 0x04008D47 RID: 36167
		private const int RandomInteractMTBTicks_Normal = 6600;

		// Token: 0x04008D48 RID: 36168
		private const int RandomInteractMTBTicks_SuperActive = 550;

		// Token: 0x04008D49 RID: 36169
		public const int RandomInteractIntervalMin = 320;

		// Token: 0x04008D4A RID: 36170
		private const int RandomInteractCheckInterval = 60;

		// Token: 0x04008D4B RID: 36171
		private const float SlaveSocialFightFactor = 0.5f;

		// Token: 0x04008D4C RID: 36172
		private const int ChildSocialFightAgeRange = 6;

		// Token: 0x04008D4D RID: 36173
		private const int InteractIntervalAbsoluteMin = 120;

		// Token: 0x04008D4E RID: 36174
		public const int DirectTalkInteractInterval = 320;

		// Token: 0x04008D4F RID: 36175
		public const float IdeoExposurePointsInteraction = 0.5f;

		// Token: 0x04008D50 RID: 36176
		private const float MetalhorrorDetectionChance = 0.001f;

		// Token: 0x04008D51 RID: 36177
		private const float MetalhorrorDetectionNoticed = 0.3f;

		// Token: 0x04008D52 RID: 36178
		private const float DisturbingInhumanRamblingDaysMTB = 1f;

		// Token: 0x04008D53 RID: 36179
		private const float OccultInteractionWait = 60000f;

		// Token: 0x04008D54 RID: 36180
		private List<Pawn> tmpPawns;

		// Token: 0x04008D55 RID: 36181
		private List<int> tmpTicks;

		// Token: 0x04008D56 RID: 36182
		private static readonly List<Pawn> workingList = new List<Pawn>();
	}
}
