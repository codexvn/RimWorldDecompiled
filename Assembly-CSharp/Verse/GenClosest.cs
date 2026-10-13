using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CEA RID: 3306
	public static class GenClosest
	{
		// Token: 0x06005471 RID: 21617 RVA: 0x001B2964 File Offset: 0x001B0B64
		private static bool EarlyOutSearch(IntVec3 start, Map map, ThingRequest thingReq, IEnumerable<Thing> customGlobalSearchSet, Predicate<Thing> validator)
		{
			if (thingReq.group == ThingRequestGroup.Everything)
			{
				Log.Error("Cannot do ClosestThingReachable searching everything without restriction.");
				return true;
			}
			if (!start.InBounds(map))
			{
				string str = "Did FindClosestThing with start out of bounds (";
				IntVec3 intVec = start;
				string str2 = intVec.ToString();
				string str3 = "), thingReq=";
				ThingRequest thingRequest = thingReq;
				Log.Error(str + str2 + str3 + thingRequest.ToString());
				return true;
			}
			return thingReq.group == ThingRequestGroup.Nothing || ((thingReq.IsUndefined || map.listerThings.ThingsMatching(thingReq).Count == 0) && customGlobalSearchSet.EnumerableNullOrEmpty<Thing>());
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x001B29F8 File Offset: 0x001B0BF8
		public static Thing ClosestThingReachable(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, IEnumerable<Thing> customGlobalSearchSet = null, int searchRegionsMin = 0, int searchRegionsMax = -1, bool forceAllowGlobalSearch = false, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false, bool lookInHaulSources = false)
		{
			GenClosest.<>c__DisplayClass2_0 CS$<>8__locals1 = new GenClosest.<>c__DisplayClass2_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.root = root;
			CS$<>8__locals1.peMode = peMode;
			CS$<>8__locals1.traverseParams = traverseParams;
			CS$<>8__locals1.validator = validator;
			bool flag = searchRegionsMax < 0 || forceAllowGlobalSearch;
			if (!flag && customGlobalSearchSet != null)
			{
				Log.ErrorOnce("searchRegionsMax >= 0 && customGlobalSearchSet != null && !forceAllowGlobalSearch. customGlobalSearchSet will never be used.", 634984);
			}
			if (!flag && !thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThingReachable with thing request group " + thingReq.group.ToString() + " and global search not allowed. This will never find anything because this group is never stored in regions. Either allow global search or don't call this method at all.", 518498981);
				return null;
			}
			if (GenClosest.EarlyOutSearch(CS$<>8__locals1.root, CS$<>8__locals1.map, thingReq, customGlobalSearchSet, CS$<>8__locals1.validator))
			{
				return null;
			}
			Thing thing = null;
			bool flag2 = false;
			if (!thingReq.IsUndefined && thingReq.CanBeFoundInRegion)
			{
				int num = (searchRegionsMax > 0) ? searchRegionsMax : 30;
				int num2;
				thing = GenClosest.RegionwiseBFSWorker(CS$<>8__locals1.root, CS$<>8__locals1.map, thingReq, CS$<>8__locals1.peMode, CS$<>8__locals1.traverseParams, CS$<>8__locals1.validator, null, searchRegionsMin, num, maxDistance, out num2, traversableRegionTypes, ignoreEntirelyForbiddenRegions, lookInHaulSources);
				flag2 = (thing == null && num2 < num);
			}
			if (thing == null && flag && !flag2)
			{
				if (traversableRegionTypes != RegionType.Set_Passable)
				{
					Log.ErrorOnce("ClosestThingReachable had to do a global search, but traversableRegionTypes is not set to passable only. It's not supported, because Reachability is based on passable regions only.", 14384767);
				}
				IEnumerable<Thing> searchSet = customGlobalSearchSet ?? CS$<>8__locals1.map.listerThings.ThingsMatching(thingReq);
				thing = GenClosest.ClosestThing_Global(CS$<>8__locals1.root, searchSet, maxDistance, new Predicate<Thing>(CS$<>8__locals1.<ClosestThingReachable>g__GlobalValidator|0), null, false);
			}
			return thing;
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x001B2B6C File Offset: 0x001B0D6C
		public static Thing ClosestThing_Regionwise_ReachablePrioritized(IntVec3 root, Map map, ThingRequest thingReq, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, int minRegions = 24, int maxRegions = 30, bool lookInHaulSources = false)
		{
			if (!thingReq.IsUndefined && !thingReq.CanBeFoundInRegion)
			{
				Log.ErrorOnce("ClosestThing_Regionwise_ReachablePrioritized with thing request group " + thingReq.group.ToString() + ". This will never find anything because this group is never stored in regions. Most likely a global search should have been used.", 738476712);
				return null;
			}
			if (GenClosest.EarlyOutSearch(root, map, thingReq, null, validator))
			{
				return null;
			}
			if (maxRegions < minRegions)
			{
				Log.ErrorOnce("maxRegions < minRegions", 754343);
			}
			Thing result = null;
			if (!thingReq.IsUndefined)
			{
				int num;
				result = GenClosest.RegionwiseBFSWorker(root, map, thingReq, peMode, traverseParams, validator, priorityGetter, minRegions, maxRegions, maxDistance, out num, RegionType.Set_Passable, false, lookInHaulSources);
			}
			return result;
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x001B2C08 File Offset: 0x001B0E08
		public static Thing RegionwiseBFSWorker(IntVec3 root, Map map, ThingRequest req, PathEndMode peMode, TraverseParms traverseParams, Predicate<Thing> validator, Func<Thing, float> priorityGetter, int minRegions, int maxRegions, float maxDistance, out int regionsSeen, RegionType traversableRegionTypes = RegionType.Set_Passable, bool ignoreEntirelyForbiddenRegions = false, bool lookInHaulSources = false)
		{
			regionsSeen = 0;
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThings. Use ClosestThingGlobal.");
				return null;
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyablePlayerOwnedThings. Use ClosestThingGlobal.");
				return null;
			}
			if (traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
			{
				Log.Error("RegionwiseBFSWorker with traverseParams.mode PassAllDestroyableThingsNotWater. Use ClosestThingGlobal.");
				return null;
			}
			if (!req.IsUndefined && !req.CanBeFoundInRegion)
			{
				Log.ErrorOnce("RegionwiseBFSWorker with thing request group " + req.group.ToString() + ". This group is never stored in regions. Most likely a global search should have been used.", 385766189);
				return null;
			}
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionProcessorClosestThingReachable regionProcessorClosestThingReachable = SimplePool<RegionProcessorClosestThingReachable>.Get();
			regionProcessorClosestThingReachable.SetParameters(traverseParams, maxDistance, root, ignoreEntirelyForbiddenRegions, req, peMode, priorityGetter, validator, minRegions, 9999999f, 0, float.MinValue, null, lookInHaulSources);
			RegionTraverser.BreadthFirstTraverse(region, regionProcessorClosestThingReachable, maxRegions, traversableRegionTypes);
			regionsSeen = regionProcessorClosestThingReachable.regionsSeenScan;
			Thing closestThing = regionProcessorClosestThingReachable.closestThing;
			regionProcessorClosestThingReachable.Clear();
			SimplePool<RegionProcessorClosestThingReachable>.Return(regionProcessorClosestThingReachable);
			return closestThing;
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x001B2CF8 File Offset: 0x001B0EF8
		public static Thing ClosestThing_Global(IntVec3 center, IEnumerable searchSet, float maxDistance = 99999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, bool lookInHaulSources = false)
		{
			GenClosest.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.priorityGetter = priorityGetter;
			CS$<>8__locals1.lookInHaulSources = lookInHaulSources;
			CS$<>8__locals1.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			CS$<>8__locals1.closestDistSquared = 2.1474836E+09f;
			CS$<>8__locals1.chosen = null;
			CS$<>8__locals1.bestPrio = float.MinValue;
			CS$<>8__locals1.maxDistanceSquared = maxDistance * maxDistance;
			IList<Thing> list = searchSet as IList<Thing>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global>g__Process|5_0(list[i], ref CS$<>8__locals1);
				}
			}
			else
			{
				IList<Pawn> list2 = searchSet as IList<Pawn>;
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						GenClosest.<ClosestThing_Global>g__Process|5_0(list2[j], ref CS$<>8__locals1);
					}
				}
				else
				{
					IList<Building> list3 = searchSet as IList<Building>;
					if (list3 != null)
					{
						for (int k = 0; k < list3.Count; k++)
						{
							GenClosest.<ClosestThing_Global>g__Process|5_0(list3[k], ref CS$<>8__locals1);
						}
					}
					else
					{
						IList<IAttackTarget> list4 = searchSet as IList<IAttackTarget>;
						if (list4 != null)
						{
							for (int l = 0; l < list4.Count; l++)
							{
								GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)list4[l], ref CS$<>8__locals1);
							}
						}
						else
						{
							foreach (object obj in searchSet)
							{
								GenClosest.<ClosestThing_Global>g__Process|5_0((Thing)obj, ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			return CS$<>8__locals1.chosen;
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x001B2E78 File Offset: 0x001B1078
		public static Thing ClosestThing_Global_Reachable(IntVec3 center, Map map, IEnumerable<Thing> searchSet, PathEndMode peMode, TraverseParms traverseParams, float maxDistance = 9999f, Predicate<Thing> validator = null, Func<Thing, float> priorityGetter = null, bool canLookInHaulableSources = false)
		{
			GenClosest.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.priorityGetter = priorityGetter;
			CS$<>8__locals1.canLookInHaulableSources = canLookInHaulableSources;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.peMode = peMode;
			CS$<>8__locals1.traverseParams = traverseParams;
			CS$<>8__locals1.validator = validator;
			if (searchSet == null)
			{
				return null;
			}
			CS$<>8__locals1.debug_changeCount = 0;
			CS$<>8__locals1.debug_scanCount = 0;
			CS$<>8__locals1.bestThing = null;
			CS$<>8__locals1.bestPrio = float.MinValue;
			CS$<>8__locals1.maxDistanceSquared = maxDistance * maxDistance;
			CS$<>8__locals1.closestDistSquared = 2.1474836E+09f;
			CS$<>8__locals1.careAboutHaulSourceEnabled = (CS$<>8__locals1.canLookInHaulableSources && CS$<>8__locals1.traverseParams.pawn != null && CS$<>8__locals1.traverseParams.pawn.IsColonist);
			IList<Thing> list = searchSet as IList<Thing>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list[i], ref CS$<>8__locals1);
				}
			}
			else
			{
				IList<Pawn> list2 = searchSet as IList<Pawn>;
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list2[j], ref CS$<>8__locals1);
					}
				}
				else
				{
					IList<Building> list3 = searchSet as IList<Building>;
					if (list3 != null)
					{
						for (int k = 0; k < list3.Count; k++)
						{
							GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(list3[k], ref CS$<>8__locals1);
						}
					}
					else
					{
						foreach (Thing t in searchSet)
						{
							GenClosest.<ClosestThing_Global_Reachable>g__Process|6_0(t, ref CS$<>8__locals1);
						}
					}
				}
			}
			return CS$<>8__locals1.bestThing;
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x001B3008 File Offset: 0x001B1208
		[CompilerGenerated]
		internal static void <ClosestThing_Global>g__Process|5_0(Thing t, ref GenClosest.<>c__DisplayClass5_0 A_1)
		{
			if (!t.Spawned && !HaulAIUtility.IsInHaulableInventory(t))
			{
				return;
			}
			float num = (float)(A_1.center - t.PositionHeld).LengthHorizontalSquared;
			if (num > A_1.maxDistanceSquared)
			{
				return;
			}
			if (A_1.priorityGetter != null || num < A_1.closestDistSquared)
			{
				GenClosest.<ClosestThing_Global>g__ValidateThing|5_1(t, num, ref A_1);
				if (A_1.lookInHaulSources)
				{
					IHaulSource haulSource = t as IHaulSource;
					if (haulSource != null)
					{
						ThingOwner directlyHeldThings = haulSource.GetDirectlyHeldThings();
						for (int i = 0; i < directlyHeldThings.Count; i++)
						{
							GenClosest.<ClosestThing_Global>g__ValidateThing|5_1(directlyHeldThings[i], num, ref A_1);
						}
					}
				}
			}
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x001B30A4 File Offset: 0x001B12A4
		[CompilerGenerated]
		internal static void <ClosestThing_Global>g__ValidateThing|5_1(Thing t, float distSquared, ref GenClosest.<>c__DisplayClass5_0 A_2)
		{
			if (A_2.validator != null && !A_2.validator(t))
			{
				return;
			}
			float num = 0f;
			if (A_2.priorityGetter != null)
			{
				num = A_2.priorityGetter(t);
				if (num < A_2.bestPrio)
				{
					return;
				}
				if (Mathf.Approximately(num, A_2.bestPrio) && distSquared >= A_2.closestDistSquared)
				{
					return;
				}
			}
			A_2.chosen = t;
			A_2.closestDistSquared = distSquared;
			A_2.bestPrio = num;
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x001B311C File Offset: 0x001B131C
		[CompilerGenerated]
		internal static void <ClosestThing_Global_Reachable>g__Process|6_0(Thing t, ref GenClosest.<>c__DisplayClass6_0 A_1)
		{
			if (t == null)
			{
				return;
			}
			if (!t.Spawned)
			{
				return;
			}
			int debug_scanCount = A_1.debug_scanCount;
			A_1.debug_scanCount = debug_scanCount + 1;
			float num = (float)(A_1.center - t.PositionHeld).LengthHorizontalSquared;
			if (num > A_1.maxDistanceSquared)
			{
				return;
			}
			if (A_1.priorityGetter != null || num < A_1.closestDistSquared)
			{
				GenClosest.<ClosestThing_Global_Reachable>g__ValidateThing|6_1(t, num, ref A_1);
				if (A_1.canLookInHaulableSources)
				{
					IHaulSource haulSource = t as IHaulSource;
					if (haulSource != null)
					{
						if (A_1.careAboutHaulSourceEnabled && !haulSource.HaulSourceEnabled)
						{
							return;
						}
						ThingOwner directlyHeldThings = haulSource.GetDirectlyHeldThings();
						for (int i = 0; i < directlyHeldThings.Count; i++)
						{
							GenClosest.<ClosestThing_Global_Reachable>g__ValidateThing|6_1(directlyHeldThings[i], num, ref A_1);
						}
					}
				}
			}
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x001B31D8 File Offset: 0x001B13D8
		[CompilerGenerated]
		internal static void <ClosestThing_Global_Reachable>g__ValidateThing|6_1(Thing t, float distSquared, ref GenClosest.<>c__DisplayClass6_0 A_2)
		{
			if (!A_2.map.reachability.CanReach(A_2.center, t.SpawnedParentOrMe, A_2.peMode, A_2.traverseParams))
			{
				return;
			}
			if (A_2.validator != null && !A_2.validator(t))
			{
				return;
			}
			float num = 0f;
			if (A_2.priorityGetter != null)
			{
				num = A_2.priorityGetter(t);
				if (num < A_2.bestPrio)
				{
					return;
				}
				if (Mathf.Approximately(num, A_2.bestPrio) && distSquared >= A_2.closestDistSquared)
				{
					return;
				}
			}
			A_2.bestThing = t;
			A_2.closestDistSquared = distSquared;
			A_2.bestPrio = num;
			int debug_changeCount = A_2.debug_changeCount;
			A_2.debug_changeCount = debug_changeCount + 1;
		}

		// Token: 0x04003CD5 RID: 15573
		private const int DefaultLocalTraverseRegionsBeforeGlobal = 30;
	}
}
