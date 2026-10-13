using System;

namespace Verse
{
	// Token: 0x020004B2 RID: 1202
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x06001F47 RID: 8007 RVA: 0x000A45C4 File Offset: 0x000A27C4
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (new IntVec3(j, 0, i).GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001AFA RID: 6906
		public int radius = 1;
	}
}
