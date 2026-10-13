using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD8 RID: 6872
	public abstract class RoadDefGenStep
	{
		// Token: 0x060099AE RID: 39342
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);

		// Token: 0x04006946 RID: 26950
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04006947 RID: 26951
		public float antialiasingMultiplier = 1f;

		// Token: 0x04006948 RID: 26952
		public int periodicSpacing;
	}
}
