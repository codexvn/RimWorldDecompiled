using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A7 RID: 5543
	public class WorkGiver_ClearPollution : WorkGiver_Scanner
	{
		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x0600830F RID: 33551 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008310 RID: 33552 RVA: 0x002755C7 File Offset: 0x002737C7
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.PollutionClear.ActiveCells;
		}

		// Token: 0x06008311 RID: 33553 RVA: 0x002755DE File Offset: 0x002737DE
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.PollutionClear.TrueCount == 0;
		}

		// Token: 0x06008312 RID: 33554 RVA: 0x002755F8 File Offset: 0x002737F8
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return ModLister.CheckBiotech("Clear pollution") && pawn.Map.pollutionGrid.IsPolluted(c) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x06008313 RID: 33555 RVA: 0x00275632 File Offset: 0x00273832
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearPollution, c);
		}
	}
}
