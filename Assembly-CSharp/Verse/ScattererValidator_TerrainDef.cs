using System;

namespace Verse
{
	// Token: 0x020004B6 RID: 1206
	public class ScattererValidator_TerrainDef : ScattererValidator
	{
		// Token: 0x06001F50 RID: 8016 RVA: 0x000A49D0 File Offset: 0x000A2BD0
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.GetTerrain(map) != this.terrainDef)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001AFF RID: 6911
		public int radius = 1;

		// Token: 0x04001B00 RID: 6912
		public TerrainDef terrainDef;
	}
}
