using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000C62 RID: 3170
	public static class CellIndicesUtility
	{
		// Token: 0x06005122 RID: 20770 RVA: 0x001A577B File Offset: 0x001A397B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CellToIndex(IntVec3 c, int sizeX)
		{
			return c.z * sizeX + c.x;
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x001A578C File Offset: 0x001A398C
		public static int CellToIndex(int x, int z, int sizeX)
		{
			return z * sizeX + x;
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x001A5794 File Offset: 0x001A3994
		public static IntVec3 IndexToCell(int ind, int sizeX)
		{
			int newX = ind % sizeX;
			int newZ = ind / sizeX;
			return new IntVec3(newX, 0, newZ);
		}
	}
}
