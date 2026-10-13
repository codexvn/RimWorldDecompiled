using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E68 RID: 3688
	public class JobDriver_PlayWalking : JobDriver_BabyPlay
	{
		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005CED RID: 23789 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.PickupBaby;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005CEE RID: 23790 RVA: 0x001D9A38 File Offset: 0x001D7C38
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				if (!this.pawn.IsCarryingPawn(base.Baby))
				{
					return Vector3.zero;
				}
				float f = (float)Find.TickManager.TicksGame / 60f * 2f;
				if (this.pawn.pather.Moving)
				{
					float f2 = Mathf.Sin(f);
					float num = Mathf.Sign(f2);
					return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(f2) * 0.6f) * num * 0.12f, 0f, 0f);
				}
				float z = EasingFunctions.EaseInOutQuint(Mathf.Abs(Mathf.Sin(f) * 0.6f)) * 0.12f;
				return new Vector3(0f, 0f, z);
			}
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x001D9AEA File Offset: 0x001D7CEA
		public static IntVec3 TryFindWanderCell(Pawn pawn, IntVec3 root)
		{
			return RCellFinder.RandomWanderDestFor(pawn, root, 8f, null, PawnUtility.ResolveMaxDanger(pawn, Danger.None), false);
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x001D9B04 File Offset: 0x001D7D04
		private void CheckEndJobSuccess(int delta)
		{
			if (this.roomPlayGainFactor < 0f || this.pawn.IsHashIntervalTick(300, delta))
			{
				this.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
			}
			if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, this.pawn, this.roomPlayGainFactor, delta, null))
			{
				this.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
			}
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x001D9B73 File Offset: 0x001D7D73
		protected override IEnumerable<Toil> Play()
		{
			Toil playWalking = this.PlayWalking();
			yield return playWalking;
			yield return this.Interact();
			yield return Toils_Jump.JumpIf(playWalking, delegate
			{
				IntVec3 c = JobDriver_PlayWalking.TryFindWanderCell(this.pawn, this.pawn.Position);
				if (c.IsValid)
				{
					this.job.SetTarget(TargetIndex.B, c);
					return true;
				}
				return false;
			});
			yield break;
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x001D9B84 File Offset: 0x001D7D84
		private Toil PlayWalking()
		{
			Toil toil = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			toil.initAction = (Action)Delegate.Combine(toil.initAction, new Action(delegate()
			{
				this.job.locomotionUrgency = LocomotionUrgency.Amble;
			}));
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(this.CheckEndJobSuccess));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x001D9BE4 File Offset: 0x001D7DE4
		private Toil Interact()
		{
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
			toil.initAction = (Action)Delegate.Combine(toil.initAction, new Action(delegate()
			{
				this.pawn.Rotation = Rot4.Random;
			}));
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(300, delta))
				{
					this.pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
				}
				this.CheckEndJobSuccess(delta);
			}));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x04004324 RID: 17188
		private const TargetIndex WanderCellInd = TargetIndex.B;

		// Token: 0x04004325 RID: 17189
		private const int InteractionTicks = 600;

		// Token: 0x04004326 RID: 17190
		private const int InteractionIntervalTicks = 300;

		// Token: 0x04004327 RID: 17191
		private const float WanderRadius = 8f;
	}
}
