using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public static class CaravanInventoryUtility
{
	[CompilerGenerated]
	private sealed class _003CGetAllDissolvingThings_003Ed__21 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		private ThingRequest _003Cgroup_003E5__2;

		private List<Thing> _003Citems_003E5__3;

		private int _003Ci_003E5__4;

		Thing IEnumerator<Thing>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetAllDissolvingThings_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cgroup_003E5__2 = default(ThingRequest);
			_003Citems_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0083;
			}
			_003C_003E1__state = -1;
			_003Cgroup_003E5__2 = ThingRequest.ForGroup(ThingRequestGroup.Dissolving);
			_003Citems_003E5__3 = AllInventoryItems(caravan);
			_003Ci_003E5__4 = 0;
			goto IL_0093;
			IL_0083:
			_003Ci_003E5__4++;
			goto IL_0093;
			IL_0093:
			if (_003Ci_003E5__4 < _003Citems_003E5__3.Count)
			{
				if (_003Cgroup_003E5__2.Accepts(_003Citems_003E5__3[_003Ci_003E5__4]))
				{
					_003C_003E2__current = _003Citems_003E5__3[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0083;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetAllDissolvingThings_003Ed__21 _003CGetAllDissolvingThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllDissolvingThings_003Ed__ = this;
			}
			else
			{
				_003CGetAllDissolvingThings_003Ed__ = new _003CGetAllDissolvingThings_003Ed__21(0);
			}
			_003CGetAllDissolvingThings_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetAllDissolvingThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private static List<Thing> inventoryItems = new List<Thing>();

	private static List<Thing> inventoryToMove = new List<Thing>();

	private static List<Apparel> tmpApparel = new List<Apparel>();

	private static List<ThingWithComps> tmpEquipment = new List<ThingWithComps>();

	public static List<Thing> AllInventoryItems(Caravan caravan)
	{
		inventoryItems.Clear();
		List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
		for (int i = 0; i < pawnsListForReading.Count; i++)
		{
			Pawn pawn = pawnsListForReading[i];
			for (int num = pawn.inventory.innerContainer.Count - 1; num >= 0; num--)
			{
				Thing item = pawn.inventory.innerContainer[num];
				inventoryItems.Add(item);
			}
		}
		return inventoryItems;
	}

	public static Building_PassengerShuttle FindShuttle(Caravan caravan)
	{
		List<Thing> list = AllInventoryItems(caravan);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] is Building_PassengerShuttle result)
			{
				return result;
			}
		}
		return null;
	}

	public static void CaravanInventoryUtilityStaticUpdate()
	{
		inventoryItems.Clear();
	}

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

	public static bool TryGetBestFood(Caravan caravan, Pawn forPawn, out Thing food, out Pawn owner)
	{
		List<Thing> list = AllInventoryItems(caravan);
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
			owner = GetOwnerOf(caravan, thing);
			return true;
		}
		food = null;
		owner = null;
		return false;
	}

	public static bool TryGetDrugToSatisfyChemicalNeed(Caravan caravan, Pawn forPawn, Hediff hediff, out Thing drug, out Pawn owner)
	{
		if (hediff == null)
		{
			drug = null;
			owner = null;
			return false;
		}
		List<Thing> list = AllInventoryItems(caravan);
		Thing thing = null;
		for (int i = 0; i < list.Count; i++)
		{
			Thing thing2 = list[i];
			if (!thing2.IngestibleNow || !thing2.def.IsDrug)
			{
				continue;
			}
			CompDrug compDrug = thing2.TryGetComp<CompDrug>();
			if (compDrug != null && compDrug.Props.chemical != null && (!(hediff is Hediff_ChemicalDependency hediff_ChemicalDependency) || compDrug.Props.chemical == hediff_ChemicalDependency.chemical) && (!(hediff is Hediff_Addiction hediff_Addiction) || compDrug.Props.chemical.addictionHediff == hediff_Addiction.def))
			{
				DrugPolicy drugPolicy = forPawn.drugs?.CurrentPolicy;
				if (drugPolicy == null || drugPolicy[thing2.def].allowedForAddiction || forPawn.story == null || forPawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0)
				{
					thing = thing2;
					break;
				}
			}
		}
		if (thing != null)
		{
			drug = thing;
			owner = GetOwnerOf(caravan, thing);
			return true;
		}
		drug = null;
		owner = null;
		return false;
	}

	public static bool TryGetBestMedicine(Caravan caravan, Pawn patient, out Medicine medicine, out Pawn owner)
	{
		if (patient.playerSettings == null || (int)patient.playerSettings.medCare <= 1)
		{
			medicine = null;
			owner = null;
			return false;
		}
		List<Thing> list = AllInventoryItems(caravan);
		Medicine medicine2 = null;
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			Thing thing = list[i];
			if (thing.def.IsMedicine && patient.playerSettings.medCare.AllowsMedicine(thing.def))
			{
				float statValue = thing.GetStatValue(StatDefOf.MedicalPotency);
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
			owner = GetOwnerOf(caravan, medicine2);
			return true;
		}
		medicine = null;
		owner = null;
		return false;
	}

	public static bool TryGetThingOfDef(Caravan caravan, ThingDef thingDef, out Thing thing, out Pawn owner)
	{
		List<Thing> list = AllInventoryItems(caravan);
		for (int i = 0; i < list.Count; i++)
		{
			Thing thing2 = list[i];
			if (thing2.def == thingDef)
			{
				thing = thing2;
				owner = GetOwnerOf(caravan, thing2);
				return true;
			}
		}
		thing = null;
		owner = null;
		return false;
	}

	public static void MoveAllInventoryToSomeoneElse(Pawn from, List<Pawn> candidates, List<Pawn> ignoreCandidates = null)
	{
		inventoryToMove.Clear();
		inventoryToMove.AddRange(from.inventory.innerContainer);
		for (int i = 0; i < inventoryToMove.Count; i++)
		{
			MoveInventoryToSomeoneElse(from, inventoryToMove[i], candidates, ignoreCandidates, inventoryToMove[i].stackCount);
		}
		inventoryToMove.Clear();
	}

	public static void MoveInventoryToSomeoneElse(Pawn itemOwner, Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, int numToMove)
	{
		if (numToMove < 0 || numToMove > item.stackCount)
		{
			Log.Warning("Tried to move item " + item?.ToString() + " with numToMove=" + numToMove + " (item stack count = " + item.stackCount + ")");
		}
		else
		{
			Pawn pawn = FindPawnToMoveInventoryTo(item, candidates, ignoreCandidates, itemOwner);
			if (pawn != null)
			{
				itemOwner.inventory.innerContainer.TryTransferToContainer(item, pawn.inventory.innerContainer, numToMove);
			}
		}
	}

	public static Pawn FindPawnToMoveInventoryTo(Thing item, List<Pawn> candidates, List<Pawn> ignoreCandidates, Pawn currentItemOwner = null)
	{
		if (item is Pawn)
		{
			Log.Error("Called FindPawnToMoveInventoryTo but the item is a pawn.");
			return null;
		}
		if (candidates.Where((Pawn x) => CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner && !MassUtility.IsOverEncumbered(x)).TryRandomElement(out var result))
		{
			return result;
		}
		if (candidates.Where((Pawn x) => CanMoveInventoryTo(x) && (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner).TryRandomElement(out result))
		{
			return result;
		}
		if (candidates.Where((Pawn x) => (ignoreCandidates == null || !ignoreCandidates.Contains(x)) && x != currentItemOwner).TryRandomElement(out result))
		{
			return result;
		}
		return null;
	}

	public static void MoveAllApparelToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates, bool moveLocked = true)
	{
		if (moveFrom.apparel == null)
		{
			return;
		}
		tmpApparel.Clear();
		if (moveLocked)
		{
			tmpApparel.AddRange(moveFrom.apparel.WornApparel);
		}
		else
		{
			for (int i = 0; i < moveFrom.apparel.WornApparel.Count; i++)
			{
				Apparel apparel = moveFrom.apparel.WornApparel[i];
				if (!moveFrom.apparel.IsLocked(apparel))
				{
					tmpApparel.Add(apparel);
				}
			}
		}
		for (int j = 0; j < tmpApparel.Count; j++)
		{
			moveFrom.apparel.Remove(tmpApparel[j]);
			FindPawnToMoveInventoryTo(tmpApparel[j], candidates, null, moveFrom)?.inventory.innerContainer.TryAdd(tmpApparel[j]);
		}
		tmpApparel.Clear();
	}

	public static void MoveAllEquipmentToSomeonesInventory(Pawn moveFrom, List<Pawn> candidates)
	{
		if (moveFrom.equipment != null)
		{
			tmpEquipment.Clear();
			tmpEquipment.AddRange(moveFrom.equipment.AllEquipmentListForReading);
			for (int i = 0; i < tmpEquipment.Count; i++)
			{
				moveFrom.equipment.Remove(tmpEquipment[i]);
				FindPawnToMoveInventoryTo(tmpEquipment[i], candidates, null, moveFrom)?.inventory.innerContainer.TryAdd(tmpEquipment[i]);
			}
			tmpEquipment.Clear();
		}
	}

	private static bool CanMoveInventoryTo(Pawn pawn)
	{
		return MassUtility.CanEverCarryAnything(pawn);
	}

	public static List<Thing> TakeThings(Caravan caravan, Func<Thing, int> takeQuantity)
	{
		List<Thing> list = new List<Thing>();
		foreach (Thing item in AllInventoryItems(caravan).ToList())
		{
			int num = takeQuantity(item);
			if (num > 0)
			{
				list.Add(item.holdingOwner.Take(item, num));
			}
		}
		return list;
	}

	public static void GiveThing(Caravan caravan, Thing thing)
	{
		if (AllInventoryItems(caravan).Contains(thing))
		{
			Log.Error("Tried to give the same item twice (" + thing?.ToString() + ") to a caravan (" + caravan?.ToString() + ").");
			return;
		}
		Pawn pawn = FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null);
		if (pawn == null)
		{
			Log.Error($"Failed to give item {thing} to caravan {caravan}; item was lost");
			thing.Destroy();
		}
		else if (!pawn.inventory.innerContainer.TryAdd(thing))
		{
			Log.Error($"Failed to give item {thing} to caravan {caravan}; item was lost");
			thing.Destroy();
		}
	}

	public static bool HasThings(Caravan caravan, ThingDef thingDef, int count, Func<Thing, bool> validator = null)
	{
		int num = 0;
		List<Thing> list = AllInventoryItems(caravan);
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

	[IteratorStateMachine(typeof(_003CGetAllDissolvingThings_003Ed__21))]
	public static IEnumerable<Thing> GetAllDissolvingThings(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllDissolvingThings_003Ed__21(-2)
		{
			_003C_003E3__caravan = caravan
		};
	}
}
