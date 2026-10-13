using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011AB RID: 4523
	public class JobDriver_GotoAndStandSociallyActive : JobDriver
	{
		// Token: 0x06006E9E RID: 28318 RVA: 0x002281DB File Offset: 0x002263DB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, true, false);
		}

		// Token: 0x06006E9F RID: 28319 RVA: 0x00228208 File Offset: 0x00226408
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
				{
					this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield return this.StandToil;
			yield break;
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06006EA0 RID: 28320 RVA: 0x00228218 File Offset: 0x00226418
		public virtual Toil StandToil
		{
			get
			{
				Toil toil = ToilMaker.MakeToil("StandToil");
				toil.tickIntervalAction = delegate(int delta)
				{
					if (!this.job.forceMaintainFacing)
					{
						Pawn pawn = JobDriver_StandAndBeSociallyActive.FindClosePawn(this.pawn);
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
					}
					this.pawn.GainComfortFromCellIfPossible(delta, false);
				};
				toil.socialMode = RandomSocialMode.SuperActive;
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.handlingFacing = true;
				return toil;
			}
		}
	}
}
