using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB2 RID: 3250
	public static class DijkstraUtility
	{
		// Token: 0x06005346 RID: 21318 RVA: 0x001AD430 File Offset: 0x001AB630
		public static IEnumerable<IntVec3> AdjacentCellsNeighborsGetter(IntVec3 cell, Map map)
		{
			DijkstraUtility.adjacentCells.Clear();
			IntVec3[] array = GenAdj.AdjacentCells;
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 intVec = cell + array[i];
				if (intVec.InBounds(map))
				{
					DijkstraUtility.adjacentCells.Add(intVec);
				}
			}
			return DijkstraUtility.adjacentCells;
		}

		// Token: 0x04003C21 RID: 15393
		private static List<IntVec3> adjacentCells = new List<IntVec3>();
	}
}
