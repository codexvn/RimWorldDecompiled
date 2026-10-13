using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000C61 RID: 3169
	public readonly struct CellIndices
	{
		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x001A56BF File Offset: 0x001A38BF
		public int NumGridCells
		{
			get
			{
				return this.sizeX * this.sizeZ;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06005117 RID: 20759 RVA: 0x001A56CE File Offset: 0x001A38CE
		public int SizeX
		{
			get
			{
				return this.sizeX;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06005118 RID: 20760 RVA: 0x001A56D6 File Offset: 0x001A38D6
		public int SizeZ
		{
			get
			{
				return this.sizeZ;
			}
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001A56DE File Offset: 0x001A38DE
		public CellIndices(Map map)
		{
			this.sizeX = map.Size.x;
			this.sizeZ = map.Size.z;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001A5702 File Offset: 0x001A3902
		public CellIndices(int sizeX, int sizeZ)
		{
			this.sizeX = sizeX;
			this.sizeZ = sizeZ;
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001A5712 File Offset: 0x001A3912
		public bool Contains(IntVec3 c)
		{
			return this.Contains(CellIndicesUtility.CellToIndex(c, this.sizeX));
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001A5726 File Offset: 0x001A3926
		public bool Contains(int ind)
		{
			return ind >= 0 && ind < this.sizeX * this.sizeZ;
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x001A573E File Offset: 0x001A393E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.sizeX);
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x001A574C File Offset: 0x001A394C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(int x, int z)
		{
			return CellIndicesUtility.CellToIndex(x, z, this.sizeX);
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x001A575B File Offset: 0x001A395B
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.sizeX);
		}

		// Token: 0x17000C4D RID: 3149
		public int this[IntVec3 cell]
		{
			get
			{
				return this.CellToIndex(cell);
			}
		}

		// Token: 0x17000C4E RID: 3150
		public IntVec3 this[int index]
		{
			get
			{
				return this.IndexToCell(index);
			}
		}

		// Token: 0x04003A3A RID: 14906
		private readonly int sizeX;

		// Token: 0x04003A3B RID: 14907
		private readonly int sizeZ;
	}
}
