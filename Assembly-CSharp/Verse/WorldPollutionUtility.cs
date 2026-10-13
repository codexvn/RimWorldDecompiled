using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D8 RID: 1240
	public static class WorldPollutionUtility
	{
		// Token: 0x06002009 RID: 8201 RVA: 0x000A7928 File Offset: 0x000A5B28
		public static void PolluteWorldAtTile(PlanetTile root, float pollutionAmount)
		{
			if (!root.Valid)
			{
				return;
			}
			PlanetTile planetTile = WorldPollutionUtility.FindBestTileToPollute(root);
			if (!planetTile.Valid)
			{
				return;
			}
			Tile tile = Find.WorldGrid[planetTile];
			float num = tile.pollution + pollutionAmount;
			float num2 = num - 1f;
			tile.pollution = Mathf.Clamp01(num);
			MapParent mapParent = Find.WorldObjects.MapParentAt(planetTile);
			if ((mapParent == null || !mapParent.HasMap) && tile.Layer.IsRootSurface)
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(planetTile);
				string value = vector.y.ToStringLatitude() + " / " + vector.x.ToStringLongitude();
				Messages.Message("MessageWorldTilePollutionChanged".Translate(pollutionAmount.ToStringPercent(), value), new LookTargets(planetTile), MessageTypeDefOf.NegativeEvent, false);
			}
			Map map = Current.Game.FindMap(planetTile);
			if (map != null)
			{
				PollutionUtility.PolluteMapToPercent(map, tile.pollution, 1000);
			}
			Find.World.renderer.Notify_TilePollutionChanged(planetTile);
			if (num2 > 0f)
			{
				WorldPollutionUtility.PolluteWorldAtTile(planetTile, num2);
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000A7A48 File Offset: 0x000A5C48
		public static PlanetTile FindBestTileToPollute(PlanetTile root)
		{
			WorldPollutionUtility.<>c__DisplayClass5_0 CS$<>8__locals1 = new WorldPollutionUtility.<>c__DisplayClass5_0();
			CS$<>8__locals1.root = root;
			if (!CS$<>8__locals1.root.Valid)
			{
				return PlanetTile.Invalid;
			}
			World world = Find.World;
			CS$<>8__locals1.grid = world.grid;
			if (CS$<>8__locals1.<FindBestTileToPollute>g__CanPollute|0(CS$<>8__locals1.root))
			{
				return CS$<>8__locals1.root;
			}
			WorldPollutionUtility.TmpPossiblePollutableTiles.Clear();
			CS$<>8__locals1.bestDistance = int.MaxValue;
			CS$<>8__locals1.root.Layer.Filler.FloodFill(CS$<>8__locals1.root, (PlanetTile x) => !base.<FindBestTileToPollute>g__CanPollute|0(x), delegate(PlanetTile t, int d)
			{
				WorldPollutionUtility.TmpTileNeighbors.Clear();
				CS$<>8__locals1.grid.GetTileNeighbors(t, WorldPollutionUtility.TmpTileNeighbors);
				for (int i = 0; i < WorldPollutionUtility.TmpTileNeighbors.Count; i++)
				{
					if (base.<FindBestTileToPollute>g__CanPollute|0(WorldPollutionUtility.TmpTileNeighbors[i]) && !WorldPollutionUtility.TmpPossiblePollutableTiles.Contains(WorldPollutionUtility.TmpTileNeighbors[i]))
					{
						int num = Mathf.RoundToInt(CS$<>8__locals1.grid.ApproxDistanceInTiles(CS$<>8__locals1.root, WorldPollutionUtility.TmpTileNeighbors[i]));
						if (num <= CS$<>8__locals1.bestDistance)
						{
							CS$<>8__locals1.bestDistance = num;
							WorldPollutionUtility.TmpPossiblePollutableTiles.Add(WorldPollutionUtility.TmpTileNeighbors[i]);
							List<PlanetTile> tmpPossiblePollutableTiles = WorldPollutionUtility.TmpPossiblePollutableTiles;
							Predicate<PlanetTile> match;
							if ((match = CS$<>8__locals1.<>9__6) == null)
							{
								match = (CS$<>8__locals1.<>9__6 = ((PlanetTile u) => Mathf.RoundToInt(CS$<>8__locals1.grid.ApproxDistanceInTiles(CS$<>8__locals1.root, u)) > CS$<>8__locals1.bestDistance));
							}
							tmpPossiblePollutableTiles.RemoveAll(match);
						}
					}
				}
				return false;
			}, int.MaxValue, null);
			CS$<>8__locals1.found = (from t in WorldPollutionUtility.TmpPossiblePollutableTiles
			orderby CS$<>8__locals1.grid[t].PollutionLevel(), CS$<>8__locals1.grid[t].pollution descending
			select t).FirstOrFallback(PlanetTile.Invalid);
			WorldPollutionUtility.TmpPossiblePollutableTiles.RemoveAll((PlanetTile t) => CS$<>8__locals1.grid[t].PollutionLevel() > CS$<>8__locals1.grid[CS$<>8__locals1.found].PollutionLevel() && CS$<>8__locals1.grid[t].pollution < CS$<>8__locals1.grid[CS$<>8__locals1.found].pollution);
			CS$<>8__locals1.found = WorldPollutionUtility.TmpPossiblePollutableTiles.RandomElement<PlanetTile>();
			WorldPollutionUtility.TmpPossiblePollutableTiles.Clear();
			WorldPollutionUtility.TmpTileNeighbors.Clear();
			return CS$<>8__locals1.found;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000A7B64 File Offset: 0x000A5D64
		public static float CalculateNearbyPollutionScore(PlanetTile tileId)
		{
			int maxTilesToProcess = Find.WorldGrid.TilesNumWithinTraversalDistance(4);
			float nearbyPollutionScore = 0f;
			tileId.Layer.Filler.FloodFill(tileId, (PlanetTile x) => true, delegate(PlanetTile tile, int dist)
			{
				nearbyPollutionScore += WorldPollutionUtility.NearbyPollutionOverDistanceCurve.Evaluate((float)Mathf.RoundToInt((float)dist)) * Find.WorldGrid[tile].pollution;
				return false;
			}, maxTilesToProcess, null);
			return nearbyPollutionScore;
		}

		// Token: 0x04001B6A RID: 7018
		public const int NearbyPollutionTileRadius = 4;

		// Token: 0x04001B6B RID: 7019
		public static readonly SimpleCurve NearbyPollutionOverDistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(4f, 0.5f),
				true
			}
		};

		// Token: 0x04001B6C RID: 7020
		private static readonly List<PlanetTile> TmpTileNeighbors = new List<PlanetTile>();

		// Token: 0x04001B6D RID: 7021
		private static readonly List<PlanetTile> TmpPossiblePollutableTiles = new List<PlanetTile>();
	}
}
