using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200155E RID: 5470
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		// Token: 0x060081D3 RID: 33235 RVA: 0x00075162 File Offset: 0x00073362
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060081D4 RID: 33236 RVA: 0x00270FF1 File Offset: 0x0026F1F1
		public static void ResetStaticData()
		{
			WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated = "ForbiddenLower".Translate();
			WorkGiver_ConstructDeliverResources.NoPathTranslated = "NoPath".Translate();
		}

		// Token: 0x060081D5 RID: 33237 RVA: 0x0027101B File Offset: 0x0026F21B
		private static bool ResourceValidator(Pawn pawn, ThingDefCountClass need, Thing th)
		{
			return th.def == need.thingDef && !th.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, th, false);
		}

		// Token: 0x060081D6 RID: 33238 RVA: 0x00271048 File Offset: 0x0026F248
		private bool CanUseCarriedResource(Pawn pawn, IConstructible c, ThingDefCountClass need)
		{
			WorkGiver_ConstructDeliverResources.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.pawn = pawn;
			Thing carriedThing = CS$<>8__locals1.pawn.carryTracker.CarriedThing;
			if (((carriedThing != null) ? carriedThing.def : null) != need.thingDef)
			{
				return false;
			}
			if (!KeyBindingDefOf.QueueOrder.IsDownEvent)
			{
				return true;
			}
			if (CS$<>8__locals1.pawn.CurJob != null && !WorkGiver_ConstructDeliverResources.<CanUseCarriedResource>g__IsValidJob|9_0(CS$<>8__locals1.pawn.CurJob, ref CS$<>8__locals1))
			{
				return false;
			}
			using (IEnumerator<QueuedJob> enumerator = CS$<>8__locals1.pawn.jobs.jobQueue.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!WorkGiver_ConstructDeliverResources.<CanUseCarriedResource>g__IsValidJob|9_0(enumerator.Current.job, ref CS$<>8__locals1))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x0027110C File Offset: 0x0026F30C
		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true, bool forced = false)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return this.InstallJob(pawn, blueprint_Install);
			}
			WorkGiver_ConstructDeliverResources.missingResources.Clear();
			using (List<ThingDefCountClass>.Enumerator enumerator = c.TotalMaterialCost().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDefCountClass need = enumerator.Current;
					if (forced)
					{
						goto IL_92;
					}
					IHaulEnroute haulEnroute = c as IHaulEnroute;
					if (haulEnroute == null)
					{
						goto IL_92;
					}
					int num = haulEnroute.GetSpaceRemainingWithEnroute(need.thingDef, pawn);
					IL_A5:
					if (num <= 0)
					{
						continue;
					}
					if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need.thingDef, num, pawn))
					{
						WorkGiver_ConstructDeliverResources.missingResources.Add(need.thingDef, num);
						if (FloatMenuMakerMap.makingFor != pawn)
						{
							break;
						}
						continue;
					}
					else
					{
						Thing foundRes;
						if (this.CanUseCarriedResource(pawn, c, need))
						{
							foundRes = pawn.carryTracker.CarriedThing;
						}
						else
						{
							foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator(pawn, need, r), null, 0, -1, false, RegionType.Set_Passable, false, false);
						}
						if (foundRes == null)
						{
							WorkGiver_ConstructDeliverResources.missingResources.Add(need.thingDef, num);
							if (FloatMenuMakerMap.makingFor != pawn)
							{
								break;
							}
							continue;
						}
						else
						{
							int num2;
							this.FindAvailableNearbyResources(foundRes, pawn, out num2);
							int num3;
							Job job;
							HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need.thingDef, c, num, num2, canRemoveExistingFloorUnderNearbyNeeders, out num3, out job);
							if (job != null)
							{
								return job;
							}
							hashSet.Add((Thing)c);
							Thing thing;
							if (hashSet.Count > 0)
							{
								thing = hashSet.MinBy((Thing needer) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, needer.Position));
								hashSet.Remove(thing);
							}
							else
							{
								thing = (Thing)c;
							}
							int num4 = 0;
							int i = 0;
							do
							{
								num4 += WorkGiver_ConstructDeliverResources.resourcesAvailable[i].stackCount;
								num4 = Mathf.Min(num4, Mathf.Min(num2, num3));
								i++;
							}
							while (num4 < num3 && num4 < num2 && i < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count);
							WorkGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(i, WorkGiver_ConstructDeliverResources.resourcesAvailable.Count - i);
							WorkGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
							Job job2 = JobMaker.MakeJob(JobDefOf.HaulToContainer);
							job2.targetA = foundRes;
							job2.targetQueueA = new List<LocalTargetInfo>();
							for (i = 0; i < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count; i++)
							{
								job2.targetQueueA.Add(WorkGiver_ConstructDeliverResources.resourcesAvailable[i]);
							}
							job2.targetC = (Thing)c;
							job2.targetB = thing;
							if (hashSet.Count > 0)
							{
								job2.targetQueueB = new List<LocalTargetInfo>();
								foreach (Thing t in hashSet)
								{
									job2.targetQueueB.Add(t);
								}
							}
							job2.count = num4;
							job2.haulMode = HaulMode.ToContainer;
							return job2;
						}
					}
					IL_92:
					num = c.ThingCountNeeded(need.thingDef);
					goto IL_A5;
				}
			}
			if (WorkGiver_ConstructDeliverResources.missingResources.Count > 0 && FloatMenuMakerMap.makingFor == pawn)
			{
				JobFailReason.Is("MissingMaterials".Translate((from kvp in WorkGiver_ConstructDeliverResources.missingResources
				select string.Format("{0}x {1}", kvp.Value, kvp.Key.label)).ToCommaList(false, false)), null);
			}
			return null;
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x002715BC File Offset: 0x0026F7BC
		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def);
			resTotalAvailable = 0;
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(firstFoundResource.PositionHeld, firstFoundResource.MapHeld, 5f, false))
				{
					if (resTotalAvailable >= num)
					{
						resTotalAvailable = num;
						break;
					}
					if (thing.def == firstFoundResource.def && GenAI.CanUseItemForWork(pawn, thing))
					{
						WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(thing);
						resTotalAvailable += thing.stackCount;
					}
				}
			}
			resTotalAvailable = Mathf.Min(resTotalAvailable, num);
		}

		// Token: 0x060081D9 RID: 33241 RVA: 0x00271690 File Offset: 0x0026F890
		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingDef stuff, IConstructible c, int resNeeded, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = resNeeded;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, true))
			{
				if (neededTotal >= resTotalAvailable)
				{
					break;
				}
				if (this.IsNewValidNearbyNeeder(thing2, hashSet, c, pawn))
				{
					Blueprint blueprint = thing2 as Blueprint;
					if (blueprint == null || !WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						int num = 0;
						IHaulEnroute haulEnroute = thing2 as IHaulEnroute;
						if (haulEnroute != null)
						{
							num = haulEnroute.GetSpaceRemainingWithEnroute(stuff, pawn);
						}
						else
						{
							IConstructible constructible = thing2 as IConstructible;
							if (constructible != null)
							{
								num = constructible.ThingCountNeeded(stuff);
							}
						}
						if (num > 0)
						{
							hashSet.Add(thing2);
							neededTotal += num;
						}
					}
				}
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing thing3 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(thing3, hashSet, c, pawn))
					{
						Blueprint blueprint3 = thing3 as Blueprint;
						if (blueprint3 != null)
						{
							Job job = this.RemoveExistingFloorJob(pawn, blueprint3);
							if (job != null)
							{
								jobToMakeNeederAvailable = job;
								return hashSet;
							}
						}
					}
				}
			}
			jobToMakeNeederAvailable = null;
			return hashSet;
		}

		// Token: 0x060081DA RID: 33242 RVA: 0x0027182C File Offset: 0x0026FA2C
		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			return t is IConstructible && t != constructible && t.Faction == pawn.Faction && t.Isnt<Blueprint_Install>() && !nearbyNeeders.Contains(t) && !t.IsForbidden(pawn) && GenConstruct.CanConstruct(t, pawn, false, false, JobDefOf.HaulToContainer);
		}

		// Token: 0x060081DB RID: 33243 RVA: 0x00271880 File Offset: 0x0026FA80
		protected static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return blue.def.entityDefToBuild is TerrainDef && pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position);
		}

		// Token: 0x060081DC RID: 33244 RVA: 0x002718AC File Offset: 0x0026FAAC
		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			if (!WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				return null;
			}
			if (!pawn.CanReserve(blue.Position, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFloor, blue.Position);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x060081DD RID: 33245 RVA: 0x00271914 File Offset: 0x0026FB14
		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			IThingHolder parentHolder = miniToInstallOrBuildingToReinstall.ParentHolder;
			if (parentHolder != null)
			{
				Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					JobFailReason.Is("BeingCarriedBy".Translate(pawn_CarryTracker.pawn), null);
					return null;
				}
			}
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
				return null;
			}
			if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
				return null;
			}
			if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn, null);
				if (pawn2 != null)
				{
					JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
				}
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer);
			job.targetA = miniToInstallOrBuildingToReinstall;
			job.targetB = install;
			job.count = 1;
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x060081E0 RID: 33248 RVA: 0x00271A35 File Offset: 0x0026FC35
		[CompilerGenerated]
		internal static bool <CanUseCarriedResource>g__IsValidJob|9_0(Job job, ref WorkGiver_ConstructDeliverResources.<>c__DisplayClass9_0 A_1)
		{
			return job.def != JobDefOf.HaulToContainer || job.targetA != A_1.pawn.carryTracker.CarriedThing;
		}

		// Token: 0x04005660 RID: 22112
		private static readonly List<Thing> resourcesAvailable = new List<Thing>();

		// Token: 0x04005661 RID: 22113
		private static readonly Dictionary<ThingDef, int> missingResources = new Dictionary<ThingDef, int>();

		// Token: 0x04005662 RID: 22114
		private const float MultiPickupRadius = 5f;

		// Token: 0x04005663 RID: 22115
		private const float NearbyConstructScanRadius = 8f;

		// Token: 0x04005664 RID: 22116
		protected static string ForbiddenLowerTranslated;

		// Token: 0x04005665 RID: 22117
		protected static string NoPathTranslated;
	}
}
