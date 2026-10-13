using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041F RID: 1055
	public sealed class BlueprintGrid
	{
		// Token: 0x170004C1 RID: 1217
		public List<Blueprint> this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x170004C2 RID: 1218
		public List<Blueprint> this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001AAA RID: 6826 RVA: 0x0008745C File Offset: 0x0008565C
		public List<Blueprint>[] InnerArray
		{
			get
			{
				return this.innerArray;
			}
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00087464 File Offset: 0x00085664
		public BlueprintGrid(Map map)
		{
			this.map = map;
			this.innerArray = new List<Blueprint>[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0008748C File Offset: 0x0008568C
		public void Register(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					List<Blueprint>[] array = this.innerArray;
					int num2 = num;
					if (array[num2] == null)
					{
						array[num2] = new List<Blueprint>();
					}
					this.innerArray[num].Add(ed);
				}
			}
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x00087510 File Offset: 0x00085710
		public void DeRegister(Blueprint ed)
		{
			CellIndices cellIndices = this.map.cellIndices;
			CellRect cellRect = ed.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					int num = cellIndices.CellToIndex(j, i);
					this.innerArray[num].Remove(ed);
					if (this.innerArray[num].Count == 0)
					{
						this.innerArray[num] = null;
					}
				}
			}
		}

		// Token: 0x0400178B RID: 6027
		private readonly Map map;

		// Token: 0x0400178C RID: 6028
		private readonly List<Blueprint>[] innerArray;
	}
}
