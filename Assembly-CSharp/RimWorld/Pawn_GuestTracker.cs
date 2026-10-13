using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200244A RID: 9290
	public class Pawn_GuestTracker : IExposable
	{
		// Token: 0x17001E59 RID: 7769
		// (get) Token: 0x0600C90E RID: 51470 RVA: 0x003ABE4B File Offset: 0x003AA04B
		public Faction HostFaction
		{
			get
			{
				return this.hostFactionInt;
			}
		}

		// Token: 0x17001E5A RID: 7770
		// (get) Token: 0x0600C90F RID: 51471 RVA: 0x003ABE53 File Offset: 0x003AA053
		public Faction SlaveFaction
		{
			get
			{
				return this.slaveFactionInt;
			}
		}

		// Token: 0x17001E5B RID: 7771
		// (get) Token: 0x0600C910 RID: 51472 RVA: 0x003ABE5B File Offset: 0x003AA05B
		public GuestStatus GuestStatus
		{
			get
			{
				return this.guestStatusInt;
			}
		}

		// Token: 0x17001E5C RID: 7772
		// (get) Token: 0x0600C911 RID: 51473 RVA: 0x003ABE63 File Offset: 0x003AA063
		public PrisonerInteractionModeDef ExclusiveInteractionMode
		{
			get
			{
				return this.interactionMode;
			}
		}

		// Token: 0x17001E5D RID: 7773
		// (get) Token: 0x0600C912 RID: 51474 RVA: 0x003ABE6B File Offset: 0x003AA06B
		public bool CanBeBroughtFood
		{
			get
			{
				return this.interactionMode != PrisonerInteractionModeDefOf.Execution && (this.interactionMode != PrisonerInteractionModeDefOf.Release || this.pawn.Downed);
			}
		}

		// Token: 0x17001E5E RID: 7774
		// (get) Token: 0x0600C913 RID: 51475 RVA: 0x003ABE96 File Offset: 0x003AA096
		public bool IsPrisoner
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Prisoner;
			}
		}

		// Token: 0x17001E5F RID: 7775
		// (get) Token: 0x0600C914 RID: 51476 RVA: 0x003ABEA1 File Offset: 0x003AA0A1
		public bool IsSlave
		{
			get
			{
				return this.guestStatusInt == GuestStatus.Slave;
			}
		}

		// Token: 0x17001E60 RID: 7776
		// (get) Token: 0x0600C915 RID: 51477 RVA: 0x003ABEAC File Offset: 0x003AA0AC
		public bool ScheduledForInteraction
		{
			get
			{
				return this.pawn.mindState.lastAssignedInteractTime < Find.TickManager.TicksGame - 10000 && this.pawn.mindState.interactionsToday < 2;
			}
		}

		// Token: 0x17001E61 RID: 7777
		// (get) Token: 0x0600C916 RID: 51478 RVA: 0x003ABEE5 File Offset: 0x003AA0E5
		// (set) Token: 0x0600C917 RID: 51479 RVA: 0x003ABEED File Offset: 0x003AA0ED
		public bool Released
		{
			get
			{
				return this.releasedInt;
			}
			set
			{
				if (value == this.releasedInt)
				{
					return;
				}
				this.releasedInt = value;
				ReachabilityUtility.ClearCacheFor(this.pawn);
			}
		}

		// Token: 0x17001E62 RID: 7778
		// (get) Token: 0x0600C918 RID: 51480 RVA: 0x003ABF0C File Offset: 0x003AA10C
		public bool PrisonerIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.HostFaction == null)
				{
					return false;
				}
				if (this.pawn.InAggroMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (PrisonBreakUtility.IsPrisonBreaking(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001E63 RID: 7779
		// (get) Token: 0x0600C919 RID: 51481 RVA: 0x003ABF8C File Offset: 0x003AA18C
		public bool SlaveIsSecure
		{
			get
			{
				if (this.Released)
				{
					return false;
				}
				if (this.pawn.InMentalState)
				{
					return false;
				}
				if (this.pawn.Spawned)
				{
					if (this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.exitMapOnArrival)
					{
						return false;
					}
					if (SlaveRebellionUtility.IsRebelling(this.pawn))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001E64 RID: 7780
		// (get) Token: 0x0600C91A RID: 51482 RVA: 0x003ABFFC File Offset: 0x003AA1FC
		public bool ShouldWaitInsteadOfEscaping
		{
			get
			{
				if (!this.IsPrisoner)
				{
					return false;
				}
				Map mapHeld = this.pawn.MapHeld;
				return mapHeld != null && mapHeld.mapPawns.FreeColonistsSpawnedCount != 0 && Find.TickManager.TicksGame < this.ticksWhenAllowedToEscapeAgain;
			}
		}

		// Token: 0x17001E65 RID: 7781
		// (get) Token: 0x0600C91B RID: 51483 RVA: 0x003AC045 File Offset: 0x003AA245
		public float Resistance
		{
			get
			{
				return this.resistance;
			}
		}

		// Token: 0x17001E66 RID: 7782
		// (get) Token: 0x0600C91C RID: 51484 RVA: 0x003AC050 File Offset: 0x003AA250
		// (set) Token: 0x0600C91D RID: 51485 RVA: 0x003AC10C File Offset: 0x003AA30C
		public bool Recruitable
		{
			get
			{
				if (!Find.Storyteller.difficulty.unwaveringPrisoners)
				{
					return true;
				}
				if (PawnUtility.EverBeenColonistOrTameAnimal(this.pawn))
				{
					return true;
				}
				if (this.pawn.Faction != null && this.pawn.Faction == Faction.OfPlayerSilentFail && !this.pawn.IsSlave && !this.pawn.IsPrisoner)
				{
					return true;
				}
				if (this.pawn.IsSubhuman)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return true;
				}
				Lord lord = this.pawn.GetLord();
				return (((lord != null) ? lord.LordJob : null) is LordJob_TradeWithColony && this.pawn.GetTraderCaravanRole() == TraderCaravanRole.Chattel) || this.recruitable;
			}
			set
			{
				this.recruitable = value;
			}
		}

		// Token: 0x17001E67 RID: 7783
		// (get) Token: 0x0600C91E RID: 51486 RVA: 0x003AC115 File Offset: 0x003AA315
		public bool ScheduledForSlaveSuppression
		{
			get
			{
				return this.pawn.mindState.lastSlaveSuppressedTick < Find.TickManager.TicksGame - 60000;
			}
		}

		// Token: 0x17001E68 RID: 7784
		// (get) Token: 0x0600C91F RID: 51487 RVA: 0x003AC139 File Offset: 0x003AA339
		public bool EverEnslaved
		{
			get
			{
				return this.everEnslaved;
			}
		}

		// Token: 0x0600C920 RID: 51488 RVA: 0x003AC144 File Offset: 0x003AA344
		public Pawn_GuestTracker()
		{
		}

		// Token: 0x0600C921 RID: 51489 RVA: 0x003AC1A8 File Offset: 0x003AA3A8
		public Pawn_GuestTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C922 RID: 51490 RVA: 0x003AC212 File Offset: 0x003AA412
		public void SetupRecruitable()
		{
			this.recruitable = !Rand.Chance(HealthTuning.NonRecruitableChanceOverPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent));
		}

		// Token: 0x0600C923 RID: 51491 RVA: 0x003AC234 File Offset: 0x003AA434
		public void GuestTrackerTickInterval(int delta)
		{
			if (ModsConfig.BiotechActive && this.pawn.IsHashIntervalTick(15000, delta) && SanguophageUtility.CanSafelyBeQueuedForHemogenExtraction(this.pawn) && this.guestStatusInt == GuestStatus.Prisoner && this.IsInteractionEnabled(PrisonerInteractionModeDefOf.HemogenFarm))
			{
				HealthCardUtility.CreateSurgeryBill(this.pawn, RecipeDefOf.ExtractHemogenPack, null, null, false);
			}
			if (this.pawn.IsHashIntervalTick(2500, delta))
			{
				float num = PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.pawn, null, false);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					PrisonBreakUtility.StartPrisonBreak(this.pawn);
				}
			}
			if (this.pawn.IsHashIntervalTick(2500, delta))
			{
				float num2 = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(this.pawn);
				if (num2 >= 0f && Rand.MTBEventOccurs(num2, 60000f, 2500f))
				{
					SlaveRebellionUtility.StartSlaveRebellion(this.pawn, false);
				}
			}
		}

		// Token: 0x0600C924 RID: 51492 RVA: 0x003AC31E File Offset: 0x003AA51E
		public void RandomizeJoinStatus()
		{
			if (ModsConfig.IdeologyActive)
			{
				this.joinStatus = ((!this.Recruitable || Rand.Value < 0.75f) ? JoinStatus.JoinAsSlave : JoinStatus.JoinAsColonist);
				return;
			}
			this.joinStatus = JoinStatus.JoinAsColonist;
		}

		// Token: 0x0600C925 RID: 51493 RVA: 0x003AC34D File Offset: 0x003AA54D
		public void SetExclusiveInteraction(PrisonerInteractionModeDef def)
		{
			if (def.isNonExclusiveInteraction)
			{
				Log.ErrorOnce("Attempted to set guest exclusive interaction to a non-exclusive interaction type: " + def.defName + ", use ToggleNonExclusiveInteraction to change non-exclusive interaction type.", 746345623);
				return;
			}
			this.interactionMode = def;
		}

		// Token: 0x0600C926 RID: 51494 RVA: 0x003AC380 File Offset: 0x003AA580
		public void ToggleNonExclusiveInteraction(PrisonerInteractionModeDef def, bool enabled)
		{
			if (!def.isNonExclusiveInteraction)
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Attempted to ",
					enabled ? "enable" : "disable",
					" an exclusive interaction type (",
					def.defName,
					") via non-exclusive method, use SetExclusiveInteraction to change exclusive interaction type."
				}), 4635128);
				return;
			}
			if (enabled && !this.enabledNonExclusiveInteractions.Contains(def))
			{
				this.enabledNonExclusiveInteractions.Add(def);
				return;
			}
			if (!enabled && this.enabledNonExclusiveInteractions.Contains(def))
			{
				this.enabledNonExclusiveInteractions.Remove(def);
			}
		}

		// Token: 0x0600C927 RID: 51495 RVA: 0x003AC41A File Offset: 0x003AA61A
		public bool IsInteractionEnabled(PrisonerInteractionModeDef def)
		{
			return this.interactionMode == def || this.enabledNonExclusiveInteractions.Contains(def);
		}

		// Token: 0x0600C928 RID: 51496 RVA: 0x003AC434 File Offset: 0x003AA634
		public bool HasInteractionWith(Func<PrisonerInteractionModeDef, bool> validator)
		{
			if (validator(this.interactionMode))
			{
				return true;
			}
			foreach (PrisonerInteractionModeDef arg in this.enabledNonExclusiveInteractions)
			{
				if (validator(arg))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C929 RID: 51497 RVA: 0x003AC4A0 File Offset: 0x003AA6A0
		public bool IsInteractionDisabled(PrisonerInteractionModeDef def)
		{
			return !this.IsInteractionEnabled(def);
		}

		// Token: 0x0600C92A RID: 51498 RVA: 0x003AC4AC File Offset: 0x003AA6AC
		public void SetNoInteraction()
		{
			this.SetExclusiveInteraction(PrisonerInteractionModeDefOf.MaintainOnly);
			this.enabledNonExclusiveInteractions.Clear();
		}

		// Token: 0x0600C92B RID: 51499 RVA: 0x003AC4C4 File Offset: 0x003AA6C4
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.hostFactionInt, "hostFaction", false);
			Scribe_References.Look<Faction>(ref this.slaveFactionInt, "slaveFaction", false);
			Scribe_Values.Look<GuestStatus>(ref this.guestStatusInt, "guestStatus", GuestStatus.Guest, false);
			Scribe_Values.Look<JoinStatus>(ref this.joinStatus, "joinStatus", JoinStatus.Undefined, false);
			Scribe_Defs.Look<PrisonerInteractionModeDef>(ref this.interactionMode, "interactionMode");
			Scribe_Defs.Look<SlaveInteractionModeDef>(ref this.slaveInteractionMode, "slaveInteractionMode");
			Scribe_Values.Look<bool>(ref this.releasedInt, "released", false, false);
			Scribe_Values.Look<int>(ref this.ticksWhenAllowedToEscapeAgain, "ticksWhenAllowedToEscapeAgain", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.spotToWaitInsteadOfEscaping, "spotToWaitInsteadOfEscaping", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.lastPrisonBreakTicks, "lastPrisonBreakTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.everParticipatedInPrisonBreak, "everParticipatedInPrisonBreak", false, false);
			Scribe_Values.Look<bool>(ref this.getRescuedThoughtOnUndownedBecauseOfPlayer, "getRescuedThoughtOnUndownedBecauseOfPlayer", false, false);
			Scribe_Values.Look<float>(ref this.resistance, "resistance", -1f, false);
			Scribe_Values.Look<float>(ref this.will, "will", -1f, false);
			Scribe_Values.Look<int>(ref this.lastRecruiterOpinion, "lastRecruiterOpinion", 0, false);
			Scribe_Values.Look<string>(ref this.lastRecruiterName, "lastRecruiterName", null, false);
			Scribe_Values.Look<bool>(ref this.hasOpinionOfLastRecruiter, "hasOpinionOfLastRecruiter", false, false);
			Scribe_Values.Look<bool>(ref this.everEnslaved, "everEnslaved", false, false);
			Scribe_References.Look<Ideo>(ref this.ideoForConversion, "ideoForConversion", false);
			Scribe_Values.Look<bool>(ref this.recruitable, "recruitable", true, false);
			Scribe_Values.Look<bool>(ref this.leftAfterRescue, "leftAfterRescue", false, false);
			Scribe_Collections.Look<PrisonerInteractionModeDef>(ref this.enabledNonExclusiveInteractions, "enabledNonExclusiveInteractions", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<ResistanceInteractionData>(ref this.lastResistanceInteractionData, "lastResistanceInteractionData", Array.Empty<object>());
			Scribe_Deep.Look<ResistanceInteractionData>(ref this.finalResistanceInteractionData, "finalResistanceInteractionData", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.interactionMode == null || (!this.interactionMode.allowOnWildMan && this.pawn.IsWildMan()))
				{
					this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
				}
				if (this.joinStatus == JoinStatus.Undefined)
				{
					this.RandomizeJoinStatus();
				}
				if (this.enabledNonExclusiveInteractions == null)
				{
					this.enabledNonExclusiveInteractions = new List<PrisonerInteractionModeDef>();
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600C92C RID: 51500 RVA: 0x003AC6F0 File Offset: 0x003AA8F0
		public void SetRecruitmentData(Pawn recruiter)
		{
			this.lastRecruiterName = recruiter.LabelShort;
			this.hasOpinionOfLastRecruiter = (this.pawn.relations != null);
			this.lastRecruiterOpinion = (this.hasOpinionOfLastRecruiter ? this.pawn.relations.OpinionOf(recruiter) : 0);
			this.finalResistanceInteractionData = this.lastResistanceInteractionData;
			this.lastResistanceInteractionData = null;
		}

		// Token: 0x0600C92D RID: 51501 RVA: 0x003AC752 File Offset: 0x003AA952
		public void SetLastResistanceReduceData(Pawn initiator, float resistanceReduce, float negotiationFactor, float moodFactor, float opinionFactor)
		{
			this.lastResistanceInteractionData = new ResistanceInteractionData
			{
				initiatorNegotiationAbilityFactor = negotiationFactor,
				recruiteeMoodFactor = moodFactor,
				resistanceReduction = resistanceReduce,
				initiatorName = initiator.LabelShort,
				recruiterOpinionFactor = opinionFactor
			};
		}

		// Token: 0x0600C92E RID: 51502 RVA: 0x003AC78C File Offset: 0x003AA98C
		public void Notify_WardensOfIdeoLost(Ideo ideo)
		{
			if (!ModsConfig.IdeologyActive || this.interactionMode != PrisonerInteractionModeDefOf.Convert || this.ideoForConversion == null || this.ideoForConversion != ideo)
			{
				return;
			}
			this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
			this.ideoForConversion = null;
			Messages.Message("MessageNoWardenOfIdeo".Translate(this.pawn.Named("PRISONER"), ideo.memberName.Named("MEMBERNAME")), new LookTargets(this.pawn), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x0600C92F RID: 51503 RVA: 0x003AC818 File Offset: 0x003AAA18
		public void SetGuestStatus(Faction newHost, GuestStatus guestStatus = GuestStatus.Guest)
		{
			if (newHost != null)
			{
				this.Released = false;
			}
			switch (guestStatus)
			{
			case GuestStatus.Guest:
				if (this.pawn.Faction.HostileTo(newHost))
				{
					string[] array = new string[8];
					array[0] = "Tried to make ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " a guest of ";
					array[3] = ((newHost != null) ? newHost.ToString() : null);
					array[4] = " but their faction ";
					int num2 = 5;
					Faction faction = this.pawn.Faction;
					array[num2] = ((faction != null) ? faction.ToString() : null);
					array[6] = " is hostile to ";
					array[7] = ((newHost != null) ? newHost.ToString() : null);
					Log.Error(string.Concat(array));
					return;
				}
				if (newHost != null && newHost == this.pawn.Faction)
				{
					string str = "Tried to make ";
					Pawn pawn2 = this.pawn;
					string str2 = (pawn2 != null) ? pawn2.ToString() : null;
					string str3 = " a guest of their own faction ";
					Faction faction2 = this.pawn.Faction;
					Log.Error(str + str2 + str3 + ((faction2 != null) ? faction2.ToString() : null));
					return;
				}
				if (newHost == null)
				{
					this.slaveFactionInt = null;
				}
				break;
			case GuestStatus.Prisoner:
			{
				if (newHost == this.HostFaction && this.IsPrisoner)
				{
					return;
				}
				this.pawn.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
				Pawn pawn3 = this.pawn;
				MentalStateDef mentalStateDef = this.pawn.MentalStateDef;
				bool clearMentalState = mentalStateDef == null || mentalStateDef.recoverFromCaptured;
				pawn3.ClearMind_NewTemp(newHost != null, false, clearMentalState, false);
				this.pawn.DropAndForbidEverything(false, false);
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.MadePrisoner, null);
				}
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				float num3 = this.pawn.kindDef.initialResistanceRange.Value.RandomInRange;
				Pawn_RoyaltyTracker royalty = this.pawn.royalty;
				RoyalTitle royalTitle = (royalty != null) ? royalty.MostSeniorTitle : null;
				if (royalTitle != null)
				{
					num3 += royalTitle.def.recruitmentResistanceOffset;
				}
				this.resistance = (float)GenMath.RoundRandom(num3);
				this.will = (this.everEnslaved ? 2.5f : this.pawn.kindDef.initialWillRange.Value.RandomInRange);
				if (ModsConfig.IdeologyActive && this.interactionMode == PrisonerInteractionModeDefOf.Enslave)
				{
					this.interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;
				}
				if (this.guestStatusInt == GuestStatus.Slave && this.slaveFactionInt != null)
				{
					this.pawn.SetFaction(this.slaveFactionInt, null);
				}
				break;
			}
			case GuestStatus.Slave:
			{
				if (newHost == this.pawn.Faction && this.IsSlave)
				{
					return;
				}
				Lord lord2 = this.pawn.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this.pawn, PawnLostCondition.MadeSlave, null);
				}
				Faction faction3 = this.pawn.Faction;
				if (this.pawn.Faction != newHost)
				{
					this.pawn.SetFaction(newHost, null);
				}
				if (this.slaveFactionInt == null)
				{
					this.slaveFactionInt = faction3;
				}
				if (!this.everEnslaved || this.slaveInteractionMode == SlaveInteractionModeDefOf.Imprison || this.slaveInteractionMode == SlaveInteractionModeDefOf.Emancipate)
				{
					this.slaveInteractionMode = SlaveInteractionModeDefOf.Suppress;
				}
				this.everEnslaved = true;
				break;
			}
			default:
				Log.Error(string.Format("Unknown GuestStatus type {0}", guestStatus));
				return;
			}
			this.guestStatusInt = guestStatus;
			Faction faction4 = this.hostFactionInt;
			this.hostFactionInt = ((guestStatus != GuestStatus.Slave) ? newHost : null);
			this.pawn.Notify_DisabledWorkTypesChanged();
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
			this.pawn.health.surgeryBills.Clear();
			Pawn_Ownership ownership = this.pawn.ownership;
			if (ownership != null)
			{
				ownership.Notify_ChangedGuestStatus();
			}
			Ideo ideo = this.pawn.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberGuestStatusChanged(this.pawn);
			}
			if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.Notify_ChangedGuestStatus();
			}
			ReachabilityUtility.ClearCacheFor(this.pawn);
			if (this.pawn.Spawned)
			{
				this.pawn.Map.mapPawns.UpdateRegistryForPawn(this.pawn);
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this.pawn);
			if ((guestStatus == GuestStatus.Prisoner || guestStatus == GuestStatus.Slave) && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.ResetMedicalCare();
			}
			if (faction4 != this.hostFactionInt)
			{
				QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ChangedHostFaction", this.pawn.Named("SUBJECT"), this.hostFactionInt.Named("FACTION"));
			}
			if ((this.pawn.Inhumanized() || this.pawn.kindDef.studiableAsPrisoner) && this.pawn.MapHeld != null)
			{
				Find.StudyManager.UpdateStudiableCache(this.pawn, this.pawn.MapHeld);
			}
		}

		// Token: 0x0600C930 RID: 51504 RVA: 0x003ACD10 File Offset: 0x003AAF10
		public void CapturedBy(Faction by, Pawn byPawn = null)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberCaptured(this.pawn, by);
			}
			this.SetGuestStatus(by, GuestStatus.Prisoner);
			if (this.IsPrisoner && byPawn != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
				{
					byPawn,
					this.pawn
				});
				byPawn.records.Increment(RecordDefOf.PeopleCaptured);
			}
			if (by == Faction.OfPlayer && !this.Recruitable && !Find.History.everCapturedUnrecruitablePawn)
			{
				Find.History.everCapturedUnrecruitablePawn = true;
				TaggedString taggedString = ModsConfig.IdeologyActive ? ("OptionalEnslaveDesc".Translate(this.pawn.Named("PAWN")) + ", ") : TaggedString.Empty;
				Find.LetterStack.ReceiveLetter("LetterLabelUnrecruitablePawnCaptured".Translate(), "LetterTextUnrecruitablePawnCaptured".Translate(this.pawn.Named("PAWN"), taggedString.Named("OPTIONALENSLAVEDESC")), LetterDefOf.NeutralEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600C931 RID: 51505 RVA: 0x003ACE32 File Offset: 0x003AB032
		public void WaitInsteadOfEscapingForDefaultTicks()
		{
			this.WaitInsteadOfEscapingFor(25000);
		}

		// Token: 0x0600C932 RID: 51506 RVA: 0x003ACE3F File Offset: 0x003AB03F
		public void WaitInsteadOfEscapingFor(int ticks)
		{
			if (!this.IsPrisoner)
			{
				return;
			}
			this.ticksWhenAllowedToEscapeAgain = Find.TickManager.TicksGame + ticks;
			this.spotToWaitInsteadOfEscaping = IntVec3.Invalid;
		}

		// Token: 0x0600C933 RID: 51507 RVA: 0x003ACE67 File Offset: 0x003AB067
		public Texture2D GetIcon()
		{
			return GuestUtility.GetGuestIcon(this.guestStatusInt);
		}

		// Token: 0x0600C934 RID: 51508 RVA: 0x003ACE74 File Offset: 0x003AB074
		public string GetLabel()
		{
			if (this.IsSlave)
			{
				return "Slave".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0600C935 RID: 51509 RVA: 0x003ACEA4 File Offset: 0x003AB0A4
		internal void Notify_PawnUndowned()
		{
			if (this.pawn.RaceProps.Humanlike && (this.HostFaction.IsPlayerSafe() || (this.pawn.IsWildMan() && this.pawn.InBed() && this.pawn.CurrentBed().Faction == Faction.OfPlayer)) && !this.IsPrisoner && this.pawn.SpawnedOrAnyParentSpawned && !this.leftAfterRescue)
			{
				if (this.getRescuedThoughtOnUndownedBecauseOfPlayer)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Rescued, null, null);
					}
				}
				if (this.pawn.Faction == null || this.pawn.Faction.def.rescueesCanJoin)
				{
					Map mapHeld = this.pawn.MapHeld;
					float num;
					if (!this.pawn.SafeTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp) || mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) || (ModsConfig.BiotechActive && mapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)))
					{
						num = 1f;
					}
					else
					{
						num = 0.5f;
					}
					if (Rand.ValueSeeded(this.pawn.thingIDNumber ^ 8976612) < num)
					{
						this.pawn.SetFaction(Faction.OfPlayer, null);
						Find.LetterStack.ReceiveLetter("LetterLabelRescueeJoins".Translate(this.pawn.Named("PAWN")), "LetterRescueeJoins".Translate(this.pawn.Named("PAWN")), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
					}
					else
					{
						Messages.Message("MessageRescueeDidntJoin".Translate().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
						this.leftAfterRescue = true;
					}
				}
			}
			this.getRescuedThoughtOnUndownedBecauseOfPlayer = false;
		}

		// Token: 0x0600C936 RID: 51510 RVA: 0x003AD0CD File Offset: 0x003AB2CD
		public void Notify_PawnRecruited()
		{
			this.slaveFactionInt = null;
		}

		// Token: 0x040088D1 RID: 35025
		private Pawn pawn;

		// Token: 0x040088D2 RID: 35026
		private PrisonerInteractionModeDef interactionMode = PrisonerInteractionModeDefOf.MaintainOnly;

		// Token: 0x040088D3 RID: 35027
		private List<PrisonerInteractionModeDef> enabledNonExclusiveInteractions = new List<PrisonerInteractionModeDef>();

		// Token: 0x040088D4 RID: 35028
		public SlaveInteractionModeDef slaveInteractionMode = SlaveInteractionModeDefOf.NoInteraction;

		// Token: 0x040088D5 RID: 35029
		private Faction hostFactionInt;

		// Token: 0x040088D6 RID: 35030
		public GuestStatus guestStatusInt;

		// Token: 0x040088D7 RID: 35031
		public JoinStatus joinStatus;

		// Token: 0x040088D8 RID: 35032
		private Faction slaveFactionInt;

		// Token: 0x040088D9 RID: 35033
		public string lastRecruiterName;

		// Token: 0x040088DA RID: 35034
		public int lastRecruiterOpinion;

		// Token: 0x040088DB RID: 35035
		public bool hasOpinionOfLastRecruiter;

		// Token: 0x040088DC RID: 35036
		public ResistanceInteractionData lastResistanceInteractionData;

		// Token: 0x040088DD RID: 35037
		public ResistanceInteractionData finalResistanceInteractionData;

		// Token: 0x040088DE RID: 35038
		private bool releasedInt;

		// Token: 0x040088DF RID: 35039
		private int ticksWhenAllowedToEscapeAgain;

		// Token: 0x040088E0 RID: 35040
		public IntVec3 spotToWaitInsteadOfEscaping = IntVec3.Invalid;

		// Token: 0x040088E1 RID: 35041
		public int lastPrisonBreakTicks = -1;

		// Token: 0x040088E2 RID: 35042
		public bool everParticipatedInPrisonBreak;

		// Token: 0x040088E3 RID: 35043
		public float resistance = -1f;

		// Token: 0x040088E4 RID: 35044
		public float will = -1f;

		// Token: 0x040088E5 RID: 35045
		public Ideo ideoForConversion;

		// Token: 0x040088E6 RID: 35046
		private bool recruitable = true;

		// Token: 0x040088E7 RID: 35047
		private bool everEnslaved;

		// Token: 0x040088E8 RID: 35048
		public bool getRescuedThoughtOnUndownedBecauseOfPlayer;

		// Token: 0x040088E9 RID: 35049
		public bool leftAfterRescue;

		// Token: 0x040088EA RID: 35050
		private const int DefaultWaitInsteadOfEscapingTicks = 25000;

		// Token: 0x040088EB RID: 35051
		public const int MinInteractionInterval = 10000;

		// Token: 0x040088EC RID: 35052
		public const int MaxInteractionsPerDay = 2;

		// Token: 0x040088ED RID: 35053
		private const int CheckInitiatePrisonBreakIntervalTicks = 2500;

		// Token: 0x040088EE RID: 35054
		private const int CheckInitiateSlaveRebellionIntervalTicks = 2500;

		// Token: 0x040088EF RID: 35055
		private const int MinSlaveSuppressionIntervalTicks = 60000;

		// Token: 0x040088F0 RID: 35056
		public const float DefaultWillIfPreviouslyEnslaved = 2.5f;

		// Token: 0x040088F1 RID: 35057
		private const int CheckHemogenBillInterval = 15000;
	}
}
