using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E4A RID: 3658
	public class JobDriver_FollowRoper : JobDriver
	{
		// Token: 0x06005C25 RID: 23589 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x001D7802 File Offset: 0x001D5A02
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
				if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				IntVec3 intVec = IntVec3.Invalid;
				if (pawn.jobs.curDriver is JobDriver_RopeToDestination)
				{
					intVec = pawn.CurJob.GetTarget(TargetIndex.B).Cell;
				}
				if (intVec.IsValid && pawn.Position == intVec)
				{
					this.GotoStandCell(intVec);
					return;
				}
				this.FollowRoper(pawn);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x001D7814 File Offset: 0x001D5A14
		private void FollowRoper(Pawn roper)
		{
			LocalTargetInfo localTargetInfo = new LocalTargetInfo(roper);
			PathEndMode peMode = PathEndMode.Touch;
			if (roper.Position.InHorDistOf(this.pawn.Position, 7.2f) && roper.pather.Moving && roper.pather.curPath != null)
			{
				int marchingPos = Mathf.Abs(roper.roping.Ropees.IndexOf(this.pawn));
				IntVec3 c = this.MarchingOrder(roper, marchingPos);
				if (c.IsValid)
				{
					localTargetInfo = c;
					peMode = PathEndMode.OnCell;
				}
			}
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != localTargetInfo)
			{
				this.pawn.pather.StartPath(localTargetInfo, peMode);
			}
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x001D78E0 File Offset: 0x001D5AE0
		private IntVec3 MarchingOrder(Pawn roper, int marchingPos)
		{
			PawnPath curPath = roper.pather.curPath;
			if (curPath.NodesLeftCount <= 0)
			{
				return IntVec3.Invalid;
			}
			Map map = this.pawn.Map;
			int num = -2 - marchingPos % 4;
			num = Mathf.Clamp(num, -curPath.NodesConsumedCount, curPath.NodesLeftCount);
			IntVec3 intVec = curPath.Peek(num);
			int num2 = Mathf.Abs(this.pawn.HashOffset()) % 3;
			if (num + 1 < curPath.NodesLeftCount && num2 != 0)
			{
				IntVec3 orig = curPath.Peek(num + 1) - intVec;
				IntVec3 intVec2 = (num2 == 1) ? (intVec + orig.RotatedBy(Rot4.East)) : (intVec + orig.RotatedBy(Rot4.West));
				if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2.GetDistrict(map, RegionType.Set_Passable) == intVec.GetDistrict(map, RegionType.Set_Passable))
				{
					intVec = intVec2;
				}
			}
			return intVec;
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x001D79C8 File Offset: 0x001D5BC8
		private void GotoStandCell(IntVec3 standCell)
		{
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != standCell)
			{
				this.pawn.pather.StartPath(standCell, PathEndMode.OnCell);
			}
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x001D7A1B File Offset: 0x001D5C1B
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040042C5 RID: 17093
		private const TargetIndex RoperInd = TargetIndex.A;
	}
}
