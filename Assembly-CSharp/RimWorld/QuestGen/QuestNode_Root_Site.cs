using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B9B RID: 15259
	public class QuestNode_Root_Site : QuestNode
	{
		// Token: 0x06014B91 RID: 84881 RVA: 0x00634948 File Offset: 0x00632B48
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(slate, out tile))
			{
				Log.Error("Could not find valid site tile.");
				return;
			}
			Faction faction = (this.factionDef != null) ? Find.FactionManager.FirstFactionOfDef(this.factionDef.GetValue(slate)) : Faction.OfAncientsHostile;
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(this.sitePartDef.GetValue(slate), new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, faction, false, null, this.worldObjectDef.GetValue(slate));
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
		}

		// Token: 0x06014B92 RID: 84882 RVA: 0x00634A28 File Offset: 0x00632C28
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return this.TryFindSiteTile(slate, out planetTile);
		}

		// Token: 0x06014B93 RID: 84883 RVA: 0x00634A44 File Offset: 0x00632C44
		protected virtual bool TryGetLayer(Slate slate, out PlanetTile source, out PlanetLayer layer)
		{
			QuestNode_Root_Site.<>c__DisplayClass18_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.slate = slate;
			layer = null;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			if (map != null && map.Tile.Valid)
			{
				source = map.Tile;
			}
			else if (!TileFinder.TryFindRandomPlayerTile(out source, false, null, true, null))
			{
				source = Find.WorldGrid.Surface.Tiles.RandomElement<Tile>().tile;
			}
			if (this.<TryGetLayer>g__Validator|18_0(source, source.Layer, ref CS$<>8__locals1))
			{
				layer = source.Layer;
			}
			else
			{
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers.InRandomOrder(null))
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					if (planetLayer2 != source.Layer && this.<TryGetLayer>g__Validator|18_0(source, planetLayer2, ref CS$<>8__locals1))
					{
						layer = planetLayer2;
						break;
					}
				}
			}
			return layer != null;
		}

		// Token: 0x06014B94 RID: 84884 RVA: 0x00634B5C File Offset: 0x00632D5C
		protected virtual bool TryFindSiteTile(Slate slate, out PlanetTile tile)
		{
			PlanetTile planetTile;
			PlanetLayer planetLayer;
			if (!this.TryGetLayer(slate, out planetTile, out planetLayer))
			{
				tile = PlanetTile.Invalid;
				return false;
			}
			Hilliness hilliness = this.maxHilliness.GetValue(slate) ?? Hilliness.Mountainous;
			int trueMin = this.distanceFromColonyRange.GetValue(slate).TrueMin;
			int trueMax = this.distanceFromColonyRange.GetValue(slate).TrueMax;
			bool valueOrDefault = this.requiresLandmark.GetValue(slate).GetValueOrDefault();
			float chance = this.selectLandmarkChance.GetValue(slate) ?? 0.5f;
			FastTileFinder.LandmarkMode landmarkMode = (valueOrDefault || Rand.Chance(chance)) ? FastTileFinder.LandmarkMode.Required : FastTileFinder.LandmarkMode.Forbidden;
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(planetTile, (float)trueMin, (float)trueMax, landmarkMode, true, Hilliness.Undefined, hilliness, true, true, true);
			int num = this.desperateIgnoreDistance.GetValue(slate).GetValueOrDefault() ? 0 : this.distanceFromColonyRange.GetValue(slate).TrueMin;
			float num2 = this.desperateIgnoreDistance.GetValue(slate).GetValueOrDefault() ? float.MaxValue : ((float)this.distanceFromColonyRange.GetValue(slate).TrueMax);
			PlanetTile origin = planetTile;
			float minDistTiles = (float)num;
			float maxDistTiles = num2;
			FastTileFinder.LandmarkMode landmarkMode2 = FastTileFinder.LandmarkMode.Any;
			bool reachable = true;
			Hilliness minHilliness = Hilliness.Undefined;
			bool checkBiome = (!this.desperateIgnoreBiome.GetValue(slate)) ?? true;
			FastTileFinder.TileQueryParams desperate = new FastTileFinder.TileQueryParams(origin, minDistTiles, maxDistTiles, landmarkMode2, reachable, minHilliness, hilliness, checkBiome, true, true);
			List<PlanetTile> list = planetLayer.FastTileFinder.Query(query, this.allowedBiomes.GetValue(slate), this.allowedLandmarks.GetValue(slate), desperate);
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06014B96 RID: 84886 RVA: 0x00634D5C File Offset: 0x00632F5C
		[CompilerGenerated]
		private bool <TryGetLayer>g__Validator|18_0(PlanetTile origin, PlanetLayer layer, ref QuestNode_Root_Site.<>c__DisplayClass18_0 A_3)
		{
			if (!this.canBeSpace.GetValue(A_3.slate) && layer.Def.isSpace)
			{
				return false;
			}
			List<PlanetLayerDef> value = this.layerWhitelist.GetValue(A_3.slate);
			List<PlanetLayerDef> value2 = this.layerBlacklist.GetValue(A_3.slate);
			return (value.NullOrEmpty<PlanetLayerDef>() || value.Contains(layer.Def)) && (value2.NullOrEmpty<PlanetLayerDef>() || !value2.Contains(layer.Def)) && (!this.requireSameOrAdjacentLayer.GetValue(A_3.slate) || !origin.Valid || origin.Layer == layer || layer.DirectConnectionTo(origin.Layer));
		}

		// Token: 0x0400E685 RID: 59013
		private SlateRef<SitePartDef> sitePartDef;

		// Token: 0x0400E686 RID: 59014
		private SlateRef<WorldObjectDef> worldObjectDef;

		// Token: 0x0400E687 RID: 59015
		private SlateRef<FactionDef> factionDef;

		// Token: 0x0400E688 RID: 59016
		private SlateRef<List<BiomeDef>> allowedBiomes;

		// Token: 0x0400E689 RID: 59017
		private SlateRef<Hilliness?> maxHilliness;

		// Token: 0x0400E68A RID: 59018
		private SlateRef<IntRange> distanceFromColonyRange;

		// Token: 0x0400E68B RID: 59019
		private SlateRef<List<LandmarkDef>> allowedLandmarks;

		// Token: 0x0400E68C RID: 59020
		private SlateRef<float?> selectLandmarkChance;

		// Token: 0x0400E68D RID: 59021
		private SlateRef<bool?> requiresLandmark;

		// Token: 0x0400E68E RID: 59022
		private SlateRef<bool?> desperateIgnoreBiome;

		// Token: 0x0400E68F RID: 59023
		private SlateRef<bool?> desperateIgnoreDistance;

		// Token: 0x0400E690 RID: 59024
		public SlateRef<bool> canBeSpace;

		// Token: 0x0400E691 RID: 59025
		public SlateRef<bool> requireSameOrAdjacentLayer = true;

		// Token: 0x0400E692 RID: 59026
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x0400E693 RID: 59027
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;

		// Token: 0x0400E694 RID: 59028
		public const string SiteAlias = "site";
	}
}
