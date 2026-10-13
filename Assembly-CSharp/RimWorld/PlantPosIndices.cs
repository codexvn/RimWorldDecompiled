using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B31 RID: 11057
	public static class PlantPosIndices
	{
		// Token: 0x0600F752 RID: 63314 RVA: 0x0047E008 File Offset: 0x0047C208
		static PlantPosIndices()
		{
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int k = 0; k < i; k++)
					{
						array[k] = k;
					}
					array.Shuffle<int>();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		// Token: 0x0600F753 RID: 63315 RVA: 0x0047E070 File Offset: 0x0047C270
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x0400A2BB RID: 41659
		private static int[][][] rootList = new int[25][][];

		// Token: 0x0400A2BC RID: 41660
		private const int ListCount = 8;
	}
}
