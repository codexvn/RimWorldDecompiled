using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D5C RID: 15708
	public static class CaravanInventoryUtility
	{
		// Token: 0x06015651 RID: 87633 RVA: 0x006667A4 File Offset: 0x006649A4
		public static List<Thing> AllInventoryItems(Caravan caravan)
		{
			CaravanInventoryUtility.inventoryItems.Clear();
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				for (int j = pawn.inventory.innerContainer.Count - 1; j >= 0; j--)
				{
					Thing item = pawn.inventory.innerContainer[j];
					CaravanInventoryUtility.inventoryItems.Add(item);
				}
			}
			return CaravanInventoryUtility.inventoryItems;
		}

		// Token: 0x06015652 RID: 87634 RVA: 0x0066681C File Offset: 0x00664A1C
		public static Building_PassengerShuttle FindShuttle(Caravan caravan)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_PassengerShuttle building_PassengerShuttle = list[i] as Building_PassengerShuttle;
				if (building_PassengerShuttle != null)
				{
					return building_PassengerShuttle;
				}
			}
			return null;
		}

		// Token: 0x06015653 RID: 87635 RVA: 0x00666854 File Offset: 0x00664A54
		public static void CaravanInventoryUtilityStaticUpdate()
		{
			CaravanInventoryUtility.inventoryItems.Clear();
		}

		// Token: 0x06015654 RID: 87636 RVA: 0x00666860 File Offset: 0x00664A60
		public static Pawn GetOwnerOf(Caravan caravan, Thing item)
		{
			IThingHolder parentHolder = item.ParentHolder;
			if (parentHolder is Pawn_InventoryTracker)
			{
				Pawn pawn = (Pawn)parentHolder.ParentHolder;
				if (caravan.ContainsPawn(pawn))
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x06015655 RID: 87637 RVA: 0x00666894 File Offset: 0x00664A94
		public static bool TryGetBestFood(Caravan caravan, Pawn forPawn, out Thing food, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (CaravanPawnsNeedsUtility.CanEatForNutritionNow(thing2, forPawn))
				{
					float foodScore = CaravanPawnsNeedsUtility.GetFoodScore(thing2, forPawn);
					if (thing == null || foodScore > num)
					{
						thing = thing2;
						num = foodScore;
					}
				}
			}
			if (thing != null)
			{
				food = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			food = null;
			owner = null;
			return false;
		}

		// Token: 0x06015656 RID: 87638 RVA: 0x00666904 File Offset: 0x00664B04
		public static bool TryGetDrugToSatisfyChemicalNeed(Caravan caravan, Pawn forPawn, Hediff hediff, out Thing drug, out Pawn owner)
		{
			if (hediff == null)
			{
				drug = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Thing thing = null;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.IngestibleNow && thing2.def.IsDrug)
				{
					CompDrug compDrug = thing2.TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.chemical != null)
					{
						Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
						if (hediff_ChemicalDependency == null || compDrug.Props.chemical == hediff_ChemicalDependency.chemical)
						{
							Hediff_Addiction hediff_Addiction = hediff as Hediff_Addiction;
							if (hediff_Addiction == null || compDrug.Props.chemical.addictionHediff == hediff_Addiction.def)
							{
								Pawn_DrugPolicyTracker drugs = forPawn.drugs;
								DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
								if (drugPolicy == null || drugPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0)
								{
									thing = thing2;
									break;
								}
							}
						}
					}
				}
			}
			if (thing != null)
			{
				drug = thing;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing);
				return true;
			}
			drug = null;
			owner = null;
			return false;
		}

		// Token: 0x06015657 RID: 87639 RVA: 0x00666A34 File Offset: 0x00664C34
		public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
		{
			if (patient.playerSettings == null || patient.playerSettings.medCare <= MedicalCareCategory.NoMeds)
			{
				medicine = null;
				owner = null;
				return false;
			}
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			Medicine medicine2 = null;
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def.IsMedicine && patient.playerSettings.medCare.AllowsMedicine(thing.def))
				{
					float statValue = thing.GetStatValue(StatDefOf.MedicalPotency, true, -1);
					if (statValue > num || medicine2 == null)
					{
						num = statValue;
						medicine2 = (Medicine)thing;
					}
				}
			}
			if (medicine2 != null)
			{
				medicine = medicine2;
				owner = CaravanInventoryUtility.GetOwnerOf(caravan, medicine2);
				return true;
			}
			medicine = null;
			owner = null;
			return false;
		}

		// Token: 0x06015658 RID: 87640 RVA: 0x00666AEC File Offset: 0x00664CEC
		public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2.def == thingDef)
				{
					thing = thing2;
					owner = CaravanInventoryUtility.GetOwnerOf(caravan, thing2);
					return true;
				}
			}
			thing = null;
			owner = null;
			return false;
		}

		// Token: 0x06015659 RID: 87641 RVA: 0x00666B38 File Offset: 0x00664D38
		public static void MoveAllInventoryToSomeoneElse(Pawn from, List<Pawn> candidates, List<Pawn> ignoreCandidates = null)
		{
			CaravanInventoryUtility.inventoryToMove.Clear();
			CaravanInventoryUtility.inventoryToMove.AddRange(from.inventory.innerContainer);
			for (int i = 0; i < CaravanInventoryUtility.inventoryToMove.Count; i++)
			{
				CaravanInventoryUtility.MoveInventoryToSomeoneElse(from, CaravanInventoryUtility.inventoryToMove[i], candidates, ignoreCandidates, CaravanInventoryUtility.inventoryToMove[i].stackCount);
			}
			CaravanInventoryUtility.inventoryToMove.Clear();
		}

		// Token: 0x0601565A RID: 87642 RVA: 0x00666BA8 File Offset: 0x00664DA8
		public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
		{
			if (numToMove < 0 || numToMove > item.stackCount)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to move item ",
					(item != null) ? item.ToString() : null,
					" with numToMove=",
					numToMove.ToString(),
					" (item stack count = ",
					item.stackCount.ToString(),
					")"
				}));
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(item, candidates, ignoreCandidates, itemOwner);
			if (pawn == null)
			{
				return;
			}
			itemOwner.inventory.innerContainer.TryTransferToContainer(item, pawn.inventory.innerContainer, numToMove, true);
		}

		// Token: 0x0601565B RID: 87643 RVA: 0x00666C4C File Offset: 0x00664E4C
		public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
		{
			if (item is Pawn)
			{
				Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.");
				return null;
			}
			Pawn result;
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where CaravanInventoryUtility.CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			if ((from x in candidates
			where (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0601565C RID: 87644 RVA: 0x00666CDC File Offset: 0x00664EDC
		public static void MoveAllApparelToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates, bool moveLocked = true)
		{
			if (moveFrom.apparel == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpApparel.Clear();
			if (moveLocked)
			{
				CaravanInventoryUtility.tmpApparel.AddRange(moveFrom.apparel.WornApparel);
			}
			else
			{
				for (int i = 0; i < moveFrom.apparel.WornApparel.Count; i++)
				{
					Apparel apparel = moveFrom.apparel.WornApparel[i];
					if (!moveFrom.apparel.IsLocked(apparel))
					{
						CaravanInventoryUtility.tmpApparel.Add(apparel);
					}
				}
			}
			for (int j = 0; j < CaravanInventoryUtility.tmpApparel.Count; j++)
			{
				moveFrom.apparel.Remove(CaravanInventoryUtility.tmpApparel[j]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpApparel[j], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpApparel[j], true);
				}
			}
			CaravanInventoryUtility.tmpApparel.Clear();
		}

		// Token: 0x0601565D RID: 87645 RVA: 0x00666DC8 File Offset: 0x00664FC8
		public static void MoveAllEquipmentToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
		{
			if (moveFrom.equipment == null)
			{
				return;
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
			CaravanInventoryUtility.tmpEquipment.AddRange(moveFrom.equipment.AllEquipmentListForReading);
			for (int i = 0; i < CaravanInventoryUtility.tmpEquipment.Count; i++)
			{
				moveFrom.equipment.Remove(CaravanInventoryUtility.tmpEquipment[i]);
				Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(CaravanInventoryUtility.tmpEquipment[i], candidates, null, moveFrom);
				if (pawn != null)
				{
					pawn.inventory.innerContainer.TryAdd(CaravanInventoryUtility.tmpEquipment[i], true);
				}
			}
			CaravanInventoryUtility.tmpEquipment.Clear();
		}

		// Token: 0x0601565E RID: 87646 RVA: 0x00666E66 File Offset: 0x00665066
		private static bool CanMoveInventoryTo(Pawn pawn)
		{
			return MassUtility.CanEverCarryAnything(pawn);
		}

		// Token: 0x0601565F RID: 87647 RVA: 0x00666E70 File Offset: 0x00665070
		public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
		{
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan).ToList<Thing>())
			{
				int num = takeQuantity(thing);
				if (num > 0)
				{
					list.Add(thing.holdingOwner.Take(thing, num));
				}
			}
			return list;
		}

		// Token: 0x06015660 RID: 87648 RVA: 0x00666EE8 File Offset: 0x006650E8
		public static void GiveThing(Caravan caravan, Thing thing)
		{
			if (CaravanInventoryUtility.AllInventoryItems(caravan).Contains(thing))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to give the same item twice (",
					(thing != null) ? thing.ToString() : null,
					") to a caravan (",
					(caravan != null) ? caravan.ToString() : null,
					")."
				}));
				return;
			}
			Pawn pawn = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn == null)
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error(string.Format("Failed to give item {0} to caravan {1}; item was lost", thing, caravan));
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
		}

		// Token: 0x06015661 RID: 87649 RVA: 0x00666FA4 File Offset: 0x006651A4
		public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
		{
			int num = 0;
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.def == thingDef && (validator == null || validator(thing)))
				{
					num += thing.stackCount;
				}
			}
			return num >= count;
		}

		// Token: 0x06015662 RID: 87650 RVA: 0x00666FF8 File Offset: 0x006651F8
		public static IEnumerable<Thing> GetAllDissolvingThings(Caravan caravan)
		{
			ThingRequest group = ThingRequest.ForGroup(ThingRequestGroup.Dissolving);
			List<Thing> items = CaravanInventoryUtility.AllInventoryItems(caravan);
			int num;
			for (int i = 0; i < items.Count; i = num + 1)
			{
				if (group.Accepts(items[i]))
				{
					yield return items[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400EE3B RID: 60987
		private static List<Thing> inventoryItems = new List<Thing>();

		// Token: 0x0400EE3C RID: 60988
		private static List<Thing> inventoryToMove = new List<Thing>();

		// Token: 0x0400EE3D RID: 60989
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x0400EE3E RID: 60990
		private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();
	}
}
