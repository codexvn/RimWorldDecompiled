using System;

namespace Verse
{
	// Token: 0x020004B7 RID: 1207
	public class ScattererValidator_Roof : ScattererValidator
	{
		// Token: 0x06001F52 RID: 8018 RVA: 0x000A4A4C File Offset: 0x000A2C4C
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
					if (map.roofGrid.RoofAt(c2) != this.roofDef)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001B01 RID: 6913
		public int radius = 1;

		// Token: 0x04001B02 RID: 6914
		public RoofDef roofDef;
	}
}
