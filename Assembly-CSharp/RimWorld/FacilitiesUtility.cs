using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003633 RID: 13875
	public static class FacilitiesUtility
	{
		// Token: 0x06013447 RID: 78919 RVA: 0x005B7624 File Offset: 0x005B5824
		public static void NotifyFacilitiesAboutChangedLOSBlockers(List<Region> affectedRegions)
		{
			if (!affectedRegions.Any<Region>())
			{
				return;
			}
			if (FacilitiesUtility.working)
			{
				Log.Warning("Tried to update facilities while already updating.");
				return;
			}
			FacilitiesUtility.working = true;
			try
			{
				FacilitiesUtility.visited.Clear();
				FacilitiesUtility.processed.Clear();
				int facilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.Facility).Count;
				int affectedByFacilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.AffectedByFacilities).Count;
				int facilitiesProcessed = 0;
				int affectedByFacilitiesProcessed = 0;
				if (facilitiesToProcess > 0 && affectedByFacilitiesToProcess > 0)
				{
					RegionProcessor <>9__1;
					for (int i = 0; i < affectedRegions.Count; i++)
					{
						if (!FacilitiesUtility.visited.Contains(affectedRegions[i]))
						{
							Region root = affectedRegions[i];
							RegionEntryPredicate entryCondition = (Region from, Region r) => !FacilitiesUtility.visited.Contains(r);
							RegionProcessor regionProcessor;
							if ((regionProcessor = <>9__1) == null)
							{
								regionProcessor = (<>9__1 = delegate(Region x)
								{
									FacilitiesUtility.visited.Add(x);
									List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
									for (int j = 0; j < list.Count; j++)
									{
										if (!FacilitiesUtility.processed.Contains(list[j]))
										{
											FacilitiesUtility.processed.Add(list[j]);
											CompFacility compFacility = list[j].TryGetComp<CompFacility>();
											CompAffectedByFacilities compAffectedByFacilities = list[j].TryGetComp<CompAffectedByFacilities>();
											if (compFacility != null)
											{
												compFacility.Notify_LOSBlockerSpawnedOrDespawned();
												int num = facilitiesProcessed;
												facilitiesProcessed = num + 1;
											}
											if (compAffectedByFacilities != null)
											{
												compAffectedByFacilities.Notify_LOSBlockerSpawnedOrDespawned();
												int num = affectedByFacilitiesProcessed;
												affectedByFacilitiesProcessed = num + 1;
											}
										}
									}
									return facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess;
								});
							}
							RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, FacilitiesUtility.RegionsToSearch, RegionType.Set_Passable);
							if (facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess)
							{
								break;
							}
						}
					}
				}
			}
			finally
			{
				FacilitiesUtility.working = false;
				FacilitiesUtility.visited.Clear();
				FacilitiesUtility.processed.Clear();
			}
		}

		// Token: 0x0400D5D1 RID: 54737
		private const float MaxDistToLinkToFacilityEver = 10f;

		// Token: 0x0400D5D2 RID: 54738
		private static int RegionsToSearch = (1 + 2 * Mathf.CeilToInt(0.8333333f)) * (1 + 2 * Mathf.CeilToInt(0.8333333f));

		// Token: 0x0400D5D3 RID: 54739
		private static HashSet<Region> visited = new HashSet<Region>();

		// Token: 0x0400D5D4 RID: 54740
		private static HashSet<Thing> processed = new HashSet<Thing>();

		// Token: 0x0400D5D5 RID: 54741
		private static bool working;
	}
}
