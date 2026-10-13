using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDB RID: 7643
	public class IncidentWorker_PoluxTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x0600AA6C RID: 43628 RVA: 0x0030BD64 File Offset: 0x00309F64
		protected override bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => x.IsPolluted(map) && this.GenStep.CanSpawnAt(x, map, 40, 0, 0f, 22, 10, 0, -1, 15), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 10, 0, 0f, 18, 20, 0, -1, 15), map, out cell);
		}
	}
}
