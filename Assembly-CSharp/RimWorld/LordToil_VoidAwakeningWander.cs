using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172E RID: 5934
	public class LordToil_VoidAwakeningWander : LordToil
	{
		// Token: 0x06008B97 RID: 35735 RVA: 0x002994E8 File Offset: 0x002976E8
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				this.UpdateDefendPoint(pawn);
			}
		}

		// Token: 0x06008B98 RID: 35736 RVA: 0x00299540 File Offset: 0x00297740
		public override void LordToilTick()
		{
			for (int i = this.lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsHashIntervalTick(1800))
				{
					this.UpdateDefendPoint(pawn);
				}
				if (pawn.mindState.enemyTarget != null)
				{
					this.lord.RemovePawn(pawn);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06008B99 RID: 35737 RVA: 0x002995B8 File Offset: 0x002977B8
		private void UpdateDefendPoint(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return;
			}
			IntVec3 c;
			if (!CellFinder.TryFindRandomReachableNearbyCell(pawn.Position, pawn.Map, 50f, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), null, null, out c, 999999))
			{
				return;
			}
			pawn.mindState.duty = new PawnDuty(DutyDefOf.VoidAwakeningWander, c, -1f);
			pawn.mindState.duty.wanderRadius = new float?(12f);
		}

		// Token: 0x04005AB0 RID: 23216
		private const float WanderRadius = 12f;

		// Token: 0x04005AB1 RID: 23217
		private const float NextDefendPointRadius = 50f;

		// Token: 0x04005AB2 RID: 23218
		private const int SwitchDefendPointInterval = 1800;
	}
}
