using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E60 RID: 3680
	public abstract class JobDriver_BabyPlay : JobDriver
	{
		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06005CAA RID: 23722 RVA: 0x000024E7 File Offset: 0x000006E7
		protected Pawn Baby
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005CAB RID: 23723
		protected abstract JobDriver_BabyPlay.StartingConditions StartingCondition { get; }

		// Token: 0x06005CAC RID: 23724 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x001D8D99 File Offset: 0x001D6F99
		public Toil SetPlayPercentage()
		{
			return Toils_General.Do(delegate
			{
				if (this.initialPlayPercentage == -1f)
				{
					Need_Play play = this.Baby.needs.play;
					this.initialPlayPercentage = ((play != null) ? play.CurLevelPercentage : 1f);
				}
			});
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x001D8DAC File Offset: 0x001D6FAC
		public void AddPlayThoughtIfAppropriate()
		{
			if (this.Baby.DestroyedOrNull())
			{
				return;
			}
			Need_Play play = this.Baby.needs.play;
			float num = (play != null) ? play.CurLevelPercentage : -1f;
			if (this.initialPlayPercentage >= 0f && num - this.initialPlayPercentage > 0.6f)
			{
				this.Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.PlayedWithMe, this.pawn, null);
			}
		}

		// Token: 0x06005CAF RID: 23727
		protected abstract IEnumerable<Toil> Play();

		// Token: 0x06005CB0 RID: 23728 RVA: 0x001D8E2F File Offset: 0x001D702F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			base.AddFailCondition(delegate
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return !ChildcareUtility.CanSuckle(this.Baby, out breastfeedFailReason);
			});
			base.AddFinishAction(delegate(JobCondition jobCondition)
			{
				this.AddPlayThoughtIfAppropriate();
			});
			base.SetFinalizerJob(delegate(JobCondition condition)
			{
				if (!this.finishedSetup)
				{
					return null;
				}
				return ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, this.Baby);
			});
			foreach (Toil toil in this.CreateStartingCondition())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return this.SetPlayPercentage();
			yield return Toils_General.DoAtomic(delegate
			{
				this.finishedSetup = true;
			});
			foreach (Toil toil2 in this.Play())
			{
				yield return toil2;
			}
			enumerator = null;
			foreach (Toil toil3 in JobDriver_PickupToHold.Toils(this, TargetIndex.A, true))
			{
				yield return toil3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x001D8E3F File Offset: 0x001D703F
		private IEnumerable<Toil> CreateStartingCondition()
		{
			if (this.StartingCondition == JobDriver_BabyPlay.StartingConditions.PickupBaby)
			{
				foreach (Toil toil in JobDriver_PickupToHold.Toils(this, TargetIndex.A, false))
				{
					yield return toil;
				}
				IEnumerator<Toil> enumerator = null;
			}
			else
			{
				if (this.StartingCondition != JobDriver_BabyPlay.StartingConditions.GotoBaby)
				{
					throw new NotImplementedException(this.StartingCondition.ToString());
				}
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x001D8E50 File Offset: 0x001D7050
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.initialPlayPercentage, "initialPlayPercentage", 0f, false);
			Scribe_Values.Look<float>(ref this.roomPlayGainFactor, "roomPlayGainFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.finishedSetup, "finishedSetup", false, false);
		}

		// Token: 0x04004302 RID: 17154
		protected const float BabyPlayMemoryNeedIncrease = 0.6f;

		// Token: 0x04004303 RID: 17155
		protected float roomPlayGainFactor = -1f;

		// Token: 0x04004304 RID: 17156
		protected float initialPlayPercentage = -1f;

		// Token: 0x04004305 RID: 17157
		protected bool finishedSetup;

		// Token: 0x04004306 RID: 17158
		protected const TargetIndex BabyInd = TargetIndex.A;

		// Token: 0x02000E61 RID: 3681
		public enum StartingConditions
		{
			// Token: 0x04004308 RID: 17160
			Invalid,
			// Token: 0x04004309 RID: 17161
			GotoBaby,
			// Token: 0x0400430A RID: 17162
			PickupBaby
		}
	}
}
