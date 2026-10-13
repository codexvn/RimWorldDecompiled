using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002967 RID: 10599
	public static class EnterPortalUtility
	{
		// Token: 0x0600E8C0 RID: 59584 RVA: 0x0043C02C File Offset: 0x0043A22C
		public static bool HasJobOnPortal(Pawn pawn, MapPortal portal)
		{
			return portal != null && !portal.leftToLoad.NullOrEmpty<TransferableOneWay>() && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn.CanReach(portal, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && EnterPortalUtility.FindThingToLoad(pawn, portal).Thing != null;
		}

		// Token: 0x0600E8C1 RID: 59585 RVA: 0x0043C094 File Offset: 0x0043A294
		public static Job JobOnPortal(Pawn p, MapPortal portal)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToPortal, LocalTargetInfo.Invalid, portal);
			job.ignoreForbidden = true;
			return job;
		}

		// Token: 0x0600E8C2 RID: 59586 RVA: 0x0043C0B4 File Offset: 0x0043A2B4
		public static ThingCount FindThingToLoad(Pawn p, MapPortal portal)
		{
			EnterPortalUtility.neededThings.Clear();
			List<TransferableOneWay> leftToLoad = portal.leftToLoad;
			EnterPortalUtility.tmpAlreadyLoading.Clear();
			if (leftToLoad != null)
			{
				List<Pawn> list = portal.Map.mapPawns.PawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != p && list[i].CurJobDef == JobDefOf.HaulToPortal)
					{
						JobDriver_HaulToPortal jobDriver_HaulToPortal = (JobDriver_HaulToPortal)list[i].jobs.curDriver;
						if (jobDriver_HaulToPortal.Container == portal)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(jobDriver_HaulToPortal.ThingToCarry, leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = 0;
								if (EnterPortalUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay, out num))
								{
									EnterPortalUtility.tmpAlreadyLoading[transferableOneWay] = num + jobDriver_HaulToPortal.initialCount;
								}
								else
								{
									EnterPortalUtility.tmpAlreadyLoading.Add(transferableOneWay, jobDriver_HaulToPortal.initialCount);
								}
							}
						}
					}
				}
				for (int j = 0; j < leftToLoad.Count; j++)
				{
					TransferableOneWay transferableOneWay2 = leftToLoad[j];
					int num2;
					if (!EnterPortalUtility.tmpAlreadyLoading.TryGetValue(leftToLoad[j], out num2))
					{
						num2 = 0;
					}
					if (transferableOneWay2.CountToTransfer - num2 > 0)
					{
						for (int k = 0; k < transferableOneWay2.things.Count; k++)
						{
							EnterPortalUtility.neededThings.Add(transferableOneWay2.things[k]);
						}
					}
				}
			}
			if (!EnterPortalUtility.neededThings.Any<Thing>())
			{
				EnterPortalUtility.tmpAlreadyLoading.Clear();
				return default(ThingCount);
			}
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.Touch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => EnterPortalUtility.neededThings.Contains(x) && p.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(p) && p.carryTracker.AvailableStackSpace(x.def) > 0, null, 0, -1, false, RegionType.Set_Passable, false, true);
			if (thing == null)
			{
				foreach (Thing thing2 in EnterPortalUtility.neededThings)
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && ((!pawn.IsColonist && !pawn.IsColonyMech) || pawn.Downed || pawn.IsSelfShutdown()) && !pawn.inventory.UnloadEverything && p.CanReserveAndReach(pawn, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						EnterPortalUtility.neededThings.Clear();
						EnterPortalUtility.tmpAlreadyLoading.Clear();
						return new ThingCount(pawn, 1, false);
					}
				}
			}
			EnterPortalUtility.neededThings.Clear();
			if (thing != null)
			{
				TransferableOneWay transferableOneWay3 = null;
				for (int l = 0; l < leftToLoad.Count; l++)
				{
					if (leftToLoad[l].things.Contains(thing))
					{
						transferableOneWay3 = leftToLoad[l];
						break;
					}
				}
				int num3;
				if (!EnterPortalUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay3, out num3))
				{
					num3 = 0;
				}
				EnterPortalUtility.tmpAlreadyLoading.Clear();
				return new ThingCount(thing, Mathf.Min(transferableOneWay3.CountToTransfer - num3, thing.stackCount), false);
			}
			EnterPortalUtility.tmpAlreadyLoading.Clear();
			return default(ThingCount);
		}

		// Token: 0x0600E8C3 RID: 59587 RVA: 0x0043C400 File Offset: 0x0043A600
		public static IEnumerable<Thing> ThingsBeingHauledTo(MapPortal portal)
		{
			IReadOnlyList<Pawn> pawns = portal.Map.mapPawns.AllPawnsSpawned;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].CurJobDef == JobDefOf.HaulToPortal && ((JobDriver_HaulToPortal)pawns[i].jobs.curDriver).MapPortal == portal && pawns[i].carryTracker.CarriedThing != null)
				{
					yield return pawns[i].carryTracker.CarriedThing;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600E8C4 RID: 59588 RVA: 0x0043C410 File Offset: 0x0043A610
		public static void MakeLordsAsAppropriate(List<Pawn> pawns, MapPortal portal)
		{
			Lord lord = null;
			IEnumerable<Pawn> enumerable = pawns.Where(delegate(Pawn x)
			{
				if ((x.IsColonist || x.IsColonyMechPlayerControlled) && !x.Downed)
				{
					Pawn_NeedsTracker needs = x.needs;
					bool flag;
					if (needs == null)
					{
						flag = true;
					}
					else
					{
						Need_MechEnergy energy = needs.energy;
						bool? flag2 = (energy != null) ? new bool?(energy.IsSelfShutdown) : null;
						bool flag3 = true;
						flag = !(flag2.GetValueOrDefault() == flag3 & flag2 != null);
					}
					if (flag)
					{
						return x.Spawned;
					}
				}
				return false;
			});
			if (enumerable.Any<Pawn>())
			{
				lord = portal.Map.lordManager.lords.Find((Lord x) => x.LordJob is LordJob_LoadAndEnterPortal && ((LordJob_LoadAndEnterPortal)x.LordJob).portal == portal);
				if (lord == null)
				{
					lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterPortal(portal), portal.Map, null);
				}
				foreach (Pawn pawn in enumerable)
				{
					if (!lord.ownedPawns.Contains(pawn))
					{
						Lord lord2 = pawn.GetLord();
						if (lord2 != null)
						{
							lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
						}
						lord.AddPawn(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				for (int i = lord.ownedPawns.Count - 1; i >= 0; i--)
				{
					if (!enumerable.Contains(lord.ownedPawns[i]))
					{
						lord.Notify_PawnLost(lord.ownedPawns[i], PawnLostCondition.LordRejected, null);
					}
				}
			}
			for (int j = portal.Map.lordManager.lords.Count - 1; j >= 0; j--)
			{
				LordJob_LoadAndEnterPortal lordJob_LoadAndEnterPortal = portal.Map.lordManager.lords[j].LordJob as LordJob_LoadAndEnterPortal;
				if (lordJob_LoadAndEnterPortal != null && lordJob_LoadAndEnterPortal.portal == portal && portal.Map.lordManager.lords[j] != lord)
				{
					portal.Map.lordManager.RemoveLord(portal.Map.lordManager.lords[j]);
				}
			}
		}

		// Token: 0x0600E8C5 RID: 59589 RVA: 0x0043C634 File Offset: 0x0043A834
		public static bool WasLoadingCanceled(Thing thing)
		{
			MapPortal mapPortal = thing as MapPortal;
			return mapPortal != null && !mapPortal.LoadInProgress;
		}

		// Token: 0x04009B01 RID: 39681
		private static HashSet<Thing> neededThings = new HashSet<Thing>();

		// Token: 0x04009B02 RID: 39682
		private static Dictionary<TransferableOneWay, int> tmpAlreadyLoading = new Dictionary<TransferableOneWay, int>();
	}
}
