using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000503 RID: 1283
	public static class RegionTraverser
	{
		// Token: 0x06002149 RID: 8521 RVA: 0x000AE084 File Offset: 0x000AC284
		public static District FloodAndSetDistricts(Region root, Map map, District existingRoom)
		{
			District floodingDistrict;
			if (existingRoom == null)
			{
				floodingDistrict = District.MakeNew(map);
			}
			else
			{
				floodingDistrict = existingRoom;
			}
			root.District = floodingDistrict;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return floodingDistrict;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.District != floodingDistrict;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.District = floodingDistrict;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
			return floodingDistrict;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000AE114 File Offset: 0x000AC314
		public static void FloodAndSetNewRegionIndex(Region root, int newRegionGroupIndex)
		{
			root.newRegionGroupIndex = newRegionGroupIndex;
			if (!root.type.AllowsMultipleRegionsPerDistrict())
			{
				return;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.type == root.type && r.newRegionGroupIndex < 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				r.newRegionGroupIndex = newRegionGroupIndex;
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, 999999, RegionType.Set_All);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000AE188 File Offset: 0x000AC388
		public static bool WithinRegions(this IntVec3 A, IntVec3 B, Map map, int regionLookCount, TraverseParms traverseParams, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = A.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return false;
			}
			Region regB = B.GetRegion(map, traversableRegionTypes);
			if (regB == null)
			{
				return false;
			}
			if (region == regB)
			{
				return true;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			bool found = false;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r == regB)
				{
					found = true;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, regionLookCount, traversableRegionTypes);
			return found;
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x000AE208 File Offset: 0x000AC408
		public static void MarkRegionsBFS(Region root, RegionEntryPredicate entryCondition, int maxRegions, int inRadiusMark, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, entryCondition, delegate(Region r)
			{
				r.mark = inRadiusMark;
				return false;
			}, maxRegions, traversableRegionTypes);
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000AE238 File Offset: 0x000AC438
		public static bool ShouldCountRegion(Region r)
		{
			return !r.IsDoorway;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000AE243 File Offset: 0x000AC443
		static RegionTraverser()
		{
			RegionTraverser.RecreateWorkers();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000AE270 File Offset: 0x000AC470
		public static void RecreateWorkers()
		{
			RegionTraverser.freeWorkers.Clear();
			for (int i = 0; i < RegionTraverser.NumWorkers; i++)
			{
				RegionTraverser.freeWorkers.Enqueue(new RegionTraverser.BFSWorker(i));
			}
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000AE2A8 File Offset: 0x000AC4A8
		public static void BreadthFirstTraverse(IntVec3 start, Map map, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = start.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000AE2CF File Offset: 0x000AC4CF
		public static void BreadthFirstTraverse(Region root, RegionProcessorDelegateCache processor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			RegionTraverser.BreadthFirstTraverse(root, processor.RegionEntryPredicateDelegate, processor.RegionProcessorDelegate, maxRegions, traversableRegionTypes);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000AE2E8 File Offset: 0x000AC4E8
		public static void BreadthFirstTraverse(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions = 999999, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			if (RegionTraverser.freeWorkers.Count == 0)
			{
				Log.Error("No free workers for breadth-first traversal. Either BFS recurred deeper than " + RegionTraverser.NumWorkers.ToString() + ", or a bug has put this system in an inconsistent state. Resetting.");
				return;
			}
			if (root == null)
			{
				Log.Error("BreadthFirstTraverse with null root region.");
				return;
			}
			RegionTraverser.BFSWorker bfsworker = RegionTraverser.freeWorkers.Dequeue();
			try
			{
				bfsworker.BreadthFirstTraverseWork(root, entryCondition, regionProcessor, maxRegions, traversableRegionTypes);
			}
			catch (Exception ex)
			{
				Log.Error("Exception in BreadthFirstTraverse: " + ex.ToString());
			}
			finally
			{
				bfsworker.Clear();
				RegionTraverser.freeWorkers.Enqueue(bfsworker);
			}
		}

		// Token: 0x04001C32 RID: 7218
		private static Queue<RegionTraverser.BFSWorker> freeWorkers = new Queue<RegionTraverser.BFSWorker>();

		// Token: 0x04001C33 RID: 7219
		public static int NumWorkers = 8;

		// Token: 0x04001C34 RID: 7220
		public static readonly RegionEntryPredicate PassAll = (Region from, Region to) => true;

		// Token: 0x02000504 RID: 1284
		private class BFSWorker
		{
			// Token: 0x06002153 RID: 8531 RVA: 0x000AE390 File Offset: 0x000AC590
			public BFSWorker(int closedArrayPos)
			{
				this.closedArrayPos = closedArrayPos;
			}

			// Token: 0x06002154 RID: 8532 RVA: 0x000AE3B1 File Offset: 0x000AC5B1
			public void Clear()
			{
				this.open.Clear();
			}

			// Token: 0x06002155 RID: 8533 RVA: 0x000AE3C0 File Offset: 0x000AC5C0
			private void QueueNewOpenRegion(Region region)
			{
				if (region.closedIndex[this.closedArrayPos] == this.closedIndex)
				{
					throw new InvalidOperationException("Region is already closed; you can't open it. Region: " + region.ToString());
				}
				this.open.Enqueue(region);
				region.closedIndex[this.closedArrayPos] = this.closedIndex;
			}

			// Token: 0x06002156 RID: 8534 RVA: 0x000026EA File Offset: 0x000008EA
			private void FinalizeSearch()
			{
			}

			// Token: 0x06002157 RID: 8535 RVA: 0x000AE418 File Offset: 0x000AC618
			public void BreadthFirstTraverseWork(Region root, RegionEntryPredicate entryCondition, RegionProcessor regionProcessor, int maxRegions, RegionType traversableRegionTypes)
			{
				if ((root.type & traversableRegionTypes) == RegionType.None)
				{
					return;
				}
				this.closedIndex += 1U;
				this.open.Clear();
				this.numRegionsProcessed = 0;
				this.QueueNewOpenRegion(root);
				while (this.open.Count > 0)
				{
					Region region = this.open.Dequeue();
					if (DebugViewSettings.drawRegionTraversal)
					{
						region.Debug_Notify_Traversed();
					}
					if (regionProcessor != null && regionProcessor(region))
					{
						this.FinalizeSearch();
						return;
					}
					if (RegionTraverser.ShouldCountRegion(region))
					{
						this.numRegionsProcessed++;
					}
					if (this.numRegionsProcessed >= maxRegions)
					{
						this.FinalizeSearch();
						return;
					}
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							Region region2 = regionLink.regions[j];
							if (region2 != null && region2.closedIndex[this.closedArrayPos] != this.closedIndex && (region2.type & traversableRegionTypes) != RegionType.None && (entryCondition == null || entryCondition(region, region2)))
							{
								this.QueueNewOpenRegion(region2);
							}
						}
					}
				}
				this.FinalizeSearch();
			}

			// Token: 0x04001C35 RID: 7221
			private Queue<Region> open = new Queue<Region>();

			// Token: 0x04001C36 RID: 7222
			private int numRegionsProcessed;

			// Token: 0x04001C37 RID: 7223
			private uint closedIndex = 1U;

			// Token: 0x04001C38 RID: 7224
			private int closedArrayPos;

			// Token: 0x04001C39 RID: 7225
			private const int skippableRegionSize = 4;
		}
	}
}
