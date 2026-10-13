using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020023A2 RID: 9122
	public class PawnsArrivalModeWorker_SpecificLocationDrop : PawnsArrivalModeWorker
	{
		// Token: 0x0600C585 RID: 50565 RVA: 0x0039769C File Offset: 0x0039589C
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			List<List<Thing>> thingsGroups = (from p in pawns
			select new List<Thing>
			{
				p
			}).ToList<List<Thing>>();
			PawnsArrivalModeWorker_SpecificLocationDrop.DropThingGroupsNear(parms.spawnCenter, map, thingsGroups, parms.dropInRadius, parms.podOpenDelay, false, true, flag || parms.raidArrivalModeForQuickMilitaryAid, true, true, false, parms.faction);
		}

		// Token: 0x0600C586 RID: 50566 RVA: 0x0039772C File Offset: 0x0039592C
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			throw new NotImplementedException("PawnsArrivalModeWorker_SpecificLocationDrop is only for raids");
		}

		// Token: 0x0600C587 RID: 50567 RVA: 0x00397738 File Offset: 0x00395938
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}

		// Token: 0x0600C588 RID: 50568 RVA: 0x00397768 File Offset: 0x00395968
		private static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int radius, int openDelay = 110, bool instaDrop = false, bool leaveSlag = false, bool canRoofPunch = true, bool forbid = true, bool allowFogged = true, bool canTransfer = false, Faction faction = null)
		{
			Predicate<IntVec3> <>9__0;
			foreach (List<Thing> list in thingsGroups)
			{
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, canRoofPunch, radius, true, null, true) && (canRoofPunch || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, allowFogged, true, radius, true, null, true)))
				{
					if (!dropCenter.IsValid)
					{
						continue;
					}
					string[] array = new string[5];
					array[0] = "DropThingsNear failed to find a place to drop ";
					int num = 1;
					Thing thing = list.FirstOrDefault<Thing>();
					array[num] = ((thing != null) ? thing.ToString() : null);
					array[2] = " near ";
					int num2 = 3;
					IntVec3 intVec2 = dropCenter;
					array[num2] = intVec2.ToString();
					array[4] = ". Dropping on random square instead.";
					Log.Warning(string.Concat(array));
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = delegate(IntVec3 c)
						{
							if (c.Walkable(map))
							{
								RoofDef roof = c.GetRoof(map);
								return roof == null || !roof.isThickRoof;
							}
							return false;
						});
					}
					intVec = CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
				if (forbid)
				{
					for (int i = 0; i < list.Count; i++)
					{
						list[i].SetForbidden(true, false);
					}
				}
				if (instaDrop)
				{
					using (List<Thing>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Thing thing2 = enumerator2.Current;
							GenPlace.TryPlaceThing(thing2, intVec, map, ThingPlaceMode.Near, null, null, null, 1);
						}
						continue;
					}
				}
				ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
				foreach (Thing item in list)
				{
					activeTransporterInfo.innerContainer.TryAdd(item, true);
				}
				activeTransporterInfo.openDelay = openDelay;
				activeTransporterInfo.leaveSlag = leaveSlag;
				DropPodUtility.MakeDropPodAt(intVec, map, activeTransporterInfo, faction);
			}
		}

		// Token: 0x040086B6 RID: 34486
		public const int PodOpenDelay = 520;
	}
}
