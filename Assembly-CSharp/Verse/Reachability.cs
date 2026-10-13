using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004E4 RID: 1252
	public class Reachability
	{
		// Token: 0x0600205B RID: 8283 RVA: 0x000A8E7C File Offset: 0x000A707C
		public Reachability(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000A8EC9 File Offset: 0x000A70C9
		public void ClearCache()
		{
			if (this.cache.Count > 0)
			{
				this.cache.Clear();
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000A8EE4 File Offset: 0x000A70E4
		public void ClearCacheFor(Pawn pawn)
		{
			this.cache.ClearFor(pawn);
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000A8EF2 File Offset: 0x000A70F2
		public void ClearCacheForHostile(Thing hostileTo)
		{
			this.cache.ClearForHostile(hostileTo);
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000A8F00 File Offset: 0x000A7100
		private void QueueNewOpenRegion(Region region)
		{
			if (region == null)
			{
				Log.ErrorOnce("Tried to queue null region.", 881121);
				return;
			}
			if (region.reachedIndex == this.reachedIndex)
			{
				Log.ErrorOnce("Region is already reached; you can't open it. Region: " + region.ToString(), 719991);
				return;
			}
			this.openQueue.Enqueue(region);
			region.reachedIndex = this.reachedIndex;
			this.numRegionsOpened++;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000A8F70 File Offset: 0x000A7170
		private uint NewReachedIndex()
		{
			uint num = this.reachedIndex;
			this.reachedIndex = num + 1U;
			return num;
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000A8F8E File Offset: 0x000A718E
		private void FinalizeCheck()
		{
			this.working = false;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000A8F97 File Offset: 0x000A7197
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseMode, maxDanger);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000A8FC5 File Offset: 0x000A71C5
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseParams);
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000A8FF4 File Offset: 0x000A71F4
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return this.CanReach(start, dest, peMode, TraverseParms.For(traverseMode, maxDanger, false, false, false, true, false));
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000A9018 File Offset: 0x000A7218
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			if (this.working)
			{
				Log.ErrorOnce("Called CanReach() while working. This should never happen. Suppressing further errors.", 7312233);
				return false;
			}
			if (traverseParams.pawn != null)
			{
				if (!traverseParams.pawn.Spawned)
				{
					return false;
				}
				if (traverseParams.pawn.Map != this.map)
				{
					string[] array = new string[6];
					array[0] = "Called CanReach() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=";
					int num = 1;
					Pawn pawn = traverseParams.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " pawn.Map=";
					int num2 = 3;
					Map map = traverseParams.pawn.Map;
					array[num2] = ((map != null) ? map.ToString() : null);
					array[4] = " map=";
					int num3 = 5;
					Map map2 = this.map;
					array[num3] = ((map2 != null) ? map2.ToString() : null);
					Log.Error(string.Concat(array));
					return false;
				}
			}
			if (ReachabilityImmediate.CanReachImmediate(start, dest, this.map, peMode, traverseParams.pawn))
			{
				return true;
			}
			if (!dest.IsValid)
			{
				return false;
			}
			if (dest.HasThing && dest.Thing.MapHeld != this.map)
			{
				return false;
			}
			if (!start.InBounds(this.map) || !dest.Cell.InBounds(this.map))
			{
				return false;
			}
			if ((peMode == PathEndMode.OnCell || peMode == PathEndMode.Touch || peMode == PathEndMode.ClosestTouch) && traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
			{
				District district = RegionAndRoomQuery.DistirctAtFast(start, this.map, RegionType.Set_Passable);
				if (district != null && district == RegionAndRoomQuery.DistirctAtFast(dest.Cell, this.map, RegionType.Set_Passable))
				{
					return true;
				}
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings)
			{
				TraverseParms traverseParams2 = traverseParams;
				traverseParams2.mode = TraverseMode.PassDoors;
				if (this.CanReach(start, dest, peMode, traverseParams2))
				{
					return true;
				}
			}
			dest = (LocalTargetInfo)GenPath.ResolvePathMode(traverseParams.pawn, dest.ToTargetInfo(this.map), ref peMode);
			this.working = true;
			bool result;
			try
			{
				this.pathGrid = this.map.pathing.For(traverseParams).pathGrid;
				this.regionGrid = this.map.regionGrid;
				this.reachedIndex += 1U;
				this.destRegions.Clear();
				if (peMode == PathEndMode.OnCell)
				{
					Region region = dest.Cell.GetRegion(this.map, RegionType.Set_Passable);
					if (region != null && region.Allows(traverseParams, true))
					{
						this.destRegions.Add(region);
					}
				}
				else if (peMode == PathEndMode.Touch)
				{
					TouchPathEndModeUtility.AddAllowedAdjacentRegions(dest, traverseParams, this.map, this.destRegions);
				}
				if (this.destRegions.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyablePlayerOwnedThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
				{
					this.FinalizeCheck();
					result = false;
				}
				else
				{
					this.destRegions.RemoveDuplicates(null);
					this.openQueue.Clear();
					this.numRegionsOpened = 0;
					this.DetermineStartRegions(start);
					if (this.openQueue.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyablePlayerOwnedThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
					{
						this.FinalizeCheck();
						result = false;
					}
					else
					{
						if (this.startingRegions.Any<Region>() && this.destRegions.Any<Region>() && Reachability.CanUseCache(traverseParams.mode))
						{
							switch (this.GetCachedResult(traverseParams))
							{
							case BoolUnknown.True:
								this.FinalizeCheck();
								return true;
							case BoolUnknown.False:
								this.FinalizeCheck();
								return false;
							}
						}
						if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater)
						{
							bool flag = this.CheckCellBasedReachability(start, dest, peMode, traverseParams);
							this.FinalizeCheck();
							result = flag;
						}
						else
						{
							bool flag2 = this.CheckRegionBasedReachability(traverseParams);
							this.FinalizeCheck();
							result = flag2;
						}
					}
				}
			}
			finally
			{
				this.working = false;
			}
			return result;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000A93DC File Offset: 0x000A75DC
		private void DetermineStartRegions(IntVec3 start)
		{
			this.startingRegions.Clear();
			if (this.pathGrid.WalkableFast(start))
			{
				Region validRegionAt = this.regionGrid.GetValidRegionAt(start);
				this.QueueNewOpenRegion(validRegionAt);
				this.startingRegions.Add(validRegionAt);
				return;
			}
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = start + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(this.map) && this.pathGrid.WalkableFast(intVec))
				{
					Region validRegionAt2 = this.regionGrid.GetValidRegionAt(intVec);
					if (validRegionAt2 != null && validRegionAt2.reachedIndex != this.reachedIndex)
					{
						this.QueueNewOpenRegion(validRegionAt2);
						this.startingRegions.Add(validRegionAt2);
					}
				}
			}
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000A9490 File Offset: 0x000A7690
		private BoolUnknown GetCachedResult(TraverseParms traverseParams)
		{
			bool flag = false;
			for (int i = 0; i < this.startingRegions.Count; i++)
			{
				for (int j = 0; j < this.destRegions.Count; j++)
				{
					if (this.destRegions[j] == this.startingRegions[i])
					{
						return BoolUnknown.True;
					}
					BoolUnknown boolUnknown = this.cache.CachedResultFor(this.startingRegions[i].District, this.destRegions[j].District, traverseParams);
					if (boolUnknown == BoolUnknown.True)
					{
						return BoolUnknown.True;
					}
					if (boolUnknown == BoolUnknown.Unknown)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.Unknown;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000A9528 File Offset: 0x000A7728
		private bool CheckRegionBasedReachability(TraverseParms traverseParams)
		{
			while (this.openQueue.Count > 0)
			{
				Region region = this.openQueue.Dequeue();
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					for (int j = 0; j < 2; j++)
					{
						Region region2 = regionLink.regions[j];
						if (region2 != null && region2.reachedIndex != this.reachedIndex && region2.type.Passable() && region2.Allows(traverseParams, false))
						{
							if (this.destRegions.Contains(region2))
							{
								for (int k = 0; k < this.startingRegions.Count; k++)
								{
									this.cache.AddCachedResult(this.startingRegions[k].District, region2.District, traverseParams, true);
								}
								return true;
							}
							this.QueueNewOpenRegion(region2);
						}
					}
				}
			}
			for (int l = 0; l < this.startingRegions.Count; l++)
			{
				for (int m = 0; m < this.destRegions.Count; m++)
				{
					this.cache.AddCachedResult(this.startingRegions[l].District, this.destRegions[m].District, traverseParams, false);
				}
			}
			return false;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000A968C File Offset: 0x000A788C
		private bool CheckCellBasedReachability(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			IntVec3 foundCell = IntVec3.Invalid;
			Region[] directRegionGrid = this.regionGrid.DirectGrid;
			PathGrid pathGrid = this.map.pathing.For(traverseParams).pathGrid;
			CellIndices cellIndices = this.map.cellIndices;
			this.map.floodFiller.FloodFill(start, delegate(IntVec3 c)
			{
				int num = cellIndices.CellToIndex(c);
				if ((traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater) && c.GetTerrain(this.map).IsWater)
				{
					return false;
				}
				if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
				{
					if (!pathGrid.WalkableFast(num))
					{
						Building edifice = c.GetEdifice(this.map);
						if (edifice == null || !PathUtility.IsDestroyable(edifice))
						{
							return false;
						}
						if (traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings && edifice.Faction != Faction.OfPlayer)
						{
							return false;
						}
					}
				}
				else
				{
					if (traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater)
					{
						Log.ErrorOnce("Do not use this method for non-cell based modes!", 938476762);
					}
					if (!pathGrid.WalkableFast(num))
					{
						return false;
					}
				}
				Region region = directRegionGrid[num];
				return region == null || region.Allows(traverseParams, false);
			}, delegate(IntVec3 c)
			{
				if (ReachabilityImmediate.CanReachImmediate(c, dest, this.map, peMode, traverseParams.pawn))
				{
					foundCell = c;
					return true;
				}
				return false;
			}, int.MaxValue, false, null);
			if (foundCell.IsValid)
			{
				if (Reachability.CanUseCache(traverseParams.mode))
				{
					Region validRegionAt = this.regionGrid.GetValidRegionAt(foundCell);
					if (validRegionAt != null)
					{
						for (int i = 0; i < this.startingRegions.Count; i++)
						{
							this.cache.AddCachedResult(this.startingRegions[i].District, validRegionAt.District, traverseParams, true);
						}
					}
				}
				return true;
			}
			if (Reachability.CanUseCache(traverseParams.mode))
			{
				for (int j = 0; j < this.startingRegions.Count; j++)
				{
					for (int k = 0; k < this.destRegions.Count; k++)
					{
						this.cache.AddCachedResult(this.startingRegions[j].District, this.destRegions[k].District, traverseParams, false);
					}
				}
			}
			return false;
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000A9827 File Offset: 0x000A7A27
		public bool CanReachColony(IntVec3 c)
		{
			return this.CanReachFactionBase(c, Faction.OfPlayer);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000A9838 File Offset: 0x000A7A38
		public bool CanReachFactionBase(IntVec3 c, Faction factionBaseFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return this.CanReach(c, MapGenerator.PlayerStartSpot, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
			}
			if (!c.Walkable(this.map))
			{
				return false;
			}
			Faction faction = this.map.ParentFaction ?? Faction.OfPlayer;
			List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			if (faction == Faction.OfPlayer)
			{
				List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
				for (int j = 0; j < allBuildingsColonist.Count; j++)
				{
					if (this.CanReach(c, allBuildingsColonist[j], PathEndMode.Touch, traverseParams))
					{
						return true;
					}
				}
			}
			else
			{
				List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int k = 0; k < list2.Count; k++)
				{
					if (list2[k].Faction == faction && this.CanReach(c, list2[k], PathEndMode.Touch, traverseParams))
					{
						return true;
					}
				}
			}
			return this.CanReachBiggestMapEdgeDistrict(c);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000A9988 File Offset: 0x000A7B88
		public bool CanReachBiggestMapEdgeDistrict(IntVec3 c)
		{
			District district = null;
			for (int i = 0; i < this.map.regionGrid.allDistricts.Count; i++)
			{
				District district2 = this.map.regionGrid.allDistricts[i];
				if (district2.TouchesMapEdge && (district == null || district2.RegionCount > district.RegionCount))
				{
					district = district2;
				}
			}
			return district != null && this.CanReach(c, district.Regions[0].AnyCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x000A9A18 File Offset: 0x000A7C18
		public bool CanReachMapEdge(IntVec3 c, TraverseParms traverseParms)
		{
			Reachability.<>c__DisplayClass28_0 CS$<>8__locals1 = new Reachability.<>c__DisplayClass28_0();
			CS$<>8__locals1.traverseParms = traverseParms;
			if (CS$<>8__locals1.traverseParms.pawn != null)
			{
				if (!CS$<>8__locals1.traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (CS$<>8__locals1.traverseParms.pawn.Map != this.map)
				{
					string[] array = new string[6];
					array[0] = "Called CanReachMapEdge() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=";
					int num = 1;
					Pawn pawn = CS$<>8__locals1.traverseParms.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " pawn.Map=";
					int num2 = 3;
					Map map = CS$<>8__locals1.traverseParms.pawn.Map;
					array[num2] = ((map != null) ? map.ToString() : null);
					array[4] = " map=";
					int num3 = 5;
					Map map2 = this.map;
					array[num3] = ((map2 != null) ? map2.ToString() : null);
					Log.Error(string.Concat(array));
					return false;
				}
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			if (region.District.TouchesMapEdge)
			{
				return true;
			}
			CS$<>8__locals1.foundReg = false;
			RegionTraverser.BreadthFirstTraverse(region, new RegionEntryPredicate(CS$<>8__locals1.<CanReachMapEdge>g__EntryCondition|1), new RegionProcessor(CS$<>8__locals1.<CanReachMapEdge>g__RegionProcessor|0), 9999, RegionType.Set_Passable);
			return CS$<>8__locals1.foundReg;
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x000A9B3C File Offset: 0x000A7D3C
		public bool CanReachUnfogged(IntVec3 c, TraverseParms traverseParms)
		{
			Reachability.<>c__DisplayClass29_0 CS$<>8__locals1 = new Reachability.<>c__DisplayClass29_0();
			CS$<>8__locals1.traverseParms = traverseParms;
			CS$<>8__locals1.<>4__this = this;
			if (CS$<>8__locals1.traverseParms.pawn != null)
			{
				if (!CS$<>8__locals1.traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (CS$<>8__locals1.traverseParms.pawn.Map != this.map)
				{
					string[] array = new string[6];
					array[0] = "Called CanReachUnfogged() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=";
					int num = 1;
					Pawn pawn = CS$<>8__locals1.traverseParms.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " pawn.Map=";
					int num2 = 3;
					Map map = CS$<>8__locals1.traverseParms.pawn.Map;
					array[num2] = ((map != null) ? map.ToString() : null);
					array[4] = " map=";
					int num3 = 5;
					Map map2 = this.map;
					array[num3] = ((map2 != null) ? map2.ToString() : null);
					Log.Error(string.Concat(array));
					return false;
				}
			}
			if (!c.InBounds(this.map))
			{
				return false;
			}
			if (!c.Fogged(this.map))
			{
				return true;
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			CS$<>8__locals1.foundReg = false;
			RegionTraverser.BreadthFirstTraverse(region, new RegionEntryPredicate(CS$<>8__locals1.<CanReachUnfogged>g__EntryCondition|0), new RegionProcessor(CS$<>8__locals1.<CanReachUnfogged>g__RegionProcessor|1), 9999, RegionType.Set_Passable);
			return CS$<>8__locals1.foundReg;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x000A9C77 File Offset: 0x000A7E77
		private static bool CanUseCache(TraverseMode mode)
		{
			return mode != TraverseMode.PassAllDestroyableThingsNotWater && mode != TraverseMode.NoPassClosedDoorsOrWater;
		}

		// Token: 0x04001B9D RID: 7069
		private Map map;

		// Token: 0x04001B9E RID: 7070
		private Queue<Region> openQueue = new Queue<Region>();

		// Token: 0x04001B9F RID: 7071
		private List<Region> startingRegions = new List<Region>();

		// Token: 0x04001BA0 RID: 7072
		private List<Region> destRegions = new List<Region>();

		// Token: 0x04001BA1 RID: 7073
		private uint reachedIndex = 1U;

		// Token: 0x04001BA2 RID: 7074
		private int numRegionsOpened;

		// Token: 0x04001BA3 RID: 7075
		private bool working;

		// Token: 0x04001BA4 RID: 7076
		private ReachabilityCache cache = new ReachabilityCache();

		// Token: 0x04001BA5 RID: 7077
		private PathGrid pathGrid;

		// Token: 0x04001BA6 RID: 7078
		private RegionGrid regionGrid;
	}
}
