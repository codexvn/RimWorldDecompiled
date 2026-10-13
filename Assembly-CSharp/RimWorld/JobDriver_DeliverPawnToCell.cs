using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011A3 RID: 4515
	public class JobDriver_DeliverPawnToCell : JobDriver
	{
		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06006E6A RID: 28266 RVA: 0x00227608 File Offset: 0x00225808
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x0022762E File Offset: 0x0022582E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			base.Map.reservationManager.ReleaseAllForTarget(this.Takee);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006E6C RID: 28268 RVA: 0x00227667 File Offset: 0x00225867
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => !this.pawn.CanReach(this.job.GetTarget(TargetIndex.B), PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return toil;
			Toil startCarrying = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_Jump.JumpIf(gotoCell, () => this.pawn.IsCarryingPawn(this.Takee));
			yield return startCarrying;
			yield return gotoCell;
			yield return Toils_General.Do(delegate
			{
				if (!this.job.ritualTag.NullOrEmpty())
				{
					Lord lord = this.Takee.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.Takee, this.job.ritualTag);
					}
					Lord lord2 = this.pawn.GetLord();
					lordJob_Ritual = (((lord2 != null) ? lord2.LordJob : null) as LordJob_Ritual);
					if (lordJob_Ritual != null)
					{
						lordJob_Ritual.AddTagForPawn(this.pawn, this.job.ritualTag);
					}
				}
			});
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield break;
		}

		// Token: 0x04004D1A RID: 19738
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x04004D1B RID: 19739
		private const TargetIndex TargetCellIndex = TargetIndex.B;
	}
}
