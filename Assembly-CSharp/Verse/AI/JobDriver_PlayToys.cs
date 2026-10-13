using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E66 RID: 3686
	public class JobDriver_PlayToys : JobDriver_BabyPlay
	{
		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005CD9 RID: 23769 RVA: 0x001D95FF File Offset: 0x001D77FF
		private Thing ToyBox
		{
			get
			{
				return base.TargetThingB;
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005CDA RID: 23770 RVA: 0x001D9607 File Offset: 0x001D7807
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)base.TargetThingC;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005CDB RID: 23771 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.PickupBaby;
			}
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x001D9614 File Offset: 0x001D7814
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && (this.Bed == null || this.pawn.Reserve(this.Bed, this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x001D96A1 File Offset: 0x001D78A1
		protected override IEnumerable<Toil> Play()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.Touch);
			yield return this.DropBaby();
			yield return this.PlayToil();
			yield break;
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x001D96B4 File Offset: 0x001D78B4
		private Toil PlayToil()
		{
			Toil toil = ToilMaker.MakeToil("PlayToil");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.motesToMaintain.NullOrEmpty<Mote>())
				{
					Vector3 a = base.Baby.TrueCenter();
					Vector3 v = IntVec3.North.ToVector3();
					float num = 72f;
					this.motesToMaintain = new Mote[5];
					for (int i = 0; i < 5; i++)
					{
						Vector3 loc = a + v.RotatedBy(num * (float)i + JobDriver_PlayToys.ToyRandomAngleOffset.RandomInRange) * 0.5f;
						this.motesToMaintain[i] = MoteMaker.MakeStaticMote(loc, base.Map, ThingDefOf.Mote_Toy, 1f, false, 0f);
					}
				}
				this.pawn.rotationTracker.FaceTarget(base.Baby);
				for (int j = 0; j < this.motesToMaintain.Length; j++)
				{
					Mote mote = this.motesToMaintain[j];
					if (mote != null)
					{
						mote.Maintain();
					}
				}
				if (this.pawn.IsHashIntervalTick(600, delta))
				{
					this.pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
				}
				if (this.roomPlayGainFactor < 0f)
				{
					this.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
				}
				if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, this.pawn, this.roomPlayGainFactor, delta, this.ToyBox))
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			}));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x001D9719 File Offset: 0x001D7919
		private Toil DropBaby()
		{
			Toil toil = ToilMaker.MakeToil("DropBaby");
			toil.initAction = delegate()
			{
				Thing thing;
				this.pawn.carryTracker.innerContainer.TryDrop(base.Baby, ThingPlaceMode.Near, 1, out thing, null, (IntVec3 c) => !this.ToyBox.OccupiedRect().Contains(c));
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0400431A RID: 17178
		private const TargetIndex ToyBoxInd = TargetIndex.B;

		// Token: 0x0400431B RID: 17179
		private const int ToysCount = 5;

		// Token: 0x0400431C RID: 17180
		private const float ToyDistanceFactor = 0.5f;

		// Token: 0x0400431D RID: 17181
		private static readonly FloatRange ToyRandomAngleOffset = new FloatRange(-5f, 5f);

		// Token: 0x0400431E RID: 17182
		private const int InteractionTicks = 600;

		// Token: 0x0400431F RID: 17183
		private Mote[] motesToMaintain;
	}
}
