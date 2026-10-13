using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF0 RID: 8176
	public class GenStep_ScaleElevation : GenStep
	{
		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x0600B424 RID: 46116 RVA: 0x0033A9F7 File Offset: 0x00338BF7
		public override int SeedPart
		{
			get
			{
				return 253986;
			}
		}

		// Token: 0x0600B425 RID: 46117 RVA: 0x0033AA00 File Offset: 0x00338C00
		public override void Generate(Map map, GenStepParams parms)
		{
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				MapGenFloatGrid mapGenFloatGrid = elevation;
				IntVec3 c = intVec;
				mapGenFloatGrid[c] *= this.factor;
			}
		}

		// Token: 0x04007B94 RID: 31636
		public float factor = 1f;
	}
}
