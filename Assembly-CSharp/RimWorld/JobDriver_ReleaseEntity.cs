using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012EC RID: 4844
	public class JobDriver_ReleaseEntity : JobDriver
	{
		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x060076D0 RID: 30416 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x060076D1 RID: 30417 RVA: 0x00242B97 File Offset: 0x00240D97
		private Pawn InnerPawn
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = this.Platform as Building_HoldingPlatform;
				if (building_HoldingPlatform == null)
				{
					return null;
				}
				return building_HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x060076D2 RID: 30418 RVA: 0x00242BB0 File Offset: 0x00240DB0
		private bool EntityShouldBeReleased
		{
			get
			{
				Pawn innerPawn = this.InnerPawn;
				CompHoldingPlatformTarget compHoldingPlatformTarget = (innerPawn != null) ? innerPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
				return compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || this.job.ignoreDesignations);
			}
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x00242BEB File Offset: 0x00240DEB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x00242C0E File Offset: 0x00240E0E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(() => !this.EntityShouldBeReleased);
			Toil toil = Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 300);
			toil.PlaySustainerOrSound(SoundDefOf.ReleaseFromPlatform, 1f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Pawn pawn = base.TargetThingB as Pawn;
				if (pawn != null)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
					compHoldingPlatformTarget.Escape(false);
					if (compHoldingPlatformTarget != null)
					{
						compHoldingPlatformTarget.containmentMode = EntityContainmentMode.MaintainOnly;
					}
				}
				MentalState mentalState = this.pawn.MentalState;
				if (mentalState == null)
				{
					return;
				}
				mentalState.Notify_ReleasedTarget();
			});
			yield break;
		}

		// Token: 0x0400511F RID: 20767
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x04005120 RID: 20768
		private const TargetIndex EntityIndex = TargetIndex.B;

		// Token: 0x04005121 RID: 20769
		private const int TransferTicks = 300;
	}
}
