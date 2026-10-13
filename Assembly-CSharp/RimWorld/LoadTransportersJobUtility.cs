using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015E9 RID: 5609
	public static class LoadTransportersJobUtility
	{
		// Token: 0x06008466 RID: 33894 RVA: 0x0027AD68 File Offset: 0x00278F68
		public static bool HasJobOnTransporter(Pawn pawn, CompTransporter transporter)
		{
			return transporter.AnythingLeftToLoad && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn.CanReach(transporter.parent, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && LoadTransportersJobUtility.FindThingToLoad(pawn, transporter).Thing != null;
		}

		// Token: 0x06008467 RID: 33895 RVA: 0x0027ADCB File Offset: 0x00278FCB
		public static Job JobOnTransporter(Pawn p, CompTransporter transporter)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, LocalTargetInfo.Invalid, transporter.parent);
			job.ignoreForbidden = true;
			return job;
		}

		// Token: 0x06008468 RID: 33896 RVA: 0x0027ADF0 File Offset: 0x00278FF0
		public static ThingCount FindThingToLoad(Pawn p, CompTransporter transporter)
		{
			LoadTransportersJobUtility.neededThings.Clear();
			List<TransferableOneWay> leftToLoad = transporter.leftToLoad;
			LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
			if (leftToLoad != null)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = transporter.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i] != p && allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
					{
						JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
						if (jobDriver_HaulToTransporter.Container == transporter.parent)
						{
							TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(jobDriver_HaulToTransporter.ThingToCarry, leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
							if (transferableOneWay != null)
							{
								int num = 0;
								if (LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay, out num))
								{
									LoadTransportersJobUtility.tmpAlreadyLoading[transferableOneWay] = num + jobDriver_HaulToTransporter.initialCount;
								}
								else
								{
									LoadTransportersJobUtility.tmpAlreadyLoading.Add(transferableOneWay, jobDriver_HaulToTransporter.initialCount);
								}
							}
						}
					}
				}
				for (int j = 0; j < leftToLoad.Count; j++)
				{
					TransferableOneWay transferableOneWay2 = leftToLoad[j];
					int num2;
					if (!LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(leftToLoad[j], out num2))
					{
						num2 = 0;
					}
					if (transferableOneWay2.CountToTransfer - num2 > 0)
					{
						for (int k = 0; k < transferableOneWay2.things.Count; k++)
						{
							LoadTransportersJobUtility.neededThings.Add(transferableOneWay2.things[k]);
						}
					}
				}
			}
			if (!LoadTransportersJobUtility.neededThings.Any<Thing>())
			{
				LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
				return default(ThingCount);
			}
			Thing thing = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.Touch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => LoadTransportersJobUtility.neededThings.Contains(x) && p.CanReserve(x, 1, -1, null, false) && !x.IsForbidden(p) && p.carryTracker.AvailableStackSpace(x.def) > 0, null, 0, -1, false, RegionType.Set_Passable, false, true);
			if (thing == null)
			{
				foreach (Thing thing2 in LoadTransportersJobUtility.neededThings)
				{
					Pawn pawn = thing2 as Pawn;
					if (pawn != null && pawn.Spawned)
					{
						if ((pawn.IsFreeColonist || pawn.IsColonyMech) && !pawn.Downed)
						{
							Pawn_NeedsTracker needs = pawn.needs;
							bool? flag;
							if (needs == null)
							{
								flag = null;
							}
							else
							{
								Need_MechEnergy energy = needs.energy;
								flag = ((energy != null) ? new bool?(energy.IsLowEnergySelfShutdown) : null);
							}
							bool? flag2 = flag;
							if (!flag2.GetValueOrDefault())
							{
								continue;
							}
						}
						if (!pawn.inventory.UnloadEverything && p.CanReserveAndReach(pawn, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
						{
							LoadTransportersJobUtility.neededThings.Clear();
							LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
							return new ThingCount(pawn, 1, false);
						}
					}
				}
			}
			LoadTransportersJobUtility.neededThings.Clear();
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
				if (!LoadTransportersJobUtility.tmpAlreadyLoading.TryGetValue(transferableOneWay3, out num3))
				{
					num3 = 0;
				}
				LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
				return new ThingCount(thing, Mathf.Min(transferableOneWay3.CountToTransfer - num3, thing.stackCount), false);
			}
			LoadTransportersJobUtility.tmpAlreadyLoading.Clear();
			return default(ThingCount);
		}

		// Token: 0x0400571F RID: 22303
		private static HashSet<Thing> neededThings = new HashSet<Thing>();

		// Token: 0x04005720 RID: 22304
		private static Dictionary<TransferableOneWay, int> tmpAlreadyLoading = new Dictionary<TransferableOneWay, int>();
	}
}
