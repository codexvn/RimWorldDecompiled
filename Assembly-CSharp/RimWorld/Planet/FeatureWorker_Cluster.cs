using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C2D RID: 15405
	public abstract class FeatureWorker_Cluster : FeatureWorker
	{
		// Token: 0x170033AE RID: 13230
		// (get) Token: 0x06014E62 RID: 85602 RVA: 0x006426A9 File Offset: 0x006408A9
		protected virtual int MinRootGroupsInCluster
		{
			get
			{
				return this.def.minRootGroupsInCluster;
			}
		}

		// Token: 0x170033AF RID: 13231
		// (get) Token: 0x06014E63 RID: 85603 RVA: 0x006426B6 File Offset: 0x006408B6
		protected virtual int MinRootGroupSize
		{
			get
			{
				return this.def.minRootGroupSize;
			}
		}

		// Token: 0x170033B0 RID: 13232
		// (get) Token: 0x06014E64 RID: 85604 RVA: 0x006426C3 File Offset: 0x006408C3
		protected virtual int MaxRootGroupSize
		{
			get
			{
				return this.def.maxRootGroupSize;
			}
		}

		// Token: 0x170033B1 RID: 13233
		// (get) Token: 0x06014E65 RID: 85605 RVA: 0x006426D0 File Offset: 0x006408D0
		protected virtual int MinOverallSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170033B2 RID: 13234
		// (get) Token: 0x06014E66 RID: 85606 RVA: 0x006426DD File Offset: 0x006408DD
		protected virtual int MaxOverallSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170033B3 RID: 13235
		// (get) Token: 0x06014E67 RID: 85607 RVA: 0x006426EA File Offset: 0x006408EA
		protected virtual int MaxSpaceBetweenRootGroups
		{
			get
			{
				return this.def.maxSpaceBetweenRootGroups;
			}
		}

		// Token: 0x06014E68 RID: 85608
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x06014E69 RID: 85609 RVA: 0x006426F7 File Offset: 0x006408F7
		protected virtual bool CanTraverse(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return true;
		}

		// Token: 0x06014E6A RID: 85610 RVA: 0x006426FD File Offset: 0x006408FD
		protected virtual bool IsMember(PlanetTile tile, out bool ifRootThenRootGroupSizeMustMatch)
		{
			ifRootThenRootGroupSizeMustMatch = false;
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06014E6B RID: 85611 RVA: 0x00642715 File Offset: 0x00640915
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRootTiles(layer);
			this.CalculateRootsWithAreaInBetween(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x06014E6C RID: 85612 RVA: 0x0064272C File Offset: 0x0064092C
		private void CalculateRootTiles(PlanetLayer layer)
		{
			this.roots.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < layer.TilesCount; i++)
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

		// Token: 0x06014E6D RID: 85613 RVA: 0x00642798 File Offset: 0x00640998
		private void CalculateRootsWithAreaInBetween(PlanetLayer layer)
		{
			this.rootsWithAreaInBetween.Clear();
			this.rootsWithAreaInBetween.AddRange(this.roots);
			GenPlanetMorphology.Close(layer, this.rootsWithAreaInBetween, this.MaxSpaceBetweenRootGroups);
			this.rootsWithAreaInBetweenSet.Clear();
			this.rootsWithAreaInBetweenSet.AddRange(this.rootsWithAreaInBetween);
		}

		// Token: 0x06014E6E RID: 85614 RVA: 0x006427F0 File Offset: 0x006409F0
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			WorldFloodFiller filler = layer.Filler;
			WorldGrid worldGrid = Find.WorldGrid;
			int minRootGroupSize = this.MinRootGroupSize;
			int maxRootGroupSize = this.MaxRootGroupSize;
			int minOverallSize = this.MinOverallSize;
			int maxOverallSize = this.MaxOverallSize;
			int minRootGroupsInCluster = this.MinRootGroupsInCluster;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			FeatureWorker.ClearGroupIDs(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.roots.Count; i++)
			{
				PlanetTile planetTile = this.roots[i];
				if (!FeatureWorker.visited[planetTile.tileId])
				{
					bool anyMember = false;
					FeatureWorker_Cluster.tmpGroup.Clear();
					WorldFloodFiller worldFloodFiller = filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.rootsSet.Contains(x)));
					}
					worldFloodFiller.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_Cluster.tmpGroup.Add(x.tileId);
						bool flag2;
						if (!anyMember && this.IsMember(x, out flag2))
						{
							anyMember = true;
						}
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_Cluster.tmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_Cluster.tmpGroup[j]] = FeatureWorker_Cluster.tmpGroup.Count;
						if (anyMember)
						{
							FeatureWorker.groupID[FeatureWorker_Cluster.tmpGroup[j]] = i + 1;
						}
					}
				}
			}
			FeatureWorker.ClearVisited(layer);
			Predicate<PlanetTile> <>9__2;
			Action<PlanetTile> <>9__3;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__5;
			Predicate<PlanetTile> <>9__6;
			for (int k = 0; k < this.roots.Count; k++)
			{
				PlanetTile planetTile2 = this.roots[k];
				if (!FeatureWorker.visited[planetTile2.tileId] && FeatureWorker.groupSize[planetTile2.tileId] >= minRootGroupSize && FeatureWorker.groupSize[planetTile2.tileId] <= maxRootGroupSize && FeatureWorker.groupSize[planetTile2.tileId] <= maxOverallSize)
				{
					this.currentGroup.Clear();
					this.visitedValidGroupIDs.Clear();
					WorldFloodFiller worldFloodFiller2 = filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = delegate(PlanetTile x)
						{
							bool flag2;
							return this.rootsWithAreaInBetweenSet.Contains(x) && this.CanTraverse(x, out flag2) && (!flag2 || !this.rootsSet.Contains(x) || (FeatureWorker.groupSize[x.tileId] >= minRootGroupSize && FeatureWorker.groupSize[x.tileId] <= maxRootGroupSize));
						});
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__3) == null)
					{
						processor = (<>9__3 = delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							this.currentGroup.Add(x);
							if (FeatureWorker.groupID[x.tileId] != 0 && FeatureWorker.groupSize[x.tileId] >= minRootGroupSize && FeatureWorker.groupSize[x.tileId] <= maxRootGroupSize)
							{
								this.visitedValidGroupIDs.Add(FeatureWorker.groupID[x.tileId]);
							}
						});
					}
					worldFloodFiller2.FloodFill(rootTile2, passCheck2, processor, int.MaxValue, null);
					if (this.currentGroup.Count >= minOverallSize && this.currentGroup.Count <= maxOverallSize && this.visitedValidGroupIDs.Count >= minRootGroupsInCluster)
					{
						if (!this.def.canTouchWorldEdge)
						{
							List<PlanetTile> list = this.currentGroup;
							Predicate<PlanetTile> predicate;
							if ((predicate = <>9__4) == null)
							{
								predicate = (<>9__4 = ((PlanetTile x) => worldGrid.IsOnEdge(x)));
							}
							if (list.Any(predicate))
							{
								goto IL_3C1;
							}
						}
						this.currentGroupMembers.Clear();
						for (int l = 0; l < this.currentGroup.Count; l++)
						{
							PlanetTile planetTile3 = this.currentGroup[l];
							bool flag;
							if (this.IsMember(planetTile3, out flag) && (!flag || !this.rootsSet.Contains(planetTile3) || (FeatureWorker.groupSize[planetTile3.tileId] >= minRootGroupSize && FeatureWorker.groupSize[planetTile3.tileId] <= maxRootGroupSize)))
							{
								this.currentGroupMembers.Add(this.currentGroup[l]);
							}
						}
						if (this.currentGroupMembers.Count >= minOverallSize)
						{
							List<PlanetTile> list2 = this.currentGroup;
							Predicate<PlanetTile> predicate2;
							if ((predicate2 = <>9__5) == null)
							{
								predicate2 = (<>9__5 = ((PlanetTile x) => worldGrid[x].feature == null));
							}
							if (list2.Any(predicate2))
							{
								List<PlanetTile> list3 = this.currentGroup;
								Predicate<PlanetTile> match;
								if ((match = <>9__6) == null)
								{
									match = (<>9__6 = ((PlanetTile x) => worldGrid[x].feature != null));
								}
								list3.RemoveAll(match);
							}
							base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
						}
					}
				}
				IL_3C1:;
			}
		}

		// Token: 0x0400E8FE RID: 59646
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x0400E8FF RID: 59647
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x0400E900 RID: 59648
		private readonly List<PlanetTile> rootsWithAreaInBetween = new List<PlanetTile>();

		// Token: 0x0400E901 RID: 59649
		private readonly HashSet<PlanetTile> rootsWithAreaInBetweenSet = new HashSet<PlanetTile>();

		// Token: 0x0400E902 RID: 59650
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x0400E903 RID: 59651
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x0400E904 RID: 59652
		private readonly HashSet<int> visitedValidGroupIDs = new HashSet<int>();

		// Token: 0x0400E905 RID: 59653
		private static readonly List<int> tmpGroup = new List<int>();
	}
}
