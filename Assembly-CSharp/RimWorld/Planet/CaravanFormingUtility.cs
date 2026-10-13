using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003D52 RID: 15698
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		// Token: 0x06015614 RID: 87572 RVA: 0x00664EB3 File Offset: 0x006630B3
		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, PlanetTile exitFromTile, PlanetTile directionTile, PlanetTile destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

		// Token: 0x06015615 RID: 87573 RVA: 0x00664EC4 File Offset: 0x006630C4
		public static void StartFormingCaravan(List<Pawn> pawns, List<Pawn> downedPawns, Faction faction, List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, PlanetTile startingTile, PlanetTile destinationTile)
		{
			if (!startingTile.Valid)
			{
				Log.Error("Can't start forming caravan because startingTile is invalid.");
				return;
			}
			if (!pawns.Any<Pawn>())
			{
				Log.Error("Can't start forming caravan with 0 pawns.");
				return;
			}
			if (pawns.Any((Pawn x) => x.Downed))
			{
				Log.Warning("Forming a caravan with a downed pawn. This shouldn't happen because we have to create a Lord.");
			}
			List<TransferableOneWay> list = transferables.ToList<TransferableOneWay>();
			list.RemoveAll((TransferableOneWay x) => x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn);
			for (int i = 0; i < pawns.Count; i++)
			{
				Lord lord = pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_FormAndSendCaravan lordJob = new LordJob_FormAndSendCaravan(list, downedPawns, meetingPoint, exitSpot, startingTile, destinationTile);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.Spawned)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06015616 RID: 87574 RVA: 0x00664FF4 File Offset: 0x006631F4
		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.AddRange(lord.ownedPawns);
			bool isPlayerHome = lord.Map.IsPlayerHome;
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
			if (isPlayerHome)
			{
				CaravanFormingUtility.LeadAnimalsToPen(CaravanFormingUtility.tmpPawnsInCancelledCaravan);
			}
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
		}

		// Token: 0x06015617 RID: 87575 RVA: 0x00665050 File Offset: 0x00663250
		public static void LeadAnimalsToPen(List<Pawn> pawns)
		{
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.Spawned && !pawn.Downed && !pawn.Dead && !pawn.Drafted)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						CaravanFormingUtility.tmpNeedRoping.Add(pawn);
					}
					else if (pawn.IsColonist)
					{
						CaravanFormingUtility.tmpRopers.Add(pawn);
					}
				}
			}
			if (CaravanFormingUtility.tmpNeedRoping.Any<Pawn>() && CaravanFormingUtility.tmpRopers.Any<Pawn>())
			{
				using (List<Pawn>.Enumerator enumerator = CaravanFormingUtility.tmpNeedRoping.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn ropee = enumerator.Current;
						if (!ropee.roping.IsRoped)
						{
							CaravanFormingUtility.tmpRopers.MinBy((Pawn p) => p.Position.DistanceToSquared(ropee.Position)).roping.RopePawn(ropee);
						}
					}
				}
				CaravanFormingUtility.StartReturnedLord((from p in CaravanFormingUtility.tmpRopers
				where p.roping.IsRopingOthers
				select p).Concat(CaravanFormingUtility.tmpNeedRoping).ToList<Pawn>());
			}
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
		}

		// Token: 0x06015618 RID: 87576 RVA: 0x006651E0 File Offset: 0x006633E0
		private static void StartReturnedLord(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_ReturnedCaravan lordJob = new LordJob_ReturnedCaravan(pawns[0].Position);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			foreach (Pawn pawn2 in pawns)
			{
				if (pawn2.Spawned)
				{
					pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06015619 RID: 87577 RVA: 0x006652C0 File Offset: 0x006634C0
		public static void RemovePawnFromCaravan(Pawn pawn, Lord lord, bool removeFromDowned = true)
		{
			bool flag = false;
			pawn.inventory.DropAllPackingCaravanThings();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = lord.ownedPawns[i];
				if (pawn2 != pawn && CaravanUtility.IsOwner(pawn2, Faction.OfPlayer))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = true;
			TaggedString taggedString = "MessagePawnLostWhileFormingCaravan".Translate(pawn).CapitalizeFirst();
			if (!flag)
			{
				CaravanFormingUtility.StopFormingCaravan(lord);
				taggedString += " " + "MessagePawnLostWhileFormingCaravan_AllLost".Translate();
			}
			else
			{
				pawn.inventory.UnloadEverything = true;
				if (lord.ownedPawns.Contains(pawn))
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
					flag2 = false;
				}
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(pawn))
				{
					if (!removeFromDowned)
					{
						flag2 = false;
					}
					else
					{
						lordJob_FormAndSendCaravan.downedPawns.Remove(pawn);
					}
				}
				if (pawn.jobs != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			}
			if (flag2)
			{
				Messages.Message(taggedString, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0601561A RID: 87578 RVA: 0x006653EC File Offset: 0x006635EC
		public static void ForceCaravanDepart(Lord lord)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)lord.LordJob;
			for (int i = lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.InMentalState)
				{
					lord.RemovePawn(pawn);
					pawn.inventory.UnloadEverything = true;
				}
			}
			if (lord.ownedPawns.Count == 0)
			{
				lord.Map.lordManager.RemoveLord(lord);
				Messages.Message("MessageForceSendingCaravan_AllLost".Translate(), MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			for (int j = lord.ownedPawns.Count - 1; j >= 0; j--)
			{
				Pawn pawn2 = lord.ownedPawns[j];
				if (pawn2.Drafted)
				{
					pawn2.drafter.Drafted = false;
				}
				else if (pawn2.roping.RopedToSpot.IsValid)
				{
					pawn2.roping.UnropeFromSpot();
				}
			}
			List<Pawn> downedPawns = lordJob_FormAndSendCaravan.downedPawns;
			if (downedPawns != null)
			{
				downedPawns.Clear();
			}
			lord.ReceiveMemo("ForceDepartNow");
		}

		// Token: 0x0601561B RID: 87579 RVA: 0x006654FC File Offset: 0x006636FC
		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.DropAllPackingCaravanThings();
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		// Token: 0x0601561C RID: 87580 RVA: 0x0066554C File Offset: 0x0066374C
		public static void TryAddItemBackToTransferables(Thing item, List<TransferableOneWay> transferables, int count)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(item, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null && !transferableOneWay.things.Contains(item))
			{
				transferableOneWay.things.Add(item);
			}
			if (transferableOneWay != null)
			{
				transferableOneWay.AdjustTo(transferableOneWay.ClampAmount(transferableOneWay.CountToTransfer + count));
			}
		}

		// Token: 0x0601561D RID: 87581 RVA: 0x00665598 File Offset: 0x00663798
		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			CaravanFormingUtility.<>c__DisplayClass16_0 CS$<>8__locals1 = new CaravanFormingUtility.<>c__DisplayClass16_0();
			CS$<>8__locals1.canMinify = canMinify;
			CS$<>8__locals1.allowEvenIfOutsideHomeArea = allowEvenIfOutsideHomeArea;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.allowEvenIfReserved = allowEvenIfReserved;
			CaravanFormingUtility.tmpThings.Clear();
			CS$<>8__locals1.map.listerThings.GetAllThings(CaravanFormingUtility.tmpThings, new Predicate<Thing>(CS$<>8__locals1.<AllReachableColonyItems>g__IsValidCaravanItem|0), true);
			return CaravanFormingUtility.tmpThings;
		}

		// Token: 0x0601561E RID: 87582 RVA: 0x006655F8 File Offset: 0x006637F8
		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDowned = false, bool allowEvenIfInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false, bool allowLodgers = false, int allowLoadAndEnterTransportersLordForGroupID = -1)
		{
			List<Pawn> list = new List<Pawn>();
			IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((allowEvenIfDowned || !pawn.Downed) && (allowEvenIfInMentalState || !pawn.InMentalState) && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || (allowCapturableDownedPawns && CaravanFormingUtility.CanListAsAutoCapturable(pawn))) && pawn.RaceProps.allowedOnCaravan && !pawn.IsQuestHelper() && (!pawn.IsQuestLodger() || allowLodgers) && (allowEvenIfPrisonerNotSecure || !pawn.IsPrisoner || pawn.guest.PrisonerIsSecure) && (pawn.GetLord() == null || pawn.GetLord().LordJob is LordJob_VoluntarilyJoinable || pawn.GetLord().LordJob.IsCaravanSendable || (allowLoadAndEnterTransportersLordForGroupID >= 0 && pawn.GetLord().LordJob is LordJob_LoadAndEnterTransporters && ((LordJob_LoadAndEnterTransporters)pawn.GetLord().LordJob).transportersGroup == allowLoadAndEnterTransportersLordForGroupID)))
				{
					list.Add(pawn);
				}
			}
			return list;
		}

		// Token: 0x0601561F RID: 87583 RVA: 0x00665724 File Offset: 0x00663924
		private static bool CanListAsAutoCapturable(Pawn p)
		{
			return p.Downed && !p.mindState.WillJoinColonyIfRescued && CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer);
		}

		// Token: 0x06015620 RID: 87584 RVA: 0x00665748 File Offset: 0x00663948
		public static IEnumerable<Gizmo> GetGizmos(Pawn pawn)
		{
			CaravanFormingUtility.<>c__DisplayClass19_0 CS$<>8__locals1 = new CaravanFormingUtility.<>c__DisplayClass19_0();
			CS$<>8__locals1.pawn = pawn;
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(CS$<>8__locals1.pawn))
			{
				CaravanFormingUtility.<>c__DisplayClass19_1 CS$<>8__locals2 = new CaravanFormingUtility.<>c__DisplayClass19_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.lord = CaravanFormingUtility.GetFormAndSendCaravanLord(CS$<>8__locals2.CS$<>8__locals1.pawn);
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelFormingCaravan".Translate(),
					defaultDesc = "CommandCancelFormingCaravanDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						CaravanFormingUtility.StopFormingCaravan(CS$<>8__locals2.lord);
					},
					hotKey = KeyBindingDefOf.Designator_Cancel
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandRemoveFromCaravan".Translate(),
					defaultDesc = "CommandRemoveFromCaravanDesc".Translate(),
					icon = CaravanFormingUtility.RemoveFromCaravanCommand,
					action = delegate()
					{
						CaravanFormingUtility.RemovePawnFromCaravan(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.lord, true);
					},
					hotKey = KeyBindingDefOf.Misc6
				};
				if (!(CS$<>8__locals2.lord.CurLordToil is LordToil_PrepareCaravan_Leave))
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandForceCaravanDepart".Translate(),
						defaultDesc = "CommandForceCaravanDepartDesc".Translate(),
						icon = CaravanFormingUtility.ForceDepartCaravanCommand,
						action = delegate()
						{
							string forceDepartWarningMessage = CaravanFormingUtility.GetForceDepartWarningMessage(CS$<>8__locals2.lord.LordJob as LordJob_FormAndSendCaravan);
							WindowStack windowStack = Find.WindowStack;
							TaggedString text = forceDepartWarningMessage;
							Action confirmedAct;
							if ((confirmedAct = CS$<>8__locals2.<>9__3) == null)
							{
								confirmedAct = (CS$<>8__locals2.<>9__3 = delegate()
								{
									CaravanFormingUtility.ForceCaravanDepart(CS$<>8__locals2.lord);
								});
							}
							windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
						},
						hotKey = KeyBindingDefOf.Misc8
					};
				}
				CS$<>8__locals2 = null;
			}
			else if (CS$<>8__locals1.pawn.Spawned)
			{
				bool flag = false;
				for (int i = 0; i < CS$<>8__locals1.pawn.Map.lordManager.lords.Count; i++)
				{
					Lord lord = CS$<>8__locals1.pawn.Map.lordManager.lords[i];
					if (lord.faction == Faction.OfPlayer && lord.LordJob is LordJob_FormAndSendCaravan)
					{
						flag = true;
						break;
					}
				}
				if (flag && Dialog_FormCaravan.AllSendablePawns(CS$<>8__locals1.pawn.Map, false).Contains(CS$<>8__locals1.pawn))
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandAddToCaravan".Translate(),
						defaultDesc = "CommandAddToCaravanDesc".Translate(),
						icon = CaravanFormingUtility.AddToCaravanCommand,
						action = delegate()
						{
							List<Lord> list = new List<Lord>();
							for (int j = 0; j < CS$<>8__locals1.pawn.Map.lordManager.lords.Count; j++)
							{
								Lord lord2 = CS$<>8__locals1.pawn.Map.lordManager.lords[j];
								if (lord2.faction == Faction.OfPlayer && lord2.LordJob is LordJob_FormAndSendCaravan)
								{
									list.Add(lord2);
								}
							}
							if (list.Count == 0)
							{
								return;
							}
							if (list.Count == 1)
							{
								CaravanFormingUtility.LateJoinFormingCaravan(CS$<>8__locals1.pawn, list[0]);
								SoundDefOf.Click.PlayOneShotOnCamera(null);
								return;
							}
							List<FloatMenuOption> list2 = new List<FloatMenuOption>();
							for (int k = 0; k < list.Count; k++)
							{
								Lord caravanLocal = list[k];
								string label = "Caravan".Translate() + " " + (k + 1).ToString();
								list2.Add(new FloatMenuOption(label, delegate()
								{
									if (CS$<>8__locals1.pawn.Spawned && CS$<>8__locals1.pawn.Map.lordManager.lords.Contains(caravanLocal) && Dialog_FormCaravan.AllSendablePawns(CS$<>8__locals1.pawn.Map, false).Contains(CS$<>8__locals1.pawn))
									{
										CaravanFormingUtility.LateJoinFormingCaravan(CS$<>8__locals1.pawn, caravanLocal);
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							Find.WindowStack.Add(new FloatMenu(list2));
						},
						hotKey = KeyBindingDefOf.Misc7
					};
				}
			}
			yield break;
		}

		// Token: 0x06015621 RID: 87585 RVA: 0x00665758 File Offset: 0x00663958
		public static void LateJoinFormingCaravan(Pawn pawn, Lord lord)
		{
			Lord lord2 = pawn.GetLord();
			if (lord2 != null)
			{
				lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
			}
			if (pawn.Downed)
			{
				((LordJob_FormAndSendCaravan)lord.LordJob).downedPawns.Add(pawn);
			}
			else
			{
				lord.AddPawn(pawn);
			}
			if (pawn.Spawned)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06015622 RID: 87586 RVA: 0x006657C0 File Offset: 0x006639C0
		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord;
			return p.TryGetFormingCaravanLord(out lord);
		}

		// Token: 0x06015623 RID: 87587 RVA: 0x006657D5 File Offset: 0x006639D5
		public static bool TryGetFormingCaravanLord(this Pawn p, out Lord lord)
		{
			lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

		// Token: 0x06015624 RID: 87588 RVA: 0x006657F4 File Offset: 0x006639F4
		public static bool IsFormingCaravanOrDownedPawnToBeTakenByCaravan(Pawn p)
		{
			return CaravanFormingUtility.GetFormAndSendCaravanLord(p) != null;
		}

		// Token: 0x06015625 RID: 87589 RVA: 0x00665800 File Offset: 0x00663A00
		public static Lord GetFormAndSendCaravanLord(Pawn p)
		{
			if (p.IsFormingCaravan())
			{
				return p.GetLord();
			}
			if (p.SpawnedOrAnyParentSpawned)
			{
				List<Lord> lords = p.MapHeld.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
					if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(p))
					{
						return lords[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06015626 RID: 87590 RVA: 0x00665874 File Offset: 0x00663A74
		public static float CapacityLeft(LordJob_FormAndSendCaravan lordJob)
		{
			float num = CollectionsMassCalculator.MassUsageTransferables(lordJob.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			for (int i = 0; i < lordJob.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lordJob.lord.ownedPawns[i];
				CaravanFormingUtility.tmpCaravanPawns.Add(new ThingCount(pawn, pawn.stackCount, false));
			}
			num += CollectionsMassCalculator.MassUsage(CaravanFormingUtility.tmpCaravanPawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			float num2 = CollectionsMassCalculator.Capacity(CaravanFormingUtility.tmpCaravanPawns, null);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			return num2 - num;
		}

		// Token: 0x06015627 RID: 87591 RVA: 0x00665905 File Offset: 0x00663B05
		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text += " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}

		// Token: 0x06015628 RID: 87592 RVA: 0x0066593C File Offset: 0x00663B3C
		public static bool AllItemsLoadedOntoCaravan(Lord lord, Map map)
		{
			bool flag = true;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsColonist && pawn.mindState.lastJobTag != JobTag.WaitingForOthersToFinishGatheringItems)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].CurJob != null && allPawnsSpawned[j].jobs.curDriver is JobDriver_PrepareCaravan_GatherItems && allPawnsSpawned[j].CurJob.lord == lord)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06015629 RID: 87593 RVA: 0x006659F0 File Offset: 0x00663BF0
		public static string GetForceDepartWarningMessage(LordJob_FormAndSendCaravan lordJob)
		{
			List<Transferable> list = new List<Transferable>();
			if (!CaravanFormingUtility.AllItemsLoadedOntoCaravan(lordJob.lord, lordJob.lord.Map))
			{
				foreach (TransferableOneWay transferableOneWay in lordJob.transferables)
				{
					if (transferableOneWay.CountToTransfer > 0)
					{
						list.Add(transferableOneWay);
					}
				}
			}
			List<ThingCount> list2 = new List<ThingCount>();
			List<Pawn> list3 = new List<Pawn>();
			foreach (Pawn pawn in lordJob.lord.ownedPawns)
			{
				if (pawn.Downed)
				{
					list3.Add(pawn);
				}
				else if (pawn.InMentalState)
				{
					list3.Add(pawn);
				}
				else
				{
					list2.Add(new ThingCount(pawn, pawn.stackCount, false));
				}
			}
			List<ThingCount> list4 = new List<ThingCount>();
			foreach (Pawn pawn2 in list3)
			{
				foreach (Thing thing in pawn2.inventory.innerContainer)
				{
					list4.Add(new ThingCount(thing, thing.stackCount, false));
				}
			}
			list4 = (from tc in list4
			group tc by tc.Thing.def into g
			select new ThingCount(g.First<ThingCount>().Thing, g.Sum((ThingCount tc) => tc.Count), true)).ToList<ThingCount>();
			TaggedString taggedString = "ConfirmForceDepartIntro".Translate();
			if (list.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartItemsStillLoading".Translate();
				taggedString += "\n\n" + (from i in list
				select i.LabelCap + " x" + i.CountToTransfer.ToString()).ToLineList("- ", false);
			}
			if (list3.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartPawnsNotLeaving".Translate();
				taggedString += "\n\n" + (from p in list3
				select p.LabelNoCountColored.ToString()).ToLineList("- ", false);
			}
			if (list4.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartItemsOnPawnsNotLeaving".Translate();
				taggedString += "\n\n" + (from i in list4
				select i.Thing.LabelCapNoCount + " x" + i.Count.ToString()).ToLineList("- ", false);
			}
			return taggedString;
		}

		// Token: 0x0400EE13 RID: 60947
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		// Token: 0x0400EE14 RID: 60948
		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		// Token: 0x0400EE15 RID: 60949
		private static readonly Texture2D ForceDepartCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/ForceCaravanToDepart", true);

		// Token: 0x0400EE16 RID: 60950
		private static List<Pawn> tmpPawnsInCancelledCaravan = new List<Pawn>();

		// Token: 0x0400EE17 RID: 60951
		private static List<Pawn> tmpRopers = new List<Pawn>();

		// Token: 0x0400EE18 RID: 60952
		private static List<Pawn> tmpNeedRoping = new List<Pawn>();

		// Token: 0x0400EE19 RID: 60953
		private static readonly List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400EE1A RID: 60954
		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();
	}
}
