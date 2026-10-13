using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F70 RID: 3952
	public class MentalState : IExposable
	{
		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x060061B6 RID: 25014 RVA: 0x001EEC78 File Offset: 0x001ECE78
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x060061B7 RID: 25015 RVA: 0x001EEC80 File Offset: 0x001ECE80
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x060061B8 RID: 25016 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x060061B9 RID: 25017 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x001EEC90 File Offset: 0x001ECE90
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<bool>(ref this.causedByDamage, "causedByDamage", false, false);
			Scribe_Values.Look<bool>(ref this.causedByPsycast, "causedByPsycast", false, false);
			Scribe_References.Look<Pawn>(ref this.causedByPawn, "causedByPawn", false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
			Scribe_References.Look<Faction>(ref this.sourceFaction, "sourceFaction", false);
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreStart()
		{
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x001EED2C File Offset: 0x001ECF2C
		public virtual void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = this.def.recoveryMessage.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"));
				if (!taggedString.NullOrEmpty())
				{
					Messages.Message(taggedString.AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x001EEDC8 File Offset: 0x001ECFC8
		public virtual void MentalStateTick(int delta)
		{
			if (this.pawn.IsHashIntervalTick(30, delta))
			{
				this.age += 30;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 30f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
				{
					this.RecoverFromState();
					return;
				}
				if (this.def.recoverFromSleep && !this.pawn.Awake())
				{
					this.RecoverFromState();
					return;
				}
			}
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x001EEE80 File Offset: 0x001ED080
		public void RecoverFromState()
		{
			if (this.pawn.MentalState != this)
			{
				string str = "Recovered from ";
				MentalStateDef mentalStateDef = this.def;
				string str2 = (mentalStateDef != null) ? mentalStateDef.ToString() : null;
				string str3 = " but pawn's mental state is not this, it is ";
				MentalState mentalState = this.pawn.MentalState;
				Log.Error(str + str2 + str3 + ((mentalState != null) ? mentalState.ToString() : null));
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null, null);
				}
				this.pawn.mindState.mentalBreaker.Notify_RecoveredFromMentalState();
				this.pawn.mindState.mentalFitGenerator.Notify_RecoveredFromMentalState();
				if (this.pawn.story != null && this.pawn.story.traits != null)
				{
					foreach (Trait trait in this.pawn.story.traits.allTraits)
					{
						if (!trait.Suppressed)
						{
							trait.Notify_MentalStateEndedOn(this.pawn, this.causedByMood);
						}
					}
				}
				if (this.def.IsAggro)
				{
					this.pawn.mindState.enemyTarget = null;
				}
			}
			if (this.def.stopsJobs && this.pawn.Spawned)
			{
				this.pawn.jobs.StopAll(true, true);
			}
			this.PostEnd();
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x001EF054 File Offset: 0x001ED254
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x00075162 File Offset: 0x00073362
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x001EF064 File Offset: 0x001ED264
		public virtual TaggedString GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SlaughteredTarget()
		{
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ReleasedTarget()
		{
		}

		// Token: 0x0400463A RID: 17978
		public Pawn pawn;

		// Token: 0x0400463B RID: 17979
		public MentalStateDef def;

		// Token: 0x0400463C RID: 17980
		protected int age;

		// Token: 0x0400463D RID: 17981
		public bool causedByMood;

		// Token: 0x0400463E RID: 17982
		public bool causedByDamage;

		// Token: 0x0400463F RID: 17983
		public bool causedByPsycast;

		// Token: 0x04004640 RID: 17984
		public Pawn causedByPawn;

		// Token: 0x04004641 RID: 17985
		public int forceRecoverAfterTicks = -1;

		// Token: 0x04004642 RID: 17986
		public Faction sourceFaction;

		// Token: 0x04004643 RID: 17987
		protected const int MentalStateTickInterval = 30;
	}
}
