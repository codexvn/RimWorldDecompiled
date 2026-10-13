using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C34 RID: 15412
	public abstract class FeatureWorker_FloodFill : FeatureWorker
	{
		// Token: 0x170033B4 RID: 13236
		// (get) Token: 0x06014E87 RID: 85639 RVA: 0x006426D0 File Offset: 0x006408D0
		protected virtual int MinSize
		{
			get
			{
				return this.def.minSize;
			}
		}

		// Token: 0x170033B5 RID: 13237
		// (get) Token: 0x06014E88 RID: 85640 RVA: 0x006426DD File Offset: 0x006408DD
		protected virtual int MaxSize
		{
			get
			{
				return this.def.maxSize;
			}
		}

		// Token: 0x170033B6 RID: 13238
		// (get) Token: 0x06014E89 RID: 85641 RVA: 0x00643018 File Offset: 0x00641218
		protected virtual int MaxPossiblyAllowedSizeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizeToTake;
			}
		}

		// Token: 0x170033B7 RID: 13239
		// (get) Token: 0x06014E8A RID: 85642 RVA: 0x00643025 File Offset: 0x00641225
		protected virtual float MaxPossiblyAllowedSizePctOfMeToTake
		{
			get
			{
				return this.def.maxPossiblyAllowedSizePctOfMeToTake;
			}
		}

		// Token: 0x06014E8B RID: 85643
		protected abstract bool IsRoot(PlanetTile tile);

		// Token: 0x06014E8C RID: 85644 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool IsPossiblyAllowed(PlanetTile tile)
		{
			return false;
		}

		// Token: 0x06014E8D RID: 85645 RVA: 0x00643032 File Offset: 0x00641232
		protected virtual bool IsMember(PlanetTile tile)
		{
			return Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x06014E8E RID: 85646 RVA: 0x00643047 File Offset: 0x00641247
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.CalculateRootsAndPossiblyAllowedTiles(layer);
			this.CalculateContiguousGroups(layer);
		}

		// Token: 0x06014E8F RID: 85647 RVA: 0x00643058 File Offset: 0x00641258
		private void CalculateRootsAndPossiblyAllowedTiles(PlanetLayer layer)
		{
			this.roots.Clear();
			this.possiblyAllowed.Clear();
			int tilesCount = layer.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				PlanetTile tile = layer[i].tile;
				if (this.IsRoot(tile))
				{
					this.roots.Add(tile);
				}
				if (this.IsPossiblyAllowed(tile))
				{
					this.possiblyAllowed.Add(tile);
				}
			}
			this.rootsSet.Clear();
			this.rootsSet.AddRange(this.roots);
			this.possiblyAllowedSet.Clear();
			this.possiblyAllowedSet.AddRange(this.possiblyAllowed);
		}

		// Token: 0x06014E90 RID: 85648 RVA: 0x00643100 File Offset: 0x00641300
		private void CalculateContiguousGroups(PlanetLayer layer)
		{
			WorldFloodFiller filler = layer.Filler;
			int minSize = this.MinSize;
			int maxSize = this.MaxSize;
			int maxPossiblyAllowedSizeToTake = this.MaxPossiblyAllowedSizeToTake;
			float maxPossiblyAllowedSizePctOfMeToTake = this.MaxPossiblyAllowedSizePctOfMeToTake;
			FeatureWorker.ClearVisited(layer);
			FeatureWorker.ClearGroupSizes(layer);
			Predicate<PlanetTile> <>9__0;
			for (int i = 0; i < this.possiblyAllowed.Count; i++)
			{
				PlanetTile planetTile = this.possiblyAllowed[i];
				if (!FeatureWorker.visited[planetTile.tileId] && !this.rootsSet.Contains(planetTile))
				{
					FeatureWorker_FloodFill.TmpGroup.Clear();
					WorldFloodFiller worldFloodFiller = filler;
					PlanetTile rootTile = planetTile;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile x) => this.possiblyAllowedSet.Contains(x) && !this.rootsSet.Contains(x)));
					}
					worldFloodFiller.FloodFill(rootTile, passCheck, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						FeatureWorker_FloodFill.TmpGroup.Add(x);
					}, int.MaxValue, null);
					for (int j = 0; j < FeatureWorker_FloodFill.TmpGroup.Count; j++)
					{
						FeatureWorker.groupSize[FeatureWorker_FloodFill.TmpGroup[j].tileId] = FeatureWorker_FloodFill.TmpGroup.Count;
					}
				}
			}
			Predicate<PlanetTile> <>9__2;
			Predicate<PlanetTile> <>9__4;
			Predicate<PlanetTile> <>9__9;
			Predicate<PlanetTile> <>9__10;
			for (int k = 0; k < this.roots.Count; k++)
			{
				PlanetTile planetTile2 = this.roots[k];
				if (!FeatureWorker.visited[planetTile2.tileId])
				{
					int initialMembersCountClamped = 0;
					WorldFloodFiller worldFloodFiller2 = filler;
					PlanetTile rootTile2 = planetTile2;
					Predicate<PlanetTile> passCheck2;
					if ((passCheck2 = <>9__2) == null)
					{
						passCheck2 = (<>9__2 = ((PlanetTile x) => (this.rootsSet.Contains(x) || this.possiblyAllowedSet.Contains(x)) && this.IsMember(x)));
					}
					worldFloodFiller2.FloodFill(rootTile2, passCheck2, delegate(PlanetTile x)
					{
						FeatureWorker.visited[x.tileId] = true;
						int initialMembersCountClamped = initialMembersCountClamped;
						initialMembersCountClamped++;
						return initialMembersCountClamped >= minSize;
					}, int.MaxValue, null);
					if (initialMembersCountClamped >= minSize)
					{
						int initialRootsCount = 0;
						WorldFloodFiller worldFloodFiller3 = filler;
						PlanetTile rootTile3 = planetTile2;
						Predicate<PlanetTile> passCheck3;
						if ((passCheck3 = <>9__4) == null)
						{
							passCheck3 = (<>9__4 = ((PlanetTile x) => this.rootsSet.Contains(x)));
						}
						worldFloodFiller3.FloodFill(rootTile3, passCheck3, delegate(PlanetTile x)
						{
							FeatureWorker.visited[x.tileId] = true;
							int initialRootsCount = initialRootsCount;
							initialRootsCount++;
						}, int.MaxValue, null);
						if (initialRootsCount >= minSize && initialRootsCount <= maxSize)
						{
							int traversedRootsCount = 0;
							this.currentGroup.Clear();
							filler.FloodFill(planetTile2, (PlanetTile x) => this.rootsSet.Contains(x) || (this.possiblyAllowedSet.Contains(x) && FeatureWorker.groupSize[x.tileId] <= maxPossiblyAllowedSizeToTake && (float)FeatureWorker.groupSize[x.tileId] <= maxPossiblyAllowedSizePctOfMeToTake * (float)Mathf.Max(traversedRootsCount, initialRootsCount) && FeatureWorker.groupSize[x.tileId] < maxSize), delegate(PlanetTile x)
							{
								FeatureWorker.visited[x.tileId] = true;
								if (this.rootsSet.Contains(x))
								{
									int traversedRootsCount = traversedRootsCount;
									traversedRootsCount++;
								}
								this.currentGroup.Add(x);
							}, int.MaxValue, null);
							if (this.currentGroup.Count >= minSize && this.currentGroup.Count <= maxSize)
							{
								if (!this.def.canTouchWorldEdge)
								{
									if (this.currentGroup.Any((PlanetTile x) => Find.WorldGrid.IsOnEdge(x)))
									{
										goto IL_428;
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
									if ((predicate = <>9__9) == null)
									{
										predicate = (<>9__9 = ((PlanetTile x) => layer[x].feature == null));
									}
									if (list.Any(predicate))
									{
										List<PlanetTile> list2 = this.currentGroup;
										Predicate<PlanetTile> match;
										if ((match = <>9__10) == null)
										{
											match = (<>9__10 = ((PlanetTile x) => layer[x].feature != null));
										}
										list2.RemoveAll(match);
									}
									base.AddFeature(layer, this.currentGroupMembers, this.currentGroup);
								}
							}
						}
					}
				}
				IL_428:;
			}
		}

		// Token: 0x0400E916 RID: 59670
		private readonly List<PlanetTile> roots = new List<PlanetTile>();

		// Token: 0x0400E917 RID: 59671
		private readonly HashSet<PlanetTile> rootsSet = new HashSet<PlanetTile>();

		// Token: 0x0400E918 RID: 59672
		private readonly List<PlanetTile> possiblyAllowed = new List<PlanetTile>();

		// Token: 0x0400E919 RID: 59673
		private readonly HashSet<PlanetTile> possiblyAllowedSet = new HashSet<PlanetTile>();

		// Token: 0x0400E91A RID: 59674
		private readonly List<PlanetTile> currentGroup = new List<PlanetTile>();

		// Token: 0x0400E91B RID: 59675
		private readonly List<PlanetTile> currentGroupMembers = new List<PlanetTile>();

		// Token: 0x0400E91C RID: 59676
		private static readonly List<PlanetTile> TmpGroup = new List<PlanetTile>();
	}
}
