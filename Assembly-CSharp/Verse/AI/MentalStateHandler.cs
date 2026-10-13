using System;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000F6C RID: 3948
	public class MentalStateHandler : IExposable
	{
		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06006193 RID: 24979 RVA: 0x001EE081 File Offset: 0x001EC281
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06006194 RID: 24980 RVA: 0x001EE08C File Offset: 0x001EC28C
		public MentalStateDef CurStateDef
		{
			get
			{
				if (this.curStateInt == null)
				{
					return null;
				}
				return this.curStateInt.def;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06006195 RID: 24981 RVA: 0x001EE0A3 File Offset: 0x001EC2A3
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x000024DF File Offset: 0x000006DF
		public MentalStateHandler()
		{
		}

		// Token: 0x06006197 RID: 24983 RVA: 0x001EE0AB File Offset: 0x001EC2AB
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x001EE0BC File Offset: 0x001EC2BC
		public void ExposeData()
		{
			Scribe_Deep.Look<MentalState>(ref this.curStateInt, "curState", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.neverFleeIndividual, "neverFleeIndividual", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curStateInt != null)
				{
					this.curStateInt.pawn = this.pawn;
				}
				if (Current.ProgramState != ProgramState.Entry && this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x001EE140 File Offset: 0x001EC340
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x001EE148 File Offset: 0x001EC348
		public void MentalStateHandlerTickInterval(int delta)
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed && this.curStateInt.def.recoverFromDowned)
				{
					string str = "In mental state while downed or deathresting, but not allowed: ";
					Pawn pawn = this.pawn;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null));
					this.CurState.RecoverFromState();
					return;
				}
				this.curStateInt.MentalStateTick(delta);
			}
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x001EE1B8 File Offset: 0x001EC3B8
		public bool MentalBreaksBlocked()
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.CurStage != null && hediff.CurStage.blocksMentalBreaks)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x001EE230 File Offset: 0x001EC430
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forced = false, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false, bool causedByDamage = false, bool causedByPsycast = false)
		{
			if (!forced && this.MentalBreaksBlocked())
			{
				return false;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.preventsMentalBreaks)
			{
				return false;
			}
			if (this.CurStateDef == stateDef || (!forceWake && !this.pawn.Awake()))
			{
				return false;
			}
			if (TutorSystem.TutorialMode && this.pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!forced && !stateDef.Worker.StateCanOccur(this.pawn))
			{
				return false;
			}
			if (this.curStateInt != null && !transitionSilently)
			{
				this.curStateInt.RecoverFromState();
			}
			MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
			mentalState.pawn = this.pawn;
			mentalState.def = stateDef;
			mentalState.causedByMood = causedByMood;
			mentalState.causedByDamage = causedByDamage;
			mentalState.causedByPsycast = causedByPsycast;
			mentalState.causedByPawn = otherPawn;
			MentalState_SocialFighting mentalState_SocialFighting = mentalState as MentalState_SocialFighting;
			if (mentalState_SocialFighting != null)
			{
				mentalState_SocialFighting.otherPawn = otherPawn;
			}
			mentalState.PreStart();
			if (!transitionSilently)
			{
				if ((this.pawn.IsColonist || this.pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
				{
					TaleRecorder.RecordTale(stateDef.tale, new object[]
					{
						this.pawn
					});
				}
				if (stateDef.IsExtreme && this.pawn.IsPlayerControlledCaravanMember())
				{
					Messages.Message("MessageCaravanMemberHasExtremeMentalBreak".Translate(), this.pawn.GetCaravan(), MessageTypeDefOf.ThreatSmall, true);
				}
				this.pawn.records.Increment(RecordDefOf.TimesInMentalState);
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			if (this.pawn.mechanitor != null)
			{
				this.pawn.mechanitor.UndraftAllMechs();
			}
			this.curStateInt = mentalState;
			if (this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (stateDef != null && stateDef.IsAggro && this.pawn.caller != null)
			{
				this.pawn.caller.Notify_InAggroMentalState();
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_InMentalState(this.pawn, stateDef);
			}
			if (this.curStateInt != null)
			{
				this.curStateInt.PostStart(reason);
			}
			if (stateDef.stopsJobs && this.pawn.CurJob != null)
			{
				this.pawn.jobs.StopAll(false, true);
				if (this.pawn.IsCarrying())
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (this.pawn.Spawned && forceWake)
			{
				if (lord != null)
				{
					lord.Notify_DormancyWakeup();
				}
				if (!this.pawn.Awake())
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			CompTransporter compTransporter = this.pawn.ParentHolder as CompTransporter;
			if (compTransporter != null)
			{
				Thing thing;
				compTransporter.innerContainer.TryDrop(this.pawn, ThingPlaceMode.Near, out thing, null, null);
			}
			if (!transitionSilently && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = mentalState.GetBeginLetterText();
				if (!taggedString.NullOrEmpty())
				{
					string str = (stateDef.beginLetterLabel ?? stateDef.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
					if (!reason.NullOrEmpty())
					{
						taggedString += "\n\n" + reason;
					}
					Find.LetterStack.ReceiveLetter(str, taggedString, stateDef.beginLetterDef, this.pawn, null, null, null, null, 0, true);
				}
			}
			return true;
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x001EE61C File Offset: 0x001EC81C
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x001EE71C File Offset: 0x001EC91C
		internal void ClearMentalStateDirect()
		{
			if (this.curStateInt == null)
			{
				return;
			}
			this.curStateInt = null;
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ExitMentalState", this.pawn.Named("SUBJECT"));
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
		}

		// Token: 0x0400462F RID: 17967
		private Pawn pawn;

		// Token: 0x04004630 RID: 17968
		private MentalState curStateInt;

		// Token: 0x04004631 RID: 17969
		public bool neverFleeIndividual;
	}
}
