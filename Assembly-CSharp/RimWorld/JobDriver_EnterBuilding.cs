using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200125F RID: 4703
	public class JobDriver_EnterBuilding : JobDriver
	{
		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x0600730E RID: 29454 RVA: 0x0023695B File Offset: 0x00234B5B
		private Building_Enterable Building
		{
			get
			{
				return (Building_Enterable)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600730F RID: 29455 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007310 RID: 29456 RVA: 0x00236972 File Offset: 0x00234B72
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.Building.CanAcceptPawn(this.pawn));
			yield return Toils_General.Do(delegate
			{
				this.Building.SelectedPawn = this.pawn;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 60, true, false, false, TargetIndex.None, PathEndMode.Touch);
			yield return Toils_General.Do(delegate
			{
				this.Building.TryAcceptPawn(this.pawn);
			});
			yield break;
		}

		// Token: 0x04004F6B RID: 20331
		public const int EnterDelay = 60;
	}
}
