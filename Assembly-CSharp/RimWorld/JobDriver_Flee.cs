using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200127D RID: 4733
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x060073C8 RID: 29640 RVA: 0x00238ACC File Offset: 0x00236CCC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x00238B0F File Offset: 0x00236D0F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.atomicWithPrevious = true;
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.initAction = delegate()
			{
				if (this.pawn.IsColonist)
				{
					MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ColonistFleeing);
				}
			};
			yield return toil;
			Toil toil2 = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			toil2.tickAction = delegate()
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.ExitMap();
				}
			};
			toil2.FailOn(() => this.pawn.Downed && !this.pawn.health.CanCrawl);
			yield return toil2;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
				{
					this.ExitMap();
				}
			};
			toil3.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil3;
			yield break;
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x00238B20 File Offset: 0x00236D20
		private void ExitMap()
		{
			this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
		}

		// Token: 0x04004FBC RID: 20412
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x04004FBD RID: 20413
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
