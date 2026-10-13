using System;

namespace Verse
{
	// Token: 0x020004B1 RID: 1201
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x06001F45 RID: 8005 RVA: 0x000A4530 File Offset: 0x000A2730
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
					if (c2.InNoBuildEdgeArea(map))
					{
						return false;
					}
					if (this.affordance != null && !c2.GetAffordances(map).Contains(this.affordance))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001AF8 RID: 6904
		public int radius = 1;

		// Token: 0x04001AF9 RID: 6905
		public TerrainAffordanceDef affordance;
	}
}
