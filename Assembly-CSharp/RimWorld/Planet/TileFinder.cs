using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CB9 RID: 15545
	public static class TileFinder
	{
		// Token: 0x0601522C RID: 86572 RVA: 0x006534E7 File Offset: 0x006516E7
		public static PlanetTile RandomStartingTile()
		{
			return TileFinder.RandomSettlementTileFor(Find.WorldGrid.Surface, Faction.OfPlayer, true, null);
		}

		// Token: 0x0601522D RID: 86573 RVA: 0x006534FF File Offset: 0x006516FF
		public static PlanetTile RandomSettlementTileFor(Faction faction, bool mustBeAutoChoosable = false, Predicate<PlanetTile> extraValidator = null)
		{
			return TileFinder.RandomSettlementTileFor(Find.WorldGrid.Surface, faction, mustBeAutoChoosable, extraValidator);
		}

		// Token: 0x0601522E RID: 86574 RVA: 0x00653514 File Offset: 0x00651714
		public static PlanetTile RandomSettlementTileFor(PlanetLayer layer, Faction faction, bool mustBeAutoChoosable = false, Predicate<PlanetTile> extraValidator = null)
		{
			Func<int, int> <>9__0;
			Func<int, float> <>9__1;
			for (int i = 0; i < 500; i++)
			{
				IEnumerable<int> source = Enumerable.Range(0, 100);
				Func<int, int> selector;
				if ((selector = <>9__0) == null)
				{
					selector = (<>9__0 = ((int _) => Rand.Range(0, layer.TilesCount)));
				}
				IEnumerable<int> source2 = source.Select(selector);
				Func<int, float> weightSelector;
				if ((weightSelector = <>9__1) == null)
				{
					weightSelector = (<>9__1 = delegate(int x)
					{
						Tile tile2 = layer[x];
						if (!tile2.PrimaryBiome.canBuildBase || !tile2.PrimaryBiome.implemented || tile2.hilliness == Hilliness.Impassable)
						{
							return 0f;
						}
						if (mustBeAutoChoosable && !tile2.PrimaryBiome.canAutoChoose)
						{
							return 0f;
						}
						if (extraValidator != null && !extraValidator(tile2.tile))
						{
							return 0f;
						}
						float num = tile2.PrimaryBiome.settlementSelectionWeight;
						Faction faction2 = faction;
						if (((faction2 != null) ? faction2.def.minSettlementTemperatureChanceCurve : null) != null)
						{
							num *= faction.def.minSettlementTemperatureChanceCurve.Evaluate(GenTemperature.MinTemperatureAtTile(tile2.tile));
						}
						return num;
					});
				}
				int tileID;
				if (source2.TryRandomElementByWeight(weightSelector, out tileID))
				{
					PlanetTile tile = layer[tileID].tile;
					if (TileFinder.IsValidTileForNewSettlement(tile, null, false))
					{
						return tile;
					}
				}
			}
			Log.Error(string.Format("Failed to find faction base tile for {0}", faction));
			return new PlanetTile(0, layer);
		}

		// Token: 0x0601522F RID: 86575 RVA: 0x006535F0 File Offset: 0x006517F0
		public static bool IsValidTileForNewSettlement(PlanetTile tile, StringBuilder reason = null, bool forGravship = false)
		{
			if (!tile.Valid)
			{
				return false;
			}
			Tile tile2 = Find.WorldGrid[tile];
			if (!tile2.PrimaryBiome.canBuildBase)
			{
				if (reason != null)
				{
					reason.Append("CannotLandBiome".Translate(tile2.PrimaryBiome.LabelCap));
				}
				return false;
			}
			if (!tile2.PrimaryBiome.implemented)
			{
				if (reason != null)
				{
					reason.Append("BiomeNotImplemented".Translate() + ": " + tile2.PrimaryBiome.LabelCap);
				}
				return false;
			}
			if (tile2.hilliness == Hilliness.Impassable)
			{
				if (reason != null)
				{
					reason.Append("CannotLandImpassableMountains".Translate());
				}
				return false;
			}
			Settlement settlement = Find.WorldObjects.SettlementBaseAt(tile);
			if (settlement != null && (!forGravship || !settlement.GravShipCanLandOn))
			{
				if (reason != null)
				{
					if (settlement.Faction == null)
					{
						reason.Append("TileOccupied".Translate());
					}
					else if (settlement.Faction == Faction.OfPlayer)
					{
						reason.Append("YourBaseAlreadyThere".Translate());
					}
					else
					{
						reason.Append("BaseAlreadyThere".Translate(settlement.Faction.Name));
					}
				}
				return false;
			}
			WorldObject worldObject;
			if (Find.WorldObjects.AnySettlementBaseAtOrAdjacent(tile, out worldObject) && !forGravship)
			{
				if (reason != null)
				{
					reason.Append("FactionBaseAdjacent".Translate());
				}
				return false;
			}
			if (Find.WorldObjects.AnyWorldObjectAt<PeaceTalks>(tile))
			{
				if (reason != null)
				{
					reason.Append("TileOccupied".Translate());
				}
				return false;
			}
			if (forGravship)
			{
				Site site = Find.WorldObjects.SiteAt(tile);
				if (site != null && site.preventGravshipLanding)
				{
					if (reason != null)
					{
						reason.Append("TileOccupied".Translate());
					}
					return false;
				}
			}
			if (Find.WorldObjects.AnyWorldObjectAt<Camp>(tile))
			{
				return true;
			}
			if (!SettleInEmptyTileUtility.CanCreateMapAt(tile, forGravship))
			{
				if (forGravship)
				{
					MapParent mapParent = Find.WorldObjects.MapParentAt(tile);
					if (mapParent != null && mapParent.GravShipCanLandOn)
					{
						return true;
					}
				}
				if (reason != null)
				{
					reason.Append("TileOccupied".Translate());
				}
				return false;
			}
			return true;
		}

		// Token: 0x06015230 RID: 86576 RVA: 0x0065381C File Offset: 0x00651A1C
		public static bool TryFindPassableTileWithTraversalDistance(PlanetTile rootTile, int minDist, int maxDist, out PlanetTile result, Predicate<PlanetTile> validator = null, bool ignoreFirstTilePassability = false, TileFinderMode tileFinderMode = TileFinderMode.Random, bool canTraverseImpassable = false, bool exitOnFirstTileFound = false)
		{
			TileFinder.tmpTiles.Clear();
			rootTile.Layer.Filler.FloodFill(rootTile, (PlanetTile x) => canTraverseImpassable || !Find.World.Impassable(x) || (x == rootTile & ignoreFirstTilePassability), delegate(PlanetTile tile, int traversalDistance)
			{
				if (traversalDistance > maxDist)
				{
					return false;
				}
				if (traversalDistance >= minDist && !Find.World.Impassable(tile) && (validator == null || validator(tile)))
				{
					TileFinder.tmpTiles.Add(new ValueTuple<PlanetTile, int>(tile, traversalDistance));
					if (exitOnFirstTileFound)
					{
						return true;
					}
				}
				return false;
			}, int.MaxValue, null);
			if (exitOnFirstTileFound && TileFinder.tmpTiles.Count > 0)
			{
				result = TileFinder.tmpTiles[0].Item1;
				return true;
			}
			switch (tileFinderMode)
			{
			case TileFinderMode.Random:
			{
				ValueTuple<PlanetTile, int> valueTuple;
				if (TileFinder.tmpTiles.TryRandomElement(out valueTuple))
				{
					result = valueTuple.Item1;
					return true;
				}
				result = PlanetTile.Invalid;
				return false;
			}
			case TileFinderMode.Near:
			{
				ValueTuple<PlanetTile, int> valueTuple;
				if (TileFinder.tmpTiles.TryRandomElementByWeight(([TupleElementNames(new string[]
				{
					"tile",
					"traversalDistance"
				})] ValueTuple<PlanetTile, int> x) => 1f - (float)(x.Item2 - minDist) / ((float)(maxDist - minDist) + 0.01f), out valueTuple))
				{
					result = valueTuple.Item1;
					return true;
				}
				result = PlanetTile.Invalid;
				return false;
			}
			case TileFinderMode.Furthest:
				if (TileFinder.tmpTiles.Count > 0)
				{
					int maxDistanceWithOffset = Mathf.Clamp(TileFinder.tmpTiles.MaxBy(([TupleElementNames(new string[]
					{
						"tile",
						"traversalDistance"
					})] ValueTuple<PlanetTile, int> t) => t.Item2).Item2 - 2, minDist, maxDist);
					ValueTuple<PlanetTile, int> valueTuple2;
					if ((from t in TileFinder.tmpTiles
					where t.Item2 >= maxDistanceWithOffset - 1
					select t).TryRandomElement(out valueTuple2))
					{
						result = valueTuple2.Item1;
						return true;
					}
				}
				result = PlanetTile.Invalid;
				return false;
			default:
				Log.Error(string.Format("Unknown tile distance preference {0}.", tileFinderMode));
				result = PlanetTile.Invalid;
				return false;
			}
		}

		// Token: 0x06015231 RID: 86577 RVA: 0x00653A00 File Offset: 0x00651C00
		public static bool TryFindTileWithDistance(PlanetTile rootTile, int minDist, int maxDist, out PlanetTile result, Predicate<PlanetTile> validator = null, TileFinderMode tileFinderMode = TileFinderMode.Random, bool exitOnFirstTileFound = false)
		{
			TileFinder.tmpTiles.Clear();
			rootTile.Layer.Filler.FloodFill(rootTile, (PlanetTile _) => true, delegate(PlanetTile tile, int traversalDistance)
			{
				if (traversalDistance > maxDist)
				{
					return false;
				}
				if (traversalDistance >= minDist && (validator == null || validator(tile)))
				{
					TileFinder.tmpTiles.Add(new ValueTuple<PlanetTile, int>(tile, traversalDistance));
					if (exitOnFirstTileFound)
					{
						return true;
					}
				}
				return false;
			}, int.MaxValue, null);
			if (exitOnFirstTileFound)
			{
				if (TileFinder.tmpTiles.Count > 0)
				{
					result = TileFinder.tmpTiles[0].Item1;
					return true;
				}
				result = PlanetTile.Invalid;
				return false;
			}
			else
			{
				switch (tileFinderMode)
				{
				case TileFinderMode.Random:
				{
					ValueTuple<PlanetTile, int> valueTuple;
					if (TileFinder.tmpTiles.TryRandomElement(out valueTuple))
					{
						result = valueTuple.Item1;
						return true;
					}
					result = PlanetTile.Invalid;
					return false;
				}
				case TileFinderMode.Near:
				{
					ValueTuple<PlanetTile, int> valueTuple;
					if (TileFinder.tmpTiles.TryRandomElementByWeight(([TupleElementNames(new string[]
					{
						"tile",
						"traversalDistance"
					})] ValueTuple<PlanetTile, int> x) => 1f - (float)(x.Item2 - minDist) / ((float)(maxDist - minDist) + 0.01f), out valueTuple))
					{
						result = valueTuple.Item1;
						return true;
					}
					result = PlanetTile.Invalid;
					return false;
				}
				case TileFinderMode.Furthest:
					if (TileFinder.tmpTiles.Count > 0)
					{
						int maxDistanceWithOffset = Mathf.Clamp(TileFinder.tmpTiles.MaxBy(([TupleElementNames(new string[]
						{
							"tile",
							"traversalDistance"
						})] ValueTuple<PlanetTile, int> t) => t.Item2).Item2 - 2, minDist, maxDist);
						ValueTuple<PlanetTile, int> valueTuple2;
						if ((from t in TileFinder.tmpTiles
						where t.Item2 >= maxDistanceWithOffset - 1
						select t).TryRandomElement(out valueTuple2))
						{
							result = valueTuple2.Item1;
							return true;
						}
					}
					result = PlanetTile.Invalid;
					return false;
				default:
					Log.Error(string.Format("Unknown tile distance preference {0}.", tileFinderMode));
					result = PlanetTile.Invalid;
					return false;
				}
			}
		}

		// Token: 0x06015232 RID: 86578 RVA: 0x00653BE4 File Offset: 0x00651DE4
		public static bool TryFindRandomPlayerTile(out PlanetTile tile, bool allowCaravans, Predicate<PlanetTile> validator = null, bool canBeSpace = false, PlanetLayer layer = null)
		{
			TileFinder.tmpPlayerTiles.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].mapPawns.FreeColonistsSpawnedCount != 0 && (maps[i].Tile.LayerDef.SurfaceTiles | canBeSpace) && (layer == null || maps[i].Tile.Layer == layer) && (validator == null || validator(maps[i].Tile)))
				{
					TileFinder.tmpPlayerTiles.Add(maps[i].Tile);
				}
			}
			if (allowCaravans)
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled && (maps[j].Tile.LayerDef.SurfaceTiles | canBeSpace) && (validator == null || validator(caravans[j].Tile)) && (layer == null || caravans[j].Tile.Layer == layer))
					{
						TileFinder.tmpPlayerTiles.Add(caravans[j].Tile);
					}
				}
			}
			if (TileFinder.tmpPlayerTiles.TryRandomElement(out tile))
			{
				return true;
			}
			Map map;
			if ((from x in Find.Maps
			where x.IsPlayerHome && (x.Tile.LayerDef.SurfaceTiles | canBeSpace) && (validator == null || validator(x.Tile)) && (layer == null || x.Tile.Layer == layer)
			select x).TryRandomElement(out map))
			{
				tile = map.Tile;
				return true;
			}
			Map map2;
			if ((from x in Find.Maps
			where x.mapPawns.FreeColonistsSpawnedCount != 0 && (x.Tile.LayerDef.SurfaceTiles | canBeSpace) && (validator == null || validator(x.Tile)) && (layer == null || x.Tile.Layer == layer)
			select x).TryRandomElement(out map2))
			{
				tile = map2.Tile;
				return true;
			}
			Caravan caravan;
			if (!allowCaravans && (from x in Find.WorldObjects.Caravans
			where x.IsPlayerControlled && (x.Tile.LayerDef.SurfaceTiles | canBeSpace) && (validator == null || validator(x.Tile)) && (layer == null || x.Tile.Layer == layer)
			select x).TryRandomElement(out caravan))
			{
				tile = caravan.Tile;
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06015233 RID: 86579 RVA: 0x00653E58 File Offset: 0x00652058
		public static bool TryFindNewSiteTile(out PlanetTile tile, int minDist = 7, int maxDist = 27, bool allowCaravans = false, List<LandmarkDef> allowedLandmarks = null, float selectLandmarkChance = 0.5f, bool canSelectComboLandmarks = true, TileFinderMode tileFinderMode = TileFinderMode.Near, bool exitOnFirstTileFound = false, bool canBeSpace = false, PlanetLayer layer = null, Predicate<PlanetTile> validator = null)
		{
			return TileFinder.TryFindNewSiteTile(out tile, PlanetTile.Invalid, minDist, maxDist, allowCaravans, allowedLandmarks, selectLandmarkChance, canSelectComboLandmarks, tileFinderMode, exitOnFirstTileFound, canBeSpace, layer, validator);
		}

		// Token: 0x06015234 RID: 86580 RVA: 0x00653E84 File Offset: 0x00652084
		public static bool TryFindNewSiteTile(out PlanetTile tile, PlanetTile nearTile, int minDist = 7, int maxDist = 27, bool allowCaravans = false, List<LandmarkDef> allowedLandmarks = null, float selectLandmarkChance = 0.5f, bool canSelectComboLandmarks = true, TileFinderMode tileFinderMode = TileFinderMode.Near, bool exitOnFirstTileFound = false, bool canBeSpace = false, PlanetLayer layer = null, Predicate<PlanetTile> validator = null)
		{
			bool flag = ModsConfig.OdysseyActive && Rand.ChanceSeeded(selectLandmarkChance, Gen.HashCombineInt(Find.TickManager.TicksGame, 18271));
			if (!nearTile.Valid && !TileFinder.TryFindRandomPlayerTile(out nearTile, allowCaravans, null, true, null))
			{
				tile = PlanetTile.Invalid;
				return false;
			}
			if (layer == null)
			{
				layer = nearTile.Layer;
			}
			if (!canBeSpace && layer.Def.isSpace && !Find.WorldGrid.TryGetFirstAdjacentLayerOfDef(nearTile, PlanetLayerDefOf.Surface, out layer))
			{
				int num;
				PlanetLayer planetLayer;
				(from t in Find.WorldGrid.PlanetLayers
				where !t.Value.Def.isSpace
				select t).RandomElement<KeyValuePair<int, PlanetLayer>>().Deconstruct(out num, out planetLayer);
				layer = planetLayer;
			}
			FastTileFinder.LandmarkMode landmarkMode = flag ? FastTileFinder.LandmarkMode.Required : FastTileFinder.LandmarkMode.Any;
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(nearTile, (float)minDist, (float)maxDist, landmarkMode, true, Hilliness.Undefined, Hilliness.Undefined, true, true, canSelectComboLandmarks);
			List<PlanetTile> list = layer.FastTileFinder.Query(query, null, allowedLandmarks, default(FastTileFinder.TileQueryParams));
			if (validator != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (!validator(list[i]))
					{
						list.RemoveAt(i);
					}
				}
			}
			if (!list.Empty<PlanetTile>())
			{
				tile = list.RandomElement<PlanetTile>();
				return true;
			}
			if (TileFinder.TryFillFindTile(layer.GetClosestTile(nearTile), out tile, minDist, maxDist, allowedLandmarks, canSelectComboLandmarks, tileFinderMode, exitOnFirstTileFound, validator, flag))
			{
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06015235 RID: 86581 RVA: 0x00654000 File Offset: 0x00652200
		private static bool TryFillFindTile(PlanetTile root, out PlanetTile tile, int minDist = 7, int maxDist = 27, List<LandmarkDef> allowedLandmarks = null, bool canSelectComboLandmarks = true, TileFinderMode tileFinderMode = TileFinderMode.Near, bool exitOnFirstTileFound = false, Predicate<PlanetTile> validator = null, bool pickLandmark = false)
		{
			TileFinder.<>c__DisplayClass11_0 CS$<>8__locals1 = new TileFinder.<>c__DisplayClass11_0();
			CS$<>8__locals1.validator = validator;
			CS$<>8__locals1.pickLandmark = pickLandmark;
			CS$<>8__locals1.allowedLandmarks = allowedLandmarks;
			CS$<>8__locals1.canSelectComboLandmarks = canSelectComboLandmarks;
			if (TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist, maxDist, out tile, (PlanetTile c) => base.<TryFillFindTile>g__ValidTile|3(c, CS$<>8__locals1.pickLandmark), false, tileFinderMode, false, exitOnFirstTileFound))
			{
				return true;
			}
			if (CS$<>8__locals1.pickLandmark && TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist, maxDist, out tile, (PlanetTile c) => base.<TryFillFindTile>g__ValidTile|3(c, false), false, tileFinderMode, false, exitOnFirstTileFound))
			{
				return true;
			}
			if (TileFinder.TryFindPassableTileWithTraversalDistance(root, minDist, maxDist, out tile, new Predicate<PlanetTile>(CS$<>8__locals1.<TryFillFindTile>g__BackupValidTile|0), false, tileFinderMode, true, exitOnFirstTileFound))
			{
				return true;
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x0400EBCE RID: 60366
		[TupleElementNames(new string[]
		{
			"tile",
			"traversalDistance"
		})]
		private static readonly List<ValueTuple<PlanetTile, int>> tmpTiles = new List<ValueTuple<PlanetTile, int>>();

		// Token: 0x0400EBCF RID: 60367
		private static readonly List<PlanetTile> tmpPlayerTiles = new List<PlanetTile>();
	}
}
