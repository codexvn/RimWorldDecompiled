using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A8 RID: 5544
	public class WorkGiver_ClearSnowOrSand : WorkGiver_Scanner
	{
		// Token: 0x06008315 RID: 33557 RVA: 0x00275644 File Offset: 0x00273844
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.SnowOrSandClear.ActiveCells;
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06008316 RID: 33558 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008317 RID: 33559 RVA: 0x0027565B File Offset: 0x0027385B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.SnowOrSandClear.TrueCount == 0;
		}

		// Token: 0x06008318 RID: 33560 RVA: 0x00275678 File Offset: 0x00273878
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return (pawn.Map.snowGrid.GetDepth(c) >= 0.2f || c.GetSandDepth(pawn.Map) >= 0.2f) && pawn.CanReserve(c, 1, -1, null, forced);
		}

		// Token: 0x06008319 RID: 33561 RVA: 0x002756C7 File Offset: 0x002738C7
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.ClearSnow, c);
		}

		// Token: 0x0600831A RID: 33562 RVA: 0x002756D9 File Offset: 0x002738D9
		private bool IsSand(IntVec3 cell, Map map)
		{
			return cell.GetSandDepth(map) > map.snowGrid.GetDepth(cell);
		}

		// Token: 0x0600831B RID: 33563 RVA: 0x002756F0 File Offset: 0x002738F0
		public override string PostProcessedGerund(Job job)
		{
			if (this.IsSand(job.targetA.Cell, Find.CurrentMap))
			{
				return "ClearingSandGerund".Translate();
			}
			return base.PostProcessedGerund(job);
		}
	}
}
