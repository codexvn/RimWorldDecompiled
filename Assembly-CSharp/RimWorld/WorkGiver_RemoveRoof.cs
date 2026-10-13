using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001570 RID: 5488
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06008235 RID: 33333 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008236 RID: 33334 RVA: 0x0027288F File Offset: 0x00270A8F
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		// Token: 0x06008237 RID: 33335 RVA: 0x002728A6 File Offset: 0x00270AA6
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof.TrueCount == 0;
		}

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06008238 RID: 33336 RVA: 0x00075162 File Offset: 0x00073362
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06008239 RID: 33337 RVA: 0x002728C0 File Offset: 0x00270AC0
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof[c] && c.Roofed(pawn.Map) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced);
		}

		// Token: 0x0600823A RID: 33338 RVA: 0x00272910 File Offset: 0x00270B10
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveRoof, c, c);
		}

		// Token: 0x0600823B RID: 33339 RVA: 0x00272928 File Offset: 0x00270B28
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = cell + GenAdj.AdjacentCells[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						return -60f;
					}
					if (c.Roofed(pawn.Map))
					{
						num++;
					}
				}
			}
			return (float)(-(float)Mathf.Min(num, 3));
		}
	}
}
