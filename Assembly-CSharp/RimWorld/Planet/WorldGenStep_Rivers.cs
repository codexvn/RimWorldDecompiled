using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CFA RID: 15610
	public class WorldGenStep_Rivers : WorldGenStep
	{
		// Token: 0x170034F0 RID: 13552
		// (get) Token: 0x060153D2 RID: 86994 RVA: 0x0065A86C File Offset: 0x00658A6C
		public override int SeedPart
		{
			get
			{
				return 605014749;
			}
		}

		// Token: 0x060153D3 RID: 86995 RVA: 0x0065A873 File Offset: 0x00658A73
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateRivers(layer);
		}

		// Token: 0x060153D4 RID: 86996 RVA: 0x0065A87C File Offset: 0x00658A7C
		private void GenerateRivers(PlanetLayer layer)
		{
			Find.WorldPathGrid.RecalculateLayerPerceivedPathCosts(layer);
			List<PlanetTile> coastalWaterTiles = this.GetCoastalWaterTiles(layer);
			if (!coastalWaterTiles.Any<PlanetTile>())
			{
				return;
			}
			List<PlanetTile> neighbors = new List<PlanetTile>();
			List<PlanetTile>[] array = layer.Pather.FloodPathsWithCostForTree(coastalWaterTiles, delegate(PlanetTile st, PlanetTile ed)
			{
				Tile tile = Find.WorldGrid[ed];
				Tile tile2 = Find.WorldGrid[st];
				Find.WorldGrid.GetTileNeighbors(ed, neighbors);
				PlanetTile planetTile = neighbors[0];
				for (int j = 0; j < neighbors.Count; j++)
				{
					if (WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[neighbors[j]]) < WorldGenStep_Rivers.GetImpliedElevation(Find.WorldGrid[planetTile]))
					{
						planetTile = neighbors[j];
					}
				}
				float num = 1f;
				if (planetTile != st)
				{
					num = 2f;
				}
				return Mathf.RoundToInt(num * WorldGenStep_Rivers.ElevationChangeCost.Evaluate(WorldGenStep_Rivers.GetImpliedElevation(tile2) - WorldGenStep_Rivers.GetImpliedElevation(tile)));
			}, (PlanetTile tid) => Find.WorldGrid[tid].WaterCovered, null);
			float[] flow = new float[array.Length];
			for (int i = 0; i < coastalWaterTiles.Count; i++)
			{
				this.AccumulateFlow(flow, array, coastalWaterTiles[i]);
				this.CreateRivers(flow, array, coastalWaterTiles[i]);
			}
		}

		// Token: 0x060153D5 RID: 86997 RVA: 0x0065A930 File Offset: 0x00658B30
		private static float GetImpliedElevation(Tile tile)
		{
			float num = 0f;
			if (tile.hilliness == Hilliness.SmallHills)
			{
				num = 15f;
			}
			else if (tile.hilliness == Hilliness.LargeHills)
			{
				num = 250f;
			}
			else if (tile.hilliness == Hilliness.Mountainous)
			{
				num = 500f;
			}
			else if (tile.hilliness == Hilliness.Impassable)
			{
				num = 1000f;
			}
			return tile.elevation + num;
		}

		// Token: 0x060153D6 RID: 86998 RVA: 0x0065A990 File Offset: 0x00658B90
		private List<PlanetTile> GetCoastalWaterTiles(PlanetLayer layer)
		{
			List<PlanetTile> list = new List<PlanetTile>();
			List<PlanetTile> list2 = new List<PlanetTile>();
			for (int i = 0; i < Find.WorldGrid.TilesCount; i++)
			{
				PlanetTile planetTile = new PlanetTile(i, layer);
				if (Find.WorldGrid[i].PrimaryBiome == BiomeDefOf.Ocean)
				{
					Find.WorldGrid.GetTileNeighbors(planetTile, list2);
					bool flag = false;
					for (int j = 0; j < list2.Count; j++)
					{
						if (Find.WorldGrid[list2[j]].PrimaryBiome != BiomeDefOf.Ocean)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(planetTile);
					}
				}
			}
			return list;
		}

		// Token: 0x060153D7 RID: 86999 RVA: 0x0065AA3C File Offset: 0x00658C3C
		private void AccumulateFlow(float[] flow, List<PlanetTile>[] riverPaths, PlanetTile planetTile)
		{
			Tile tile = Find.WorldGrid[planetTile];
			int tileId = planetTile.tileId;
			flow[tileId] += tile.rainfall;
			if (riverPaths[tileId] != null)
			{
				for (int i = 0; i < riverPaths[tileId].Count; i++)
				{
					this.AccumulateFlow(flow, riverPaths, riverPaths[tileId][i]);
					flow[tileId] += flow[riverPaths[tileId][i].tileId];
				}
			}
			flow[tileId] = Mathf.Max(0f, flow[tileId] - WorldGenStep_Rivers.CalculateTotalEvaporation(flow[tileId], tile.temperature));
		}

		// Token: 0x060153D8 RID: 87000 RVA: 0x0065AAD0 File Offset: 0x00658CD0
		private void CreateRivers(float[] flow, List<PlanetTile>[] riverPaths, PlanetTile index)
		{
			List<PlanetTile> list = new List<PlanetTile>();
			Find.WorldGrid.GetTileNeighbors(index, list);
			for (int i = 0; i < list.Count; i++)
			{
				float targetFlow = flow[list[i].tileId];
				RiverDef riverDef = (from rd in DefDatabase<RiverDef>.AllDefs
				where rd.spawnFlowThreshold > 0 && (float)rd.spawnFlowThreshold <= targetFlow
				select rd).MaxByWithFallback((RiverDef rd) => rd.spawnFlowThreshold, null);
				if (riverDef != null && Rand.Value < riverDef.spawnChance)
				{
					PlanetTile planetTile = list[i];
					Find.WorldGrid.OverlayRiver(index, planetTile, riverDef);
					this.ExtendRiver(flow, riverPaths, planetTile, riverDef);
				}
			}
		}

		// Token: 0x060153D9 RID: 87001 RVA: 0x0065AB90 File Offset: 0x00658D90
		private void ExtendRiver(float[] flow, List<PlanetTile>[] riverPaths, PlanetTile planetTile, RiverDef incomingRiver)
		{
			if (riverPaths[planetTile.tileId] == null)
			{
				return;
			}
			PlanetTile bestOutput = riverPaths[planetTile.tileId].MaxBy((PlanetTile x) => flow[x.tileId]);
			RiverDef riverDef = incomingRiver;
			while (riverDef != null && (float)riverDef.degradeThreshold > flow[bestOutput.tileId])
			{
				riverDef = riverDef.degradeChild;
			}
			if (riverDef != null)
			{
				Find.WorldGrid.OverlayRiver(planetTile, bestOutput, riverDef);
				this.ExtendRiver(flow, riverPaths, bestOutput, riverDef);
			}
			if (incomingRiver.branches != null)
			{
				IEnumerable<PlanetTile> source = riverPaths[planetTile.tileId];
				Func<PlanetTile, bool> <>9__1;
				Func<PlanetTile, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PlanetTile ni) => ni != bestOutput));
				}
				using (IEnumerator<PlanetTile> enumerator = source.Where(predicate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PlanetTile alternateRiver = enumerator.Current;
						RiverDef.Branch branch2 = (from branch in incomingRiver.branches
						where (float)branch.minFlow <= flow[alternateRiver.tileId]
						select branch).MaxByWithFallback((RiverDef.Branch branch) => branch.minFlow, null);
						if (branch2 != null && Rand.Value < branch2.chance)
						{
							Find.WorldGrid.OverlayRiver(planetTile, alternateRiver, branch2.child);
							this.ExtendRiver(flow, riverPaths, alternateRiver, branch2.child);
						}
					}
				}
			}
		}

		// Token: 0x060153DA RID: 87002 RVA: 0x0065AD3C File Offset: 0x00658F3C
		public static float CalculateEvaporationConstant(float temperature)
		{
			return 0.61121f * Mathf.Exp((18.678f - temperature / 234.5f) * (temperature / (257.14f + temperature))) / (temperature + 273f);
		}

		// Token: 0x060153DB RID: 87003 RVA: 0x0065AD68 File Offset: 0x00658F68
		public static float CalculateRiverSurfaceArea(float flow)
		{
			return Mathf.Pow(flow, 0.5f);
		}

		// Token: 0x060153DC RID: 87004 RVA: 0x0065AD75 File Offset: 0x00658F75
		public static float CalculateEvaporativeArea(float flow)
		{
			return WorldGenStep_Rivers.CalculateRiverSurfaceArea(flow) + 0f;
		}

		// Token: 0x060153DD RID: 87005 RVA: 0x0065AD83 File Offset: 0x00658F83
		public static float CalculateTotalEvaporation(float flow, float temperature)
		{
			return WorldGenStep_Rivers.CalculateEvaporationConstant(temperature) * WorldGenStep_Rivers.CalculateEvaporativeArea(flow) * 250f;
		}

		// Token: 0x0400ECCD RID: 60621
		private const float HillinessSmallHillsElevation = 15f;

		// Token: 0x0400ECCE RID: 60622
		private const float HillinessLargeHillsElevation = 250f;

		// Token: 0x0400ECCF RID: 60623
		private const float HillinessMountainousElevation = 500f;

		// Token: 0x0400ECD0 RID: 60624
		private const float HillinessImpassableElevation = 1000f;

		// Token: 0x0400ECD1 RID: 60625
		private const float NonRiverEvaporation = 0f;

		// Token: 0x0400ECD2 RID: 60626
		private const float EvaporationMultiple = 250f;

		// Token: 0x0400ECD3 RID: 60627
		private static readonly SimpleCurve ElevationChangeCost = new SimpleCurve
		{
			{
				new CurvePoint(-1000f, 50f),
				true
			},
			{
				new CurvePoint(-100f, 100f),
				true
			},
			{
				new CurvePoint(0f, 400f),
				true
			},
			{
				new CurvePoint(0f, 5000f),
				true
			},
			{
				new CurvePoint(100f, 50000f),
				true
			},
			{
				new CurvePoint(1000f, 50000f),
				true
			}
		};
	}
}
