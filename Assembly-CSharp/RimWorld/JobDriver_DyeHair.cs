using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001251 RID: 4689
	public class JobDriver_DyeHair : JobDriver
	{
		// Token: 0x060072BC RID: 29372 RVA: 0x00235728 File Offset: 0x00233928
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (!this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			Thing thing = target.Thing;
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			Thing thing2 = this.job.GetTarget(TargetIndex.B).Thing;
			return this.pawn.Reserve(thing2, this.job, 1, 1, null, errorOnFailed, false);
		}

		// Token: 0x060072BD RID: 29373 RVA: 0x002357C7 File Offset: 0x002339C7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Hair dyeing"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_General.Wait(300, TargetIndex.A).PlaySustainerOrSound(SoundDefOf.Interact_RecolorApparel, 1f).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				Thing thing = this.job.GetTarget(TargetIndex.B).Thing.SplitOff(1);
				if (thing != null && !thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
				this.pawn.style.FinalizeHairColor();
			});
			yield break;
		}

		// Token: 0x04004F3B RID: 20283
		public const TargetIndex StylingStationInd = TargetIndex.A;

		// Token: 0x04004F3C RID: 20284
		public const TargetIndex DyeInd = TargetIndex.B;

		// Token: 0x04004F3D RID: 20285
		public const int WorkTimeTicks = 300;
	}
}
