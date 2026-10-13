using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001577 RID: 5495
	public abstract class JobDriver_CreateAndEnterDryadHolder : JobDriver
	{
		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06008250 RID: 33360 RVA: 0x00272D86 File Offset: 0x00270F86
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06008251 RID: 33361 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06008252 RID: 33362 RVA: 0x00272D9D File Offset: 0x00270F9D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.TreeComp.ShouldReturnToTree(this.pawn));
			yield return Toils_General.Do(delegate
			{
				IntVec3 c2;
				if (!CellFinder.TryFindRandomCellNear(this.job.GetTarget(TargetIndex.A).Cell, this.pawn.Map, 4, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, this.pawn.Map, null), out c2, -1))
				{
					Log.Error("Could not find cell to place dryad holder. Dryad=" + this.pawn.GetUniqueLoadID());
				}
				this.job.targetB = c2;
			});
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			yield return Toils_General.Wait(200, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B);
			yield return this.EnterToil();
			yield break;
		}

		// Token: 0x06008253 RID: 33363
		public abstract Toil EnterToil();

		// Token: 0x0400568D RID: 22157
		private const int TicksToCreate = 200;
	}
}
