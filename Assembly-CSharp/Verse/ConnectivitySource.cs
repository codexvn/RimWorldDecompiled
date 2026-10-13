using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Verse
{
	// Token: 0x020005CF RID: 1487
	public class ConnectivitySource : SimplePathFinderDataSource<CellConnection>
	{
		// Token: 0x06002636 RID: 9782 RVA: 0x000C9A1F File Offset: 0x000C7C1F
		public ConnectivitySource(Map map) : base(map)
		{
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000C9A28 File Offset: 0x000C7C28
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			CellIndices cellIndices = this.map.cellIndices;
			for (int i = 0; i < this.cellCount; i++)
			{
				this.data[i] = CellConnection.AllNeighbours;
			}
			int x = this.map.Size.x;
			int z = this.map.Size.z;
			NativeArray<CellConnection> ptr;
			int index;
			for (int j = 0; j < z; j++)
			{
				IntVec3 c = new IntVec3(0, 0, j);
				IntVec3 c2 = new IntVec3(x - 1, 0, j);
				ptr = ref this.data;
				index = cellIndices.CellToIndex(c);
				ptr[index] ^= CellConnection.WestNeighbours;
				ptr = ref this.data;
				index = cellIndices.CellToIndex(c2);
				ptr[index] ^= CellConnection.EastNeighbours;
			}
			for (int k = 0; k < x; k++)
			{
				IntVec3 c3 = new IntVec3(k, 0, 0);
				IntVec3 c4 = new IntVec3(k, 0, z - 1);
				ptr = ref this.data;
				index = cellIndices.CellToIndex(c3);
				ptr[index] ^= CellConnection.SouthNeighbours;
				ptr = ref this.data;
				index = cellIndices.CellToIndex(c4);
				ptr[index] ^= CellConnection.NorthNeighbours;
			}
			IntVec3 c5 = new IntVec3(0, 0, 0);
			IntVec3 c6 = new IntVec3(x - 1, 0, 0);
			IntVec3 c7 = new IntVec3(0, 0, z - 1);
			IntVec3 c8 = new IntVec3(x - 1, 0, z - 1);
			ptr = ref this.data;
			index = cellIndices.CellToIndex(c5);
			ptr[index] ^= CellConnection.SouthWest;
			ptr = ref this.data;
			index = cellIndices.CellToIndex(c6);
			ptr[index] ^= CellConnection.SouthEast;
			ptr = ref this.data;
			index = cellIndices.CellToIndex(c7);
			ptr[index] ^= CellConnection.NorthWest;
			ptr = ref this.data;
			index = cellIndices.CellToIndex(c8);
			ptr[index] ^= CellConnection.NorthEast;
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000C9C54 File Offset: 0x000C7E54
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 cell in cellDeltas)
			{
				ConnectivitySource.checkedCells.Clear();
				foreach (IntVec3 intVec in CellRect.FromCell(cell).ExpandedBy(1).ClipInsideMap(this.map).Cells)
				{
					if (ConnectivitySource.checkedCells.Add(intVec))
					{
						this.data[cellIndices.CellToIndex(intVec)] = this.ComputeCellConnectivity(intVec);
					}
				}
			}
			return false;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000C9D3C File Offset: 0x000C7F3C
		private CellConnection ComputeCellConnectivity(IntVec3 cell)
		{
			CellConnection cellConnection = CellConnection.Self;
			using (CellConnectionEnumerator enumerator = CellConnection.AllNeighbours.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CellConnection cellConnection2 = enumerator.Current;
					IntVec3 c = cell + cellConnection2.Offset();
					if (c.InBounds(this.map))
					{
						Building building = this.map.edificeGrid.InnerArray[this.map.cellIndices.CellToIndex(c)];
						if (building != null && building.def.passability == Traversability.Impassable)
						{
							if (!building.def.destroyable)
							{
								continue;
							}
						}
						else if (!c.WalkableByAny(this.map))
						{
							continue;
						}
						cellConnection |= cellConnection2;
					}
				}
			}
			return cellConnection;
		}

		// Token: 0x04001FB3 RID: 8115
		private static readonly HashSet<IntVec3> checkedCells = new HashSet<IntVec3>(160000);
	}
}
