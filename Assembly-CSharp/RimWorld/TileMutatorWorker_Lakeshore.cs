using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020020FB RID: 8443
	public class TileMutatorWorker_Lakeshore : TileMutatorWorker_Coast
	{
		// Token: 0x17001C04 RID: 7172
		// (get) Token: 0x0600B854 RID: 47188 RVA: 0x00129253 File Offset: 0x00127453
		protected override float MaxForDeepWater
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17001C05 RID: 7173
		// (get) Token: 0x0600B855 RID: 47189 RVA: 0x0031CA56 File Offset: 0x0031AC56
		protected override float MaxForShallowWater
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x0600B856 RID: 47190 RVA: 0x00352976 File Offset: 0x00350B76
		protected override float MaxForSand
		{
			get
			{
				return 0.55f;
			}
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x0600B857 RID: 47191 RVA: 0x0035297D File Offset: 0x00350B7D
		protected override FloatRange CoastOffset
		{
			get
			{
				return new FloatRange(0.25f, 0.35f);
			}
		}

		// Token: 0x0600B858 RID: 47192 RVA: 0x0034E49C File Offset: 0x0034C69C
		public TileMutatorWorker_Lakeshore(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B859 RID: 47193 RVA: 0x0035298E File Offset: 0x00350B8E
		protected override TerrainDef DeepWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.DeepFreshWaterTerrainAt(cell, map);
		}

		// Token: 0x0600B85A RID: 47194 RVA: 0x00352997 File Offset: 0x00350B97
		protected override TerrainDef ShallowWaterTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.ShallowFreshWaterTerrainAt(cell, map);
		}

		// Token: 0x0600B85B RID: 47195 RVA: 0x003529A0 File Offset: 0x00350BA0
		protected override TerrainDef BeachTerrainAt(IntVec3 cell, Map map)
		{
			return MapGenUtility.LakeshoreTerrainAt(cell, map);
		}

		// Token: 0x0600B85C RID: 47196 RVA: 0x003529AC File Offset: 0x00350BAC
		protected override float GetCoastAngle(PlanetTile tile)
		{
			return Find.World.CoastAngleAt(tile, BiomeDefOf.Lake).Value;
		}
	}
}
