using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003539 RID: 13625
	public struct LargeBuildingSpawnParms
	{
		// Token: 0x170031B0 RID: 12720
		// (get) Token: 0x060131DB RID: 78299 RVA: 0x005AC1DC File Offset: 0x005AA3DC
		public IntVec2 Size
		{
			get
			{
				IntVec2? intVec = this.overrideSize;
				if (intVec == null)
				{
					return this.thingDef.size;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x060131DC RID: 78300 RVA: 0x005AC20C File Offset: 0x005AA40C
		public LargeBuildingSpawnParms ForThing(ThingDef thingDef)
		{
			LargeBuildingSpawnParms result = this;
			result.thingDef = thingDef;
			return result;
		}

		// Token: 0x0400C646 RID: 50758
		public ThingDef thingDef;

		// Token: 0x0400C647 RID: 50759
		public float minDistanceToColonyBuilding;

		// Token: 0x0400C648 RID: 50760
		public float maxDistanceToColonyBuilding;

		// Token: 0x0400C649 RID: 50761
		public float minDistanceFromUsedRects;

		// Token: 0x0400C64A RID: 50762
		public float maxDistanceFromPlayerStartPosition;

		// Token: 0x0400C64B RID: 50763
		public bool canSpawnOnImpassable;

		// Token: 0x0400C64C RID: 50764
		public int minDistToEdge;

		// Token: 0x0400C64D RID: 50765
		public SpawnLocationType attemptSpawnLocationType;

		// Token: 0x0400C64E RID: 50766
		public bool attemptNotUnderBuildings;

		// Token: 0x0400C64F RID: 50767
		public bool preferFarFromColony;

		// Token: 0x0400C650 RID: 50768
		public bool ignoreTerrainAffordance;

		// Token: 0x0400C651 RID: 50769
		public bool ignoreFoundations;

		// Token: 0x0400C652 RID: 50770
		public bool allowFogged;

		// Token: 0x0400C653 RID: 50771
		public IntVec2? overrideSize;

		// Token: 0x0400C654 RID: 50772
		public static readonly LargeBuildingSpawnParms Default = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = 50f,
			canSpawnOnImpassable = true
		};
	}
}
