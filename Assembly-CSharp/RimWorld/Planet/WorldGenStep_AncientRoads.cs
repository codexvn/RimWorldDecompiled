using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CEA RID: 15594
	public class WorldGenStep_AncientRoads : WorldGenStep
	{
		// Token: 0x170034E8 RID: 13544
		// (get) Token: 0x06015399 RID: 86937 RVA: 0x00659844 File Offset: 0x00657A44
		public override int SeedPart
		{
			get
			{
				return 773428712;
			}
		}

		// Token: 0x0601539A RID: 86938 RVA: 0x0065984B File Offset: 0x00657A4B
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateAncientRoads(layer);
		}

		// Token: 0x0601539B RID: 86939 RVA: 0x00659854 File Offset: 0x00657A54
		private void GenerateAncientRoads(PlanetLayer layer)
		{
			Find.WorldPathGrid.RecalculateLayerPerceivedPathCosts(layer, new int?(0));
			List<List<PlanetTile>> list = this.GenerateProspectiveRoads(layer);
			list.Sort((List<PlanetTile> lhs, List<PlanetTile> rhs) => -lhs.Count.CompareTo(rhs.Count));
			HashSet<PlanetTile> used = new HashSet<PlanetTile>();
			Predicate<PlanetTile> <>9__1;
			for (int i = 0; i < list.Count; i++)
			{
				List<PlanetTile> list2 = list[i];
				Predicate<PlanetTile> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PlanetTile elem) => used.Contains(elem)));
				}
				if (!list2.Any(predicate))
				{
					if (list[i].Count < 4)
					{
						break;
					}
					foreach (PlanetTile item in list[i])
					{
						used.Add(item);
					}
					for (int j = 0; j < list[i].Count - 1; j++)
					{
						float num = Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness;
						float costCutoff = num * 12000f;
						using (WorldPath worldPath = layer.Pather.FindPath(list[i][j], list[i][j + 1], null, (float cost) => cost > costCutoff))
						{
							if (worldPath != null && worldPath != WorldPath.NotFound)
							{
								List<PlanetTile> nodesReversed = worldPath.NodesReversed;
								if ((float)nodesReversed.Count <= Find.WorldGrid.ApproxDistanceInTiles(list[i][j], list[i][j + 1]) * this.maximumSegmentCurviness)
								{
									for (int k = 0; k < nodesReversed.Count - 1; k++)
									{
										if (Find.WorldGrid.GetRoadDef(nodesReversed[k], nodesReversed[k + 1], false) != null)
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltHighway);
										}
										else
										{
											Find.WorldGrid.OverlayRoad(nodesReversed[k], nodesReversed[k + 1], RoadDefOf.AncientAsphaltRoad);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0601539C RID: 86940 RVA: 0x00659AF0 File Offset: 0x00657CF0
		private List<List<PlanetTile>> GenerateProspectiveRoads(PlanetLayer layer)
		{
			List<PlanetTile> list = Find.World.genData.ancientSites[layer];
			List<List<PlanetTile>> list2 = new List<List<PlanetTile>>();
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					List<PlanetTile> list3 = new List<PlanetTile>();
					list3.Add(list[i]);
					List<PlanetTile> list4 = list;
					float ang = Find.World.grid.GetHeadingFromTo(list[i], list[j]);
					PlanetTile current = list[i];
					Func<PlanetTile, bool> <>9__0;
					Func<PlanetTile, float> <>9__1;
					for (;;)
					{
						IEnumerable<PlanetTile> source = list4;
						Func<PlanetTile, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((PlanetTile idx) => idx != current && Math.Abs(Find.World.grid.GetHeadingFromTo(current, idx) - ang) < this.maximumSiteCurve));
						}
						list4 = source.Where(predicate).ToList<PlanetTile>();
						if (list4.Count == 0)
						{
							break;
						}
						IEnumerable<PlanetTile> source2 = list4;
						Func<PlanetTile, float> selector;
						if ((selector = <>9__1) == null)
						{
							selector = (<>9__1 = ((PlanetTile idx) => Find.World.grid.ApproxDistanceInTiles(current, idx)));
						}
						PlanetTile planetTile = source2.MinBy(selector);
						ang = Find.World.grid.GetHeadingFromTo(current, planetTile);
						current = planetTile;
						list3.Add(current);
					}
					list2.Add(list3);
				}
			}
			return list2;
		}

		// Token: 0x0400ECA6 RID: 60582
		public float maximumSiteCurve;

		// Token: 0x0400ECA7 RID: 60583
		public float maximumSegmentCurviness;
	}
}
