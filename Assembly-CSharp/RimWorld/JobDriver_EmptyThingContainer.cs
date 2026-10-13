using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001257 RID: 4695
	public class JobDriver_EmptyThingContainer : JobDriver
	{
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x060072DC RID: 29404 RVA: 0x00235DF4 File Offset: 0x00233FF4
		protected virtual PathEndMode ContainerPathEndMode
		{
			get
			{
				if (!this.job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell)
				{
					return PathEndMode.Touch;
				}
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060072DD RID: 29405 RVA: 0x00235E24 File Offset: 0x00234024
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, this.job.def.containerReservationLayer, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x00235E8A File Offset: 0x0023408A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, this.ContainerPathEndMode, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(delegate()
			{
				CompThingContainer compThingContainer;
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp(out compThingContainer) && compThingContainer.Empty;
			});
			yield return Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 120);
			yield return Toils_General.Do(delegate
			{
				if (base.TargetThingA.TryGetInnerInteractableThingOwner().TryDropAll(this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, true))
				{
					CompThingContainer compThingContainer = base.TargetThingA.TryGetComp<CompThingContainer>();
					if (compThingContainer == null)
					{
						return;
					}
					EffecterDef dropEffecterDef = compThingContainer.Props.dropEffecterDef;
					if (dropEffecterDef == null)
					{
						return;
					}
					dropEffecterDef.Spawn(base.TargetThingA, base.Map, 1f).Cleanup();
				}
			});
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false);
			Toil toil = Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch);
			yield return toil;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, true, true);
			yield break;
		}

		// Token: 0x04004F49 RID: 20297
		protected const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x04004F4A RID: 20298
		protected const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x04004F4B RID: 20299
		protected const TargetIndex StoreCellInd = TargetIndex.C;

		// Token: 0x04004F4C RID: 20300
		private const int OpenTicks = 120;
	}
}
