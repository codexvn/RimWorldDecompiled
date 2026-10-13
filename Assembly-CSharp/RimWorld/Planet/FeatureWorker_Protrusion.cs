using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C3B RID: 15419
	public abstract class FeatureWorker_Protrusion : FeatureWorker
	{
		// Token: 0x170033B8 RID: 13240
		// (get) Token: 0x06014EA9 RID: 85673 RVA: 0x006426D0 File Offset: 0x006408D0
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170033B9 RID: 13241
		// (get) Token: 0x06014EAA RID: 85674 RVA: 0x006426DD File Offset: 0x006408DD
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170033BA RID: 13242
		// (get) Token: 0x06014EAB RID: 85675 RVA: 0x0064389F File Offset: 0x00641A9F
		protected virtual int MaxPassageWidth
		{
			get
			{
				return this.def.maxPassageWidth;
			}
		}

		// Token: 0x170033BB RID: 13243
		// (get) Token: 0x06014EAC RID: 85676 RVA: 0x006438AC File Offset: 0x00641AAC
		protected virtual float MaxPctOfWholeArea
		{
			get
			{
				return this.def.maxPctOfWholeArea;
			}
		}

		// Token: 0x06014EAD RID: 85677
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x06014EAE RID: 85678 RVA: 0x00643032 File Offset: 0x00641232
		protected virtual bool IsMember(PlanetTile tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06014EAF RID: 85679 RVA: 0x006438B9 File Offset: 0x00641AB9
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRoots(layer);
			this.CalculateRootsWithoutSmallPassages(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x06014EB0 RID: 85680 RVA: 0x006438D0 File Offset: 0x00641AD0
		private void CalculateRoots(PlanetLayer layer)
		{
			this.roots.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				PlanetTile planetTile = new PlanetTile(i, layer);
				if (this.IsRoot(planetTile))
				{
					this.roots.Add(planetTile);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
		}

		// Token: 0x06014EB1 RID: 85681 RVA: 0x00643938 File Offset: 0x00641B38
		private void CalculateRootsWithoutSmallPassages(PlanetLayer layer)
		{
			this.rootsWithoutSmallPassages.Clear();
			this.rootsWithoutSmallPassages.AddRange(this.roots);
			GenPlanetMorphology.Open(layer, this.rootsWithoutSmallPassages, this.MaxPassageWidth);
			this.rootsWithoutSmallPassagesSet.Clear();
			this.rootsWithoutSmallPassagesSet.AddRange(this.rootsWithoutSmallPassages);
		}

		// Token: 0x06014EB2 RID: 85682 RVA: 0x00643990 File Offset: 0x00641B90
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			float maxPctOfWholeArea = this.MaxPctOfWholeArea;
			int maxPassageWidth = this.MaxPassageWidth;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				PlanetTile planetTile = this.roots[i];
				if (!FeatureWorker.visited[planetTile.tileId])
				{
					FeatureWorker_Protrusion.tmpGroup.Clear();
					WorldFloodFiller filler = layer.Filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.rootsSet.Contains(x)));
					}
					filler.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_Protrusion.tmpGroup.Add(x.tileId);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Protrusion.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Protrusion.tmpGroup[j]] = FeatureWorker_Protrusion.tmpGroup.Count;
					}
					FeatureWorker_Protrusion.tmpGroup.Clear();
				}
			}
			FeatureWorker.ClearVisited(layer);
			Predicate<PlanetTile> <>9__2;
			Action<PlanetTile> <>9__3;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__6;
			Predicate<PlanetTile> <>9__7;
			for (int k = 0; k < this.rootsWithoutSmallPassages.Count; k++)
			{
				PlanetTile planetTile2 = this.rootsWithoutSmallPassages[k];
				if (!FeatureWorker.visited[planetTile2.tileId])
				{
					this.currentGroup.Clear();
					WorldFloodFiller filler2 = layer.Filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((PlanetTile x) => this.rootsWithoutSmallPassagesSet.Contains(x)));
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							this.currentGroup.Add(x);
						});
					}
					filler2.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minSize)
					{
						PlanetLayer layer2 = layer;
						List<PlanetTile> tiles = this.currentGroup;
						int count = maxPassageWidth * 2;
						Predicate<PlanetTile> extraPredicate;
						if ((extraPredicate = <>9__4) == null)
						{
							extraPredicate = (<>9__4 = ((PlanetTile x) => this.rootsSet.Contains(x)));
						}
						GenPlanetMorphology.Dilate(layer2, tiles, count, extraPredicate);
						if (this.currentGroup.Count <= maxSize && (float)this.currentGroup.Count / (float)FeatureWorker.groupSize[planetTile2.tileId] <= maxPctOfWholeArea)
						{
							if (!this.def.canTouchWorldEdge)
							{
								if (this.currentGroup.Any((PlanetTile x) => Find.WorldGrid.IsOnEdge(x)))
								{
									goto IL_34A;
								}
							}
							this.currentGroupMembers.Clear();
							for (int l = 0; l < this.currentGroup.Count; l++)
							{
								if (this.IsMember(this.currentGroup[l]))
								{
									this.currentGroupMembers.Add(this.currentGroup[l]);
								}
							}
							if (this.currentGroupMembers.Count >= minSize)
							{
								List<PlanetTile> list = this.currentGroup;
								Predicate<PlanetTile> predicate;
								if ((predicate = <>9__6) == null)
								{
									predicate = (<>9__6 = ((PlanetTile x) => layer[x].feature == null));
								}
								if (list.Any(predicate))
								{
									List<PlanetTile> list2 = this.currentGroup;
									Predicate<PlanetTile> match;
									if ((match = <>9__7) == null)
									{
										match = (<>9__7 = ((PlanetTile x) => layer[x].feature != null));
									}
									list2.RemoveAll(match);
								}
								base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
							}
						}
					}
				}
				IL_34A:;
			}
		}

		// Token: 0x0400E92F RID: 59695
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x0400E930 RID: 59696
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x0400E931 RID: 59697
		private readonly List<PlanetTile> rootsWithoutSmallPassages = new List<PlanetTile>();

		// Token: 0x0400E932 RID: 59698
		private readonly HashSet<PlanetTile> rootsWithoutSmallPassagesSet = new HashSet<PlanetTile>();

		// Token: 0x0400E933 RID: 59699
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x0400E934 RID: 59700
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x0400E935 RID: 59701
		private static readonly List<int> tmpGroup = new List<int>();
	}
}
