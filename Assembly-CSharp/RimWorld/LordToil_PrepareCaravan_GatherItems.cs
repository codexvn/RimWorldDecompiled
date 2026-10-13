using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016D6 RID: 5846
	public class LordToil_PrepareCaravan_GatherItems : LordToil
	{
		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x06008A11 RID: 35345 RVA: 0x00293245 File Offset: 0x00291445
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x06008A12 RID: 35346 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008A13 RID: 35347 RVA: 0x0029336D File Offset: 0x0029156D
		public LordToil_PrepareCaravan_GatherItems(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06008A14 RID: 35348 RVA: 0x0029337C File Offset: 0x0029157C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherItems);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06008A15 RID: 35349 RVA: 0x002933FC File Offset: 0x002915FC
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (Find.TickManager.TicksGame % 120 == 0 && CaravanFormingUtility.AllItemsLoadedOntoCaravan(this.lord, base.Map))
			{
				foreach (Pawn pawn in this.lord.ownedPawns)
				{
					pawn.inventory.ClearHaulingCaravanCache();
				}
				this.lord.ReceiveMemo("AllItemsGathered");
			}
		}

		// Token: 0x04005A11 RID: 23057
		private IntVec3 meetingPoint;
	}
}
