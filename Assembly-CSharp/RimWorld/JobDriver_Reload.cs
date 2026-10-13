using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012F0 RID: 4848
	public class JobDriver_Reload : JobDriver
	{
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x060076EC RID: 30444 RVA: 0x00242EB0 File Offset: 0x002410B0
		private Thing Gear
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060076ED RID: 30445 RVA: 0x00242ED1 File Offset: 0x002410D1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060076EE RID: 30446 RVA: 0x00242EF4 File Offset: 0x002410F4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Reload.<>c__DisplayClass5_0 CS$<>8__locals1 = new JobDriver_Reload.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			JobDriver_Reload.<>c__DisplayClass5_0 CS$<>8__locals2 = CS$<>8__locals1;
			Thing gear = this.Gear;
			IReloadableComp reloadableComp = (gear != null) ? gear.TryGetComp<CompApparelReloadable>() : null;
			IReloadableComp reloadable;
			if ((reloadable = reloadableComp) == null)
			{
				Thing gear2 = this.Gear;
				reloadable = ((gear2 != null) ? gear2.TryGetComp<CompEquippableAbilityReloadable>() : null);
			}
			CS$<>8__locals2.reloadable = reloadable;
			this.FailOn(() => CS$<>8__locals1.reloadable == null);
			this.FailOn(() => ReloadableUtility.OwnerOf(CS$<>8__locals1.reloadable) != CS$<>8__locals1.<>4__this.pawn);
			this.FailOn(() => !CS$<>8__locals1.reloadable.NeedsReload(true));
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			Toil getNextIngredient = Toils_General.Label();
			yield return getNextIngredient;
			foreach (Toil toil in this.ReloadAsMuchAsPossible(CS$<>8__locals1.reloadable))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Jump.JumpIf(getNextIngredient, () => !CS$<>8__locals1.<>4__this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty<LocalTargetInfo>());
			foreach (Toil toil2 in this.ReloadAsMuchAsPossible(CS$<>8__locals1.reloadable))
			{
				yield return toil2;
			}
			enumerator = null;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				Thing carriedThing = CS$<>8__locals1.<>4__this.pawn.carryTracker.CarriedThing;
				if (carriedThing != null && !carriedThing.Destroyed)
				{
					Thing thing;
					CS$<>8__locals1.<>4__this.pawn.carryTracker.TryDropCarriedThing(CS$<>8__locals1.<>4__this.pawn.Position, ThingPlaceMode.Near, out thing, null);
				}
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;
			yield break;
			yield break;
		}

		// Token: 0x060076EF RID: 30447 RVA: 0x00242F04 File Offset: 0x00241104
		private IEnumerable<Toil> ReloadAsMuchAsPossible(IReloadableComp reloadable)
		{
			Toil done = Toils_General.Label();
			yield return Toils_Jump.JumpIf(done, () => this.pawn.carryTracker.CarriedThing == null || this.pawn.carryTracker.CarriedThing.stackCount < reloadable.MinAmmoNeeded(true));
			yield return Toils_General.Wait(reloadable.BaseReloadTicks, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil = ToilMaker.MakeToil("ReloadAsMuchAsPossible");
			toil.initAction = delegate()
			{
				Thing carriedThing = this.pawn.carryTracker.CarriedThing;
				reloadable.ReloadFrom(carriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield return done;
			yield break;
		}

		// Token: 0x0400512A RID: 20778
		private const TargetIndex GearInd = TargetIndex.A;

		// Token: 0x0400512B RID: 20779
		private const TargetIndex AmmoInd = TargetIndex.B;
	}
}
