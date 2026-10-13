using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000FB2 RID: 4018
	public static class TantrumMentalStateUtility
	{
		// Token: 0x060062B1 RID: 25265 RVA: 0x001F19F8 File Offset: 0x001EFBF8
		public static bool CanSmash(Pawn pawn, Thing thing, bool skipReachabilityCheck = false, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0)
		{
			if (customValidator != null)
			{
				if (!customValidator(thing))
				{
					return false;
				}
			}
			else if (!thing.def.IsBuildingArtificial && thing.def.category != ThingCategory.Item)
			{
				return false;
			}
			return !thing.Destroyed && thing.Spawned && thing != pawn && (thing.def.category == ThingCategory.Pawn || thing.def.useHitPoints) && (thing.def.category == ThingCategory.Pawn || !thing.def.CanHaveFaction || thing.Faction == pawn.Faction) && (thing.def.category != ThingCategory.Item || thing.MarketValue * (float)thing.stackCount >= 75f) && (thing.def.category != ThingCategory.Pawn || !((Pawn)thing).Downed) && ((thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Building) || thing.MarketValue * (float)thing.stackCount >= (float)extraMinBuildingOrItemMarketValue) && (thing.def.category != ThingCategory.Building || !thing.def.building.ai_neverTrashThis) && (skipReachabilityCheck || pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x060062B2 RID: 25266 RVA: 0x001F1B40 File Offset: 0x001EFD40
		public static void GetSmashableThingsNear(Pawn pawn, IntVec3 near, List<Thing> outCandidates, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0, int maxDistance = 40)
		{
			outCandidates.Clear();
			if (!pawn.CanReach(near, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return;
			}
			Region region = near.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list[i], true, customValidator, extraMinBuildingOrItemMarketValue))
					{
						outCandidates.Add(list[i]);
					}
				}
				List<Thing> list2 = r.ListerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int j = 0; j < list2.Count; j++)
				{
					if (list2[j].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list2[j], true, customValidator, extraMinBuildingOrItemMarketValue))
					{
						outCandidates.Add(list2[j]);
					}
				}
				List<Thing> list3 = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int k = 0; k < list3.Count; k++)
				{
					if (list3[k].Position.InHorDistOf(near, (float)maxDistance) && TantrumMentalStateUtility.CanSmash(pawn, list3[k], true, customValidator, extraMinBuildingOrItemMarketValue))
					{
						outCandidates.Add(list3[k]);
					}
				}
				return false;
			}, 40, RegionType.Set_Passable);
		}

		// Token: 0x060062B3 RID: 25267 RVA: 0x001F1C00 File Offset: 0x001EFE00
		public static void GetSmashableThingsIn(Room room, Pawn pawn, List<Thing> outCandidates, Predicate<Thing> customValidator = null, int extraMinBuildingOrItemMarketValue = 0)
		{
			outCandidates.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (TantrumMentalStateUtility.CanSmash(pawn, containedAndAdjacentThings[i], false, customValidator, extraMinBuildingOrItemMarketValue))
				{
					outCandidates.Add(containedAndAdjacentThings[i]);
				}
			}
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x001F1C4C File Offset: 0x001EFE4C
		public static bool CanAttackPrisoner(Pawn pawn, Thing prisoner)
		{
			Pawn pawn2 = prisoner as Pawn;
			return pawn2 != null && pawn2.IsPrisoner && !pawn2.Downed && pawn2.HostFaction == pawn.Faction;
		}

		// Token: 0x0400468C RID: 18060
		private const int MaxRegionsToSearch = 40;

		// Token: 0x0400468D RID: 18061
		private const int AbsoluteMinItemMarketValue = 75;
	}
}
