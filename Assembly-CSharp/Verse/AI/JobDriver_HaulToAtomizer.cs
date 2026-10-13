using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E5A RID: 3674
	public class JobDriver_HaulToAtomizer : JobDriver
	{
		// Token: 0x06005C8B RID: 23691 RVA: 0x001D8734 File Offset: 0x001D6934
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x001D8783 File Offset: 0x001D6983
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			base.AddEndCondition(delegate
			{
				if (base.TargetThingA.TryGetComp<CompAtomizer>().SpaceLeft > 0)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			Toil clearQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.B, null);
			yield return clearQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.B);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, true);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return toil;
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false);
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(clearQueue, TargetIndex.B, TargetIndex.None, true, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.A, TargetIndex.B, null);
			yield return Toils_Jump.Jump(clearQueue);
			yield break;
		}

		// Token: 0x040042EF RID: 17135
		private const TargetIndex AtomizerInd = TargetIndex.A;

		// Token: 0x040042F0 RID: 17136
		private const TargetIndex WastepackInd = TargetIndex.B;

		// Token: 0x040042F1 RID: 17137
		private const TargetIndex ChargerCellInd = TargetIndex.C;
	}
}
