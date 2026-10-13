using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001159 RID: 4441
	public class JobDriver_FertilizeOvum : JobDriver
	{
		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06006CAC RID: 27820 RVA: 0x002223C7 File Offset: 0x002205C7
		private HumanOvum ovum
		{
			get
			{
				return (HumanOvum)base.TargetThingA;
			}
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x002223D4 File Offset: 0x002205D4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x002223FC File Offset: 0x002205FC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModsConfig.BiotechActive)
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedOrNull(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, true, true, false);
			Toil toil = Toils_General.Wait(180, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(100, delta))
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Heart, 0.42f);
				}
			};
			yield return toil;
			yield return Toils_General.DoAtomic(delegate
			{
				Thing thing = this.ovum.ProduceEmbryo(this.pawn);
				if (thing != null)
				{
					GenPlace.TryPlaceThing(thing, this.pawn.PositionHeld, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
					this.pawn.carryTracker.DestroyCarriedThing();
					return;
				}
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageFerilizeFailed".Translate(this.pawn.Named("PAWN")) + ": " + "CombinedGenesExceedMetabolismLimits".Translate(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			});
			yield break;
		}

		// Token: 0x04004C4F RID: 19535
		private const int FertilizeTicks = 180;

		// Token: 0x04004C50 RID: 19536
		public const TargetIndex OvumIndex = TargetIndex.A;
	}
}
