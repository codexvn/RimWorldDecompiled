using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200122D RID: 4653
	public class JobDriver_CarryToBuilding : JobDriver
	{
		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060071BF RID: 29119 RVA: 0x00232760 File Offset: 0x00230960
		private Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x060071C0 RID: 29120 RVA: 0x00232788 File Offset: 0x00230988
		private Building_Enterable Building
		{
			get
			{
				return (Building_Enterable)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060071C1 RID: 29121 RVA: 0x002327AE File Offset: 0x002309AE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060071C2 RID: 29122 RVA: 0x002327D1 File Offset: 0x002309D1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.Takee != null && !this.Building.CanAcceptPawn(this.Takee));
			yield return Toils_General.Do(delegate
			{
				this.Building.SelectedPawn = this.Takee;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 60, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				this.Building.TryAcceptPawn(this.Takee);
			});
			yield break;
		}

		// Token: 0x04004EC0 RID: 20160
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x04004EC1 RID: 20161
		private const TargetIndex TakeeInd = TargetIndex.B;
	}
}
