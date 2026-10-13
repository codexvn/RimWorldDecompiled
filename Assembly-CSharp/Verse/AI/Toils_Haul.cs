using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000EC6 RID: 3782
	public class Toils_Haul
	{
		// Token: 0x06005F2D RID: 24365 RVA: 0x001E3FA4 File Offset: 0x001E21A4
		public static bool ErrorCheckForCarry(Pawn pawn, Thing haulThing, bool canTakeFromInventory = false)
		{
			if (!haulThing.SpawnedOrAnyParentSpawned || (!canTakeFromInventory && !haulThing.Spawned))
			{
				Log.Message(((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + ((haulThing != null) ? haulThing.ToString() : null) + " which isn't spawned.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (haulThing.stackCount == 0)
			{
				Log.Message(((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + ((haulThing != null) ? haulThing.ToString() : null) + " which had stackcount 0.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (pawn.jobs.curJob.count <= 0)
			{
				string str = "Invalid count: ";
				string str2 = pawn.jobs.curJob.count.ToString();
				string str3 = ", setting to 1. Job was ";
				Job curJob = pawn.jobs.curJob;
				Log.Error(str + str2 + str3 + ((curJob != null) ? curJob.ToString() : null));
				pawn.jobs.curJob.count = 1;
			}
			return false;
		}

		// Token: 0x06005F2E RID: 24366 RVA: 0x001E40B0 File Offset: 0x001E22B0
		public static Toil StartCarryThing(TargetIndex haulableInd, bool putRemainderInQueue = false, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = false, bool reserve = true, bool canTakeFromInventory = false)
		{
			Toil toil = ToilMaker.MakeToil("StartCarryThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(haulableInd).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing, canTakeFromInventory))
				{
					return;
				}
				if (curJob.count == 0)
				{
					throw new Exception(string.Format("StartCarryThing job had count = {0}. Job: {1}", curJob.count, curJob));
				}
				int num = actor.carryTracker.AvailableStackSpace(thing.def);
				if (num <= 0)
				{
					int num2 = actor.carryTracker.MaxStackSpaceEver(thing.def);
					int num3 = 0;
					if (actor.carryTracker.CarriedThing != null)
					{
						num3 = actor.carryTracker.CarriedThing.stackCount;
					}
					throw new Exception(string.Format("StartCarryThing got availableStackSpace {0} (haulTarg {1}, Job: {2}, maximum: {3}, carrying: {4})", new object[]
					{
						num,
						thing,
						curJob,
						num2,
						num3
					}));
				}
				if (failIfStackCountLessThanJobCount && thing.stackCount < curJob.count)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				int num4 = Mathf.Min(new int[]
				{
					curJob.count,
					num,
					thing.stackCount
				});
				if (num4 <= 0)
				{
					int num5 = actor.carryTracker.MaxStackSpaceEver(thing.def);
					int num6 = 0;
					if (actor.carryTracker.CarriedThing != null)
					{
						num6 = actor.carryTracker.CarriedThing.stackCount;
					}
					throw new Exception(string.Format("StartCarryThing zero or negative desiredNumToTake ({0}), actor: {1}, curJob.count: {2}, availableStackSpace: {3} (maximum: {4}, carrying: {5}), haulTarg.stackCount: {6}", new object[]
					{
						num4,
						actor,
						curJob.count,
						num,
						num5,
						num6,
						thing.stackCount
					}));
				}
				int stackCount = thing.stackCount;
				int num7 = actor.carryTracker.TryStartCarry(thing, num4, reserve);
				if (num7 == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				if (num7 < stackCount)
				{
					int num8 = curJob.count - num7;
					if (putRemainderInQueue && num8 > 0)
					{
						curJob.GetTargetQueue(haulableInd).Insert(0, thing);
						Job job = curJob;
						if (job.countQueue == null)
						{
							job.countQueue = new List<int>();
						}
						curJob.countQueue.Insert(0, num8);
					}
					else if (actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
					{
						actor.Map.reservationManager.Release(thing, actor, curJob);
					}
				}
				if (subtractNumTakenFromJobCount)
				{
					curJob.count -= num7;
				}
				curJob.SetTarget(haulableInd, actor.carryTracker.CarriedThing);
				actor.records.Increment(RecordDefOf.ThingsHauled);
			};
			return toil;
		}

		// Token: 0x06005F2F RID: 24367 RVA: 0x001E411C File Offset: 0x001E231C
		public static Toil StoreThingJob(TargetIndex thingIndex)
		{
			Toil toil = ToilMaker.MakeToil("StoreThingJob");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				Thing thing = curJob.GetTarget(thingIndex).Thing;
				Job job = HaulAIUtility.HaulToStorageJob(actor, thing, curJob.playerForced);
				if (job != null)
				{
					actor.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			};
			return toil;
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x001E4164 File Offset: 0x001E2364
		public static Toil DropCarriedThing()
		{
			Toil toil = ToilMaker.MakeToil("DropCarriedThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to drop carried thing but is not carrying anything.");
					return;
				}
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(actor.Position, ThingPlaceMode.Direct, out thing, null))
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			return toil;
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x001E41A4 File Offset: 0x001E23A4
		public static Toil JumpIfAlsoCollectingNextTargetInQueue(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfAlsoCollectingNextTargetInQueue");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				if (curJob.count <= 0)
				{
					return;
				}
				if (actor.carryTracker.CarriedThing == null)
				{
					string str = "JumpToAlsoCollectTargetInQueue run on ";
					Pawn pawn = actor;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who is not carrying something.");
					return;
				}
				if (actor.carryTracker.AvailableStackSpace(actor.carryTracker.CarriedThing.def) <= 0)
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (!GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					if (targetQueue[i].Thing.def == actor.carryTracker.CarriedThing.def)
					{
						curJob.SetTarget(ind, targetQueue[i].Thing);
						targetQueue.RemoveAt(i);
						actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x001E41F4 File Offset: 0x001E23F4
		public static Toil CheckForGetOpportunityDuplicate(Toil getHaulTargetToil, TargetIndex haulableInd, TargetIndex storeCellInd, bool takeFromValidStorage = false, Predicate<Thing> extraValidator = null)
		{
			Toils_Haul.<>c__DisplayClass5_0 CS$<>8__locals1 = new Toils_Haul.<>c__DisplayClass5_0();
			CS$<>8__locals1.storeCellInd = storeCellInd;
			CS$<>8__locals1.takeFromValidStorage = takeFromValidStorage;
			CS$<>8__locals1.extraValidator = extraValidator;
			CS$<>8__locals1.haulableInd = haulableInd;
			CS$<>8__locals1.getHaulTargetToil = getHaulTargetToil;
			CS$<>8__locals1.toil = ToilMaker.MakeToil("CheckForGetOpportunityDuplicate");
			CS$<>8__locals1.toil.initAction = delegate()
			{
				Toils_Haul.<>c__DisplayClass5_1 CS$<>8__locals2 = new Toils_Haul.<>c__DisplayClass5_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.actor = CS$<>8__locals1.toil.actor;
				CS$<>8__locals2.curJob = CS$<>8__locals2.actor.jobs.curJob;
				if (CS$<>8__locals2.actor.carryTracker.CarriedThing.def.stackLimit == 1)
				{
					return;
				}
				if (CS$<>8__locals2.actor.carryTracker.Full)
				{
					return;
				}
				if (CS$<>8__locals2.curJob.count <= 0)
				{
					return;
				}
				Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals2.actor.Position, CS$<>8__locals2.actor.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals2.actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, new Predicate<Thing>(CS$<>8__locals2.<CheckForGetOpportunityDuplicate>g__DupeValidator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					CS$<>8__locals2.curJob.SetTarget(CS$<>8__locals1.haulableInd, thing);
					CS$<>8__locals2.actor.jobs.curDriver.JumpToToil(CS$<>8__locals1.getHaulTargetToil);
				}
			};
			return CS$<>8__locals1.toil;
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x001E4258 File Offset: 0x001E2458
		public static Toil CarryHauledThingToCell(TargetIndex squareIndex, PathEndMode pathEndMode = PathEndMode.ClosestTouch)
		{
			Toil toil = ToilMaker.MakeToil("CarryHauledThingToCell");
			toil.initAction = delegate()
			{
				IntVec3 cell = toil.actor.jobs.curJob.GetTarget(squareIndex).Cell;
				toil.actor.pather.StartPath(cell, pathEndMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.AddEndCondition(delegate
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				CompPushable compPushable = actor.carryTracker.CarriedThing.TryGetComp<CompPushable>();
				if (compPushable != null)
				{
					Vector3 v = actor.Position.ToVector3() + compPushable.drawPos;
					if (new IntVec3(v) == cell)
					{
						return JobCondition.Succeeded;
					}
				}
				return JobCondition.Ongoing;
			});
			toil.AddFailCondition(delegate
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					return true;
				}
				if (actor.jobs.curJob.haulMode == HaulMode.ToCellStorage && !cell.IsValidStorageFor(actor.Map, actor.carryTracker.CarriedThing))
				{
					return true;
				}
				CompPushable compPushable = actor.carryTracker.CarriedThing.TryGetComp<CompPushable>();
				return compPushable != null && !compPushable.canBePushed;
			});
			return toil;
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x001E42E0 File Offset: 0x001E24E0
		public static Toil PlaceCarriedThingInCellFacing(TargetIndex facingTargetInd)
		{
			Toil toil = ToilMaker.MakeToil("PlaceCarriedThingInCellFacing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in facing cell but is not hauling anything.");
					return;
				}
				LocalTargetInfo target = actor.CurJob.GetTarget(facingTargetInd);
				IntVec3 b;
				if (target.HasThing)
				{
					b = target.Thing.OccupiedRect().ClosestCellTo(actor.Position);
				}
				else
				{
					b = target.Cell;
				}
				IntVec3 dropLoc = actor.Position + Pawn_RotationTracker.RotFromAngleBiased((actor.Position - b).AngleFlat).FacingCell;
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(dropLoc, ThingPlaceMode.Direct, out thing, null))
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			return toil;
		}

		// Token: 0x06005F35 RID: 24373 RVA: 0x001E4328 File Offset: 0x001E2528
		public static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode, bool tryStoreInSameStorageIfSpotCantHoldWholeStack = false)
		{
			Toil toil = ToilMaker.MakeToil("PlaceHauledThingInCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				IntVec3 cell = curJob.GetTarget(cellInd).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in cell but is not hauling anything.");
					return;
				}
				SlotGroup slotGroup = actor.Map.haulDestinationManager.SlotGroupAt(cell);
				if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
				{
					actor.Map.designationManager.TryRemoveDesignationOn(actor.carryTracker.CarriedThing, DesignationDefOf.Haul);
				}
				Action<Thing, int> placedAction = null;
				if (curJob.def == JobDefOf.DoBill || curJob.def == JobDefOf.RecolorApparel || curJob.def == JobDefOf.RefuelAtomic || curJob.def == JobDefOf.RearmTurretAtomic)
				{
					placedAction = delegate(Thing th, int added)
					{
						HaulAIUtility.UpdateJobWithPlacedThings(curJob, th, added);
					};
				}
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out thing, placedAction))
				{
					if (storageMode)
					{
						IntVec3 c;
						if (nextToilOnPlaceFailOrIncomplete != null && ((tryStoreInSameStorageIfSpotCantHoldWholeStack && StoreUtility.TryFindBestBetterStoreCellForIn(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, curJob.bill.GetSlotGroup(), out c, true)) || StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out c, true)))
						{
							if (actor.CanReserve(c, 1, -1, null, false))
							{
								actor.Reserve(c, actor.CurJob, 1, -1, null, true, false);
							}
							actor.CurJob.SetTarget(cellInd, c);
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						IntVec3 c2;
						if (!HaulAIUtility.CanHaulAside(actor, actor.carryTracker.CarriedThing, out c2))
						{
							Log.Warning(string.Format("Incomplete haul for {0}: Could not find anywhere to put {1} near {2}. Destroying. This should be very uncommon!", actor, actor.carryTracker.CarriedThing, actor.Position));
							actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
							return;
						}
						curJob.SetTarget(cellInd, c2);
						curJob.count = int.MaxValue;
						curJob.haulOpportunisticDuplicates = false;
						curJob.haulMode = HaulMode.ToCellNonStorage;
						if (nextToilOnPlaceFailOrIncomplete != null)
						{
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
					}
					else if (nextToilOnPlaceFailOrIncomplete != null)
					{
						actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x001E4384 File Offset: 0x001E2584
		public static Toil CarryHauledThingToContainer()
		{
			Toil gotoDest = ToilMaker.MakeToil("CarryHauledThingToContainer");
			gotoDest.initAction = delegate()
			{
				gotoDest.actor.pather.StartPath(gotoDest.actor.jobs.curJob.targetB.Thing, PathEndMode.Touch);
			};
			gotoDest.AddFailCondition(delegate
			{
				Thing thing = gotoDest.actor.jobs.curJob.targetB.Thing;
				if (thing.Destroyed || (!gotoDest.actor.jobs.curJob.ignoreForbidden && thing.IsForbidden(gotoDest.actor)))
				{
					return true;
				}
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				return thingOwner != null && !thingOwner.CanAcceptAnyOf(gotoDest.actor.carryTracker.CarriedThing, true);
			});
			gotoDest.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return gotoDest;
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x001E43E8 File Offset: 0x001E25E8
		public static Toil DepositHauledThingInContainer(TargetIndex containerInd, TargetIndex reserveForContainerInd, Action onDeposited = null)
		{
			Toil toil = ToilMaker.MakeToil("DepositHauledThingInContainer");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in container but is not hauling anything.");
					return;
				}
				Thing thing = curJob.GetTarget(containerInd).Thing;
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					int num = actor.carryTracker.CarriedThing.stackCount;
					IHaulEnroute haulEnroute = thing as IHaulEnroute;
					if (haulEnroute != null)
					{
						ThingDef def = actor.carryTracker.CarriedThing.def;
						num = Mathf.Min(haulEnroute.GetSpaceRemainingWithEnroute(def, actor), num);
						if (reserveForContainerInd != TargetIndex.None)
						{
							Thing thing2 = curJob.GetTarget(reserveForContainerInd).Thing;
							if (!thing2.DestroyedOrNull() && thing2 != haulEnroute)
							{
								IHaulEnroute haulEnroute2 = thing2 as IHaulEnroute;
								if (haulEnroute2 != null)
								{
									int spaceRemainingWithEnroute = haulEnroute2.GetSpaceRemainingWithEnroute(def, actor);
									num = Mathf.Min(num, actor.carryTracker.CarriedThing.stackCount - spaceRemainingWithEnroute);
								}
							}
						}
					}
					Thing carriedThing = actor.carryTracker.CarriedThing;
					int num2 = actor.carryTracker.innerContainer.TryTransferToContainer(carriedThing, thingOwner, num, true);
					if (num2 != 0)
					{
						IHaulEnroute haulEnroute3 = thing as IHaulEnroute;
						if (haulEnroute3 != null)
						{
							thing.Map.enrouteManager.ReleaseFor(haulEnroute3, actor);
						}
						INotifyHauledTo notifyHauledTo = thing as INotifyHauledTo;
						if (notifyHauledTo != null)
						{
							notifyHauledTo.Notify_HauledTo(actor, carriedThing, num2);
						}
						ThingWithComps thingWithComps = thing as ThingWithComps;
						if (thingWithComps != null)
						{
							foreach (ThingComp thingComp in thingWithComps.AllComps)
							{
								INotifyHauledTo notifyHauledTo2 = thingComp as INotifyHauledTo;
								if (notifyHauledTo2 != null)
								{
									notifyHauledTo2.Notify_HauledTo(actor, carriedThing, num2);
								}
							}
						}
						if (curJob.def == JobDefOf.DoBill)
						{
							HaulAIUtility.UpdateJobWithPlacedThings(curJob, carriedThing, num2);
						}
						Action onDeposited2 = onDeposited;
						if (onDeposited2 == null)
						{
							return;
						}
						onDeposited2();
						return;
					}
				}
				else
				{
					if (curJob.GetTarget(containerInd).Thing.def.Minifiable)
					{
						actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						return;
					}
					string str = "Could not deposit hauled thing in container: ";
					Thing thing3 = curJob.GetTarget(containerInd).Thing;
					Log.Error(str + ((thing3 != null) ? thing3.ToString() : null));
				}
			};
			return toil;
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x001E4440 File Offset: 0x001E2640
		public static Toil JumpToCarryToNextContainerIfPossible(Toil carryToContainerToil, TargetIndex primaryTargetInd)
		{
			Toil toil = ToilMaker.MakeToil("JumpToCarryToNextContainerIfPossible");
			toil.debugName = "Jump carry if possible";
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					return;
				}
				if (curJob.targetQueueB != null && curJob.targetQueueB.Count > 0)
				{
					Thing nextTarget;
					if (!Toils_Haul.TryGetNextDestinationFromQueue(primaryTargetInd, TargetIndex.B, actor.carryTracker.CarriedThing.def, curJob, actor, out nextTarget))
					{
						return;
					}
					curJob.targetQueueB.RemoveAll((LocalTargetInfo target) => target.Thing == nextTarget);
					curJob.targetB = nextTarget;
					curJob.targetC = nextTarget;
					actor.jobs.curDriver.JumpToToil(carryToContainerToil);
				}
			};
			return toil;
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x001E44A0 File Offset: 0x001E26A0
		public static bool TryGetNextDestinationFromQueue(TargetIndex primaryIndex, TargetIndex destIndex, ThingDef stuff, Job job, Pawn actor, out Thing target)
		{
			Toils_Haul.<>c__DisplayClass12_0 CS$<>8__locals1 = new Toils_Haul.<>c__DisplayClass12_0();
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.actor = actor;
			CS$<>8__locals1.primaryTarget = job.GetTarget(primaryIndex).Thing;
			target = null;
			Pawn_CarryTracker carryTracker = CS$<>8__locals1.actor.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) == null)
			{
				return false;
			}
			CS$<>8__locals1.hasSpareItems = (CS$<>8__locals1.actor.carryTracker.CarriedThing.stackCount > 0);
			if (CS$<>8__locals1.primaryTarget != null && CS$<>8__locals1.primaryTarget.Spawned)
			{
				IHaulEnroute haulEnroute = CS$<>8__locals1.primaryTarget as IHaulEnroute;
				if (haulEnroute != null)
				{
					int spaceRemainingWithEnroute = haulEnroute.GetSpaceRemainingWithEnroute(CS$<>8__locals1.stuff, CS$<>8__locals1.actor);
					CS$<>8__locals1.hasSpareItems = (CS$<>8__locals1.actor.carryTracker.CarriedThing.stackCount > spaceRemainingWithEnroute);
				}
			}
			target = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.actor.Position, CS$<>8__locals1.actor.Map, from x in job.GetTargetQueue(destIndex)
			select x.Thing, PathEndMode.Touch, TraverseParms.For(CS$<>8__locals1.actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 99999f, new Predicate<Thing>(CS$<>8__locals1.<TryGetNextDestinationFromQueue>g__Validator|0), null, false);
			return target != null;
		}

		// Token: 0x06005F3A RID: 24378 RVA: 0x001E45DD File Offset: 0x001E27DD
		public static Toil TakeToInventory(TargetIndex ind, int count)
		{
			return Toils_Haul.TakeToInventory(ind, new int?(count), null, null);
		}

		// Token: 0x06005F3B RID: 24379 RVA: 0x001E45F0 File Offset: 0x001E27F0
		private static Toil TakeToInventory(TargetIndex ind, int? count, Func<int> countGetter, Func<Thing, int> countGetterPassingThing)
		{
			Toil takeThing = ToilMaker.MakeToil("TakeToInventory");
			takeThing.initAction = delegate()
			{
				Pawn actor = takeThing.actor;
				Thing thing = actor.CurJob.GetTarget(ind).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing, false))
				{
					return;
				}
				int? count2 = count;
				int a;
				if (count2 == null)
				{
					Func<Thing, int> countGetterPassingThing2 = countGetterPassingThing;
					a = ((countGetterPassingThing2 != null) ? countGetterPassingThing2(thing) : countGetter());
				}
				else
				{
					a = count2.GetValueOrDefault();
				}
				int num = Mathf.Min(a, thing.stackCount);
				if (actor.CurJob.checkEncumbrance)
				{
					num = Math.Min(num, MassUtility.CountToPickUpUntilOverEncumbered(actor, thing));
				}
				if (num <= 0)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.inventory.GetDirectlyHeldThings().TryAdd(thing.SplitOff(num), true);
				if (thing.def.ingestible != null && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				}
				thing.def.soundPickup.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return takeThing;
		}

		// Token: 0x06005F3C RID: 24380 RVA: 0x001E464C File Offset: 0x001E284C
		public static Toil TakeToInventory(TargetIndex ind, Func<int> countGetter)
		{
			return Toils_Haul.TakeToInventory(ind, null, countGetter, null);
		}

		// Token: 0x06005F3D RID: 24381 RVA: 0x001E466C File Offset: 0x001E286C
		public static Toil TakeToInventory(TargetIndex ind, Func<Thing, int> countGetter)
		{
			return Toils_Haul.TakeToInventory(ind, null, null, countGetter);
		}

		// Token: 0x06005F3E RID: 24382 RVA: 0x001E468C File Offset: 0x001E288C
		public static Toil TakeFromOtherInventory(Thing item, ThingOwner taker, ThingOwner holder, int count = -1, TargetIndex indexToSet = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("TakeFromOtherInventory");
			toil.initAction = delegate()
			{
				if (!holder.Contains(item))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				count = ((count < 0) ? toil.actor.jobs.curJob.count : count);
				Thing thing;
				holder.TryTransferToContainer(item, taker, Mathf.Min(item.stackCount, count), out thing, true);
				if (thing == null)
				{
					Log.Warning(string.Format("Taker {0} unable to take count {1} of thing {2} from holder's inventory", toil.actor.Label, count, item.Label));
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				if (indexToSet != TargetIndex.None)
				{
					toil.actor.jobs.curJob.SetTarget(indexToSet, thing);
				}
			};
			return toil;
		}

		// Token: 0x06005F3F RID: 24383 RVA: 0x001E46F0 File Offset: 0x001E28F0
		public static Toil CheckItemCarriedByOtherPawn(Thing item, TargetIndex targetPawnIfCarried = TargetIndex.None, Toil jumpIfCarriedByOther = null)
		{
			Toil toil = ToilMaker.MakeToil("CheckItemCarriedByOtherPawn");
			toil.initAction = delegate()
			{
				Thing item2 = item;
				Pawn_InventoryTracker pawn_InventoryTracker = ((item2 != null) ? item2.ParentHolder : null) as Pawn_InventoryTracker;
				Pawn pawn = (pawn_InventoryTracker != null) ? pawn_InventoryTracker.pawn : null;
				if (pawn != null && pawn != toil.actor)
				{
					if (targetPawnIfCarried != TargetIndex.None)
					{
						toil.actor.jobs.curJob.SetTarget(targetPawnIfCarried, pawn);
					}
					if (jumpIfCarriedByOther != null)
					{
						toil.actor.jobs.curDriver.JumpToToil(jumpIfCarriedByOther);
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
