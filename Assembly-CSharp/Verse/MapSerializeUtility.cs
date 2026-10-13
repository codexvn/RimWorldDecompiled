using System;

namespace Verse
{
	// Token: 0x02000526 RID: 1318
	public static class MapSerializeUtility
	{
		// Token: 0x06002257 RID: 8791 RVA: 0x000B2DA4 File Offset: 0x000B0FA4
		public static byte[] SerializeByte(Map map, Func<IntVec3, byte> shortReader)
		{
			return DataSerializeUtility.SerializeByte(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000B2DE8 File Offset: 0x000B0FE8
		public static void LoadByte(byte[] arr, Map map, Action<IntVec3, byte> shortWriter)
		{
			DataSerializeUtility.LoadByte(arr, map.info.NumCells, delegate(int idx, byte data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000B2E2C File Offset: 0x000B102C
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000B2E70 File Offset: 0x000B1070
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000B2EB4 File Offset: 0x000B10B4
		public static byte[] SerializeInt(Map map, Func<IntVec3, int> intReader)
		{
			return DataSerializeUtility.SerializeInt(map.info.NumCells, (int idx) => intReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000B2EF8 File Offset: 0x000B10F8
		public static void LoadInt(byte[] arr, Map map, Action<IntVec3, int> intWriter)
		{
			DataSerializeUtility.LoadInt(arr, map.info.NumCells, delegate(int idx, int data)
			{
				intWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x000B2F3C File Offset: 0x000B113C
		public static byte[] SerializeUint(Map map, Func<IntVec3, uint> uintReader)
		{
			return DataSerializeUtility.SerializeUint(map.info.NumCells, (int idx) => uintReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x000B2F80 File Offset: 0x000B1180
		public static void LoadUint(byte[] arr, Map map, Action<IntVec3, uint> uintWriter)
		{
			DataSerializeUtility.LoadUint(arr, map.info.NumCells, delegate(int idx, uint data)
			{
				uintWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
