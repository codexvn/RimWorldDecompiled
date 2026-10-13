using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B05 RID: 15109
	public abstract class QuestNode_Root_Gravcore : QuestNode
	{
		// Token: 0x06014912 RID: 84242 RVA: 0x006225E4 File Offset: 0x006207E4
		protected virtual bool TryFindSiteTile(out PlanetTile tile)
		{
			PlanetLayer planetLayer = null;
			PlanetTile origin;
			if (!TileFinder.TryFindRandomPlayerTile(out origin, false, null, true, null))
			{
				Log.Error("Failed to find a valid root tile for gravcore site.");
				tile = PlanetTile.Invalid;
				return false;
			}
			if (this.layer != null && !Find.WorldGrid.TryGetFirstAdjacentLayerOfDef(origin, this.layer, out planetLayer))
			{
				tile = PlanetTile.Invalid;
				return false;
			}
			if (planetLayer == null)
			{
				planetLayer = origin.Layer;
			}
			int trueMin = this.distanceFromColonyRange.TrueMin;
			int trueMax = this.distanceFromColonyRange.TrueMax;
			FastTileFinder.LandmarkMode landmarkMode = (this.requiresLandmark || Rand.Chance(this.selectLandmarkChance)) ? FastTileFinder.LandmarkMode.Required : FastTileFinder.LandmarkMode.Forbidden;
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(origin, (float)trueMin, (float)trueMax, landmarkMode, true, this.minHilliness, this.maxHilliness, true, true, true);
			FastTileFinder.TileQueryParams desperate = new FastTileFinder.TileQueryParams(origin, 1f, (float)(trueMax * 2), FastTileFinder.LandmarkMode.Any, true, this.minHilliness, this.maxHilliness, false, true, true);
			List<PlanetTile> list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes, this.allowedLandmarks, desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			query = new FastTileFinder.TileQueryParams(origin, (float)trueMin, (float)trueMax, landmarkMode, true, this.minHilliness, this.maxHilliness, true, true, true);
			desperate = new FastTileFinder.TileQueryParams(origin, 1f, float.MaxValue, FastTileFinder.LandmarkMode.Any, true, this.minHilliness, this.maxHilliness, false, true, true);
			list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes, this.allowedLandmarks, desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06014913 RID: 84243 RVA: 0x00622778 File Offset: 0x00620978
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return ModsConfig.OdysseyActive && this.TryFindSiteTile(out planetTile);
		}

		// Token: 0x0400E45E RID: 58462
		protected List<BiomeDef> allowedBiomes = new List<BiomeDef>();

		// Token: 0x0400E45F RID: 58463
		private Hilliness minHilliness;

		// Token: 0x0400E460 RID: 58464
		private Hilliness maxHilliness;

		// Token: 0x0400E461 RID: 58465
		protected IntRange distanceFromColonyRange = new IntRange(20, 60);

		// Token: 0x0400E462 RID: 58466
		protected List<LandmarkDef> allowedLandmarks;

		// Token: 0x0400E463 RID: 58467
		protected float selectLandmarkChance = 0.5f;

		// Token: 0x0400E464 RID: 58468
		public int requiredSubquestsGiven;

		// Token: 0x0400E465 RID: 58469
		private bool requiresLandmark;

		// Token: 0x0400E466 RID: 58470
		protected PlanetLayerDef layer;

		// Token: 0x0400E467 RID: 58471
		protected const string SiteAlias = "site";
	}
}
