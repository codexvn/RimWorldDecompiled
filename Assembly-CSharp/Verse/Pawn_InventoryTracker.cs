using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse;

[StaticConstructorOnStartup]
public class Pawn_InventoryTracker : IThingHolder, IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass44_0
	{
		public Thing drug;

		public Pawn_InventoryTracker _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__0()
		{
			FoodUtility.IngestFromInventoryNow(_003C_003E4__this.pawn, drug);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass44_1
	{
		public Thing drug;

		public Pawn_InventoryTracker _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__2()
		{
			FoodUtility.IngestFromInventoryNow(_003C_003E4__this.pawn, drug);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCombatEnhancingDrugs_003Ed__41 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_InventoryTracker _003C_003E4__this;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetCombatEnhancingDrugs_003Ed__41(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_InventoryTracker pawn_InventoryTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn_InventoryTracker.innerContainer.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					CompDrug compDrug = current.TryGetComp<CompDrug>();
					if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetCombatEnhancingDrugs_003Ed__41 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetCombatEnhancingDrugs_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetDrugs_003Ed__40 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_InventoryTracker _003C_003E4__this;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetDrugs_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_InventoryTracker pawn_InventoryTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = pawn_InventoryTracker.innerContainer.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					if (current.TryGetComp<CompDrug>() != null)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetDrugs_003Ed__40 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDrugs_003Ed__40(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__44 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_InventoryTracker _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__44(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Pawn_InventoryTracker CS_0024_003C_003E8__locals26 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals26.pawn.IsColonistPlayerControlled || !CS_0024_003C_003E8__locals26.pawn.Drafted || Find.Selector.SingleSelectedThing != CS_0024_003C_003E8__locals26.pawn)
				{
					break;
				}
				CS_0024_003C_003E8__locals26.usableDrugsTmp.Clear();
				foreach (Thing drug in CS_0024_003C_003E8__locals26.GetDrugs())
				{
					if (FoodUtility.WillIngestFromInventoryNow(CS_0024_003C_003E8__locals26.pawn, drug))
					{
						CS_0024_003C_003E8__locals26.usableDrugsTmp.Add(drug);
					}
				}
				if (CS_0024_003C_003E8__locals26.usableDrugsTmp.Count == 0)
				{
					return false;
				}
				if (CS_0024_003C_003E8__locals26.usableDrugsTmp.Count == 1)
				{
					_003C_003Ec__DisplayClass44_0 CS_0024_003C_003E8__locals24 = new _003C_003Ec__DisplayClass44_0
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals26,
						drug = CS_0024_003C_003E8__locals26.usableDrugsTmp[0]
					};
					string defaultLabel = (CS_0024_003C_003E8__locals24.drug.def.ingestible.ingestCommandString.NullOrEmpty() ? "ConsumeThing".Translate(CS_0024_003C_003E8__locals24.drug.LabelNoCount, CS_0024_003C_003E8__locals24.drug) : CS_0024_003C_003E8__locals24.drug.def.ingestible.ingestCommandString.Formatted(CS_0024_003C_003E8__locals24.drug.LabelShort));
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = defaultLabel,
						defaultDesc = CS_0024_003C_003E8__locals24.drug.LabelCapNoCount + ": " + CS_0024_003C_003E8__locals24.drug.def.description.CapitalizeFirst(),
						icon = CS_0024_003C_003E8__locals24.drug.def.uiIcon,
						iconAngle = CS_0024_003C_003E8__locals24.drug.def.uiIconAngle,
						iconOffset = CS_0024_003C_003E8__locals24.drug.def.uiIconOffset,
						action = delegate
						{
							FoodUtility.IngestFromInventoryNow(CS_0024_003C_003E8__locals24._003C_003E4__this.pawn, CS_0024_003C_003E8__locals24.drug);
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "TakeDrug".Translate(),
					defaultDesc = "TakeDrugDesc".Translate(),
					icon = DrugTex,
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						using (List<Thing>.Enumerator enumerator2 = CS_0024_003C_003E8__locals26.usableDrugsTmp.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								_003C_003Ec__DisplayClass44_1 CS_0024_003C_003E8__locals34 = new _003C_003Ec__DisplayClass44_1
								{
									_003C_003E4__this = CS_0024_003C_003E8__locals26,
									drug = enumerator2.Current
								};
								string label = (CS_0024_003C_003E8__locals34.drug.def.ingestible.ingestCommandString.NullOrEmpty() ? "ConsumeThing".Translate(CS_0024_003C_003E8__locals34.drug.LabelNoCount, CS_0024_003C_003E8__locals34.drug) : CS_0024_003C_003E8__locals34.drug.def.ingestible.ingestCommandString.Formatted(CS_0024_003C_003E8__locals34.drug.LabelShort));
								list.Add(new FloatMenuOption(label, delegate
								{
									FoodUtility.IngestFromInventoryNow(CS_0024_003C_003E8__locals34._003C_003E4__this.pawn, CS_0024_003C_003E8__locals34.drug);
								}));
							}
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__44 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__44(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public Pawn pawn;

	public ThingOwner<Thing> innerContainer;

	private bool unloadEverything;

	private List<Thing> itemsNotForSale = new List<Thing>();

	private List<Thing> unpackedCaravanItems = new List<Thing>();

	public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug");

	private static List<ThingDefCount> tmpItemsToKeep = new List<ThingDefCount>();

	private static readonly List<Thing> tmpThingList = new List<Thing>();

	private List<Thing> usableDrugsTmp = new List<Thing>();

	public bool UnloadEverything
	{
		get
		{
			if (unloadEverything)
			{
				return HasAnyUnloadableThing;
			}
			return false;
		}
		set
		{
			if (value && HasAnyUnloadableThing)
			{
				unloadEverything = true;
			}
			else
			{
				unloadEverything = false;
			}
		}
	}

	public bool HasAnyUnpackedCaravanItems => unpackedCaravanItems.Count > 0;

	private bool HasAnyUnloadableThing => FirstUnloadableThing != default(ThingCount);

	public ThingCount FirstUnloadableThing
	{
		get
		{
			if (innerContainer.Count == 0)
			{
				return default(ThingCount);
			}
			if (pawn.drugs?.CurrentPolicy != null)
			{
				DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
				tmpItemsToKeep.Clear();
				for (int i = 0; i < currentPolicy.Count; i++)
				{
					if (currentPolicy[i].takeToInventory > 0)
					{
						tmpItemsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
					}
				}
			}
			Pawn_InventoryStockTracker inventoryStock = pawn.inventoryStock;
			if (inventoryStock != null && inventoryStock.stockEntries?.Count > 0)
			{
				foreach (InventoryStockEntry value in pawn.inventoryStock.stockEntries.Values)
				{
					tmpItemsToKeep.Add(new ThingDefCount(value.thingDef, value.count));
				}
			}
			foreach (Thing item in innerContainer)
			{
				int num = -1;
				for (int j = 0; j < tmpItemsToKeep.Count; j++)
				{
					if (item.def == tmpItemsToKeep[j].ThingDef)
					{
						num = j;
						break;
					}
				}
				if (pawn.IsColonist && item.def.IsNutritionGivingIngestible && !item.def.IsDrug && JobGiver_PackFood.IsGoodPackableFoodFor(item, pawn, checkMass: false))
				{
					float inventoryPackableFoodNutrition = JobGiver_PackFood.GetInventoryPackableFoodNutrition(pawn);
					float maxLevel = pawn.needs.food.MaxLevel;
					if (inventoryPackableFoodNutrition - item.GetStatValue(StatDefOf.Nutrition) * (float)item.stackCount <= maxLevel)
					{
						int k;
						for (k = 0; inventoryPackableFoodNutrition - item.GetStatValue(StatDefOf.Nutrition) * (float)k > maxLevel; k++)
						{
						}
						if (item.stackCount - k > 0)
						{
							tmpItemsToKeep.Add(new ThingDefCount(item.def, item.stackCount - k));
							num = tmpItemsToKeep.Count - 1;
						}
					}
				}
				if (num < 0)
				{
					return new ThingCount(item, item.stackCount);
				}
				if (item.stackCount > tmpItemsToKeep[num].Count)
				{
					return new ThingCount(item, item.stackCount - tmpItemsToKeep[num].Count);
				}
				tmpItemsToKeep[num] = new ThingDefCount(tmpItemsToKeep[num].ThingDef, tmpItemsToKeep[num].Count - item.stackCount);
			}
			return default(ThingCount);
		}
	}

	public IThingHolder ParentHolder => pawn;

	public Pawn_InventoryTracker(Pawn pawn)
	{
		this.pawn = pawn;
		innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref itemsNotForSale, "itemsNotForSale", LookMode.Reference);
		Scribe_Collections.Look(ref unpackedCaravanItems, "unpackedCaravanItems", LookMode.Reference);
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref unloadEverything, "unloadEverything", defaultValue: false);
	}

	public void InventoryTrackerTick()
	{
		if (unloadEverything && !HasAnyUnloadableThing)
		{
			unloadEverything = false;
		}
	}

	public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
	{
		DropAllNearPawnHelper(pos, forbid, unforbid);
	}

	private void DropAllNearPawnHelper(IntVec3 pos, bool forbid = false, bool unforbid = false, bool caravanHaulOnly = false)
	{
		if (pawn.MapHeld == null)
		{
			Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + pawn);
			return;
		}
		tmpThingList.Clear();
		if (caravanHaulOnly)
		{
			tmpThingList.AddRange(unpackedCaravanItems);
		}
		else
		{
			tmpThingList.AddRange(innerContainer);
		}
		int i;
		for (i = 0; i < tmpThingList.Count; i++)
		{
			if (caravanHaulOnly && !innerContainer.Contains(tmpThingList[i]))
			{
				unpackedCaravanItems.Remove(tmpThingList[i]);
				Log.Warning("Could not drop unpacked caravan item " + tmpThingList[i].Label + ", inventory no longer contains it");
				continue;
			}
			innerContainer.TryDrop(tmpThingList[i], pos, pawn.MapHeld, ThingPlaceMode.Near, out var _, delegate(Thing t, int unused)
			{
				if (forbid)
				{
					t.SetForbiddenIfOutsideHomeArea();
				}
				if (unforbid)
				{
					t.SetForbidden(value: false, warnOnFail: false);
				}
				if (t.def.IsPleasureDrug)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
				}
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = CaravanFormingUtility.GetFormAndSendCaravanLord(pawn)?.LordJob as LordJob_FormAndSendCaravan;
				if (caravanHaulOnly && lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
				{
					CaravanFormingUtility.TryAddItemBackToTransferables(t, lordJob_FormAndSendCaravan.transferables, tmpThingList[i].stackCount);
				}
				unpackedCaravanItems.Remove(tmpThingList[i]);
			});
		}
	}

	public void DropCount(ThingDef def, int count, bool forbid = false, bool unforbid = false)
	{
		if (pawn.MapHeld == null)
		{
			Log.Error("Tried to drop a thing near pawn but the pawn is unspawned. pawn=" + pawn);
			return;
		}
		tmpThingList.Clear();
		tmpThingList.AddRange(innerContainer);
		int num = 0;
		for (int i = 0; i < tmpThingList.Count; i++)
		{
			Thing thing = tmpThingList[i];
			if (thing.def != def)
			{
				continue;
			}
			int num2 = Math.Min(thing.stackCount, count);
			innerContainer.TryDrop(tmpThingList[i], pawn.Position, pawn.MapHeld, ThingPlaceMode.Near, num2, out var _, delegate(Thing t, int unused)
			{
				if (forbid)
				{
					t.SetForbiddenIfOutsideHomeArea();
				}
				if (unforbid)
				{
					t.SetForbidden(value: false, warnOnFail: false);
				}
				if (t.def.IsPleasureDrug)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
				}
			});
			num += num2;
			if (num >= count)
			{
				break;
			}
		}
	}

	public void RemoveCount(ThingDef def, int count, bool destroy = true)
	{
		tmpThingList.Clear();
		tmpThingList.AddRange(innerContainer);
		foreach (Thing tmpThing in tmpThingList)
		{
			if (tmpThing.def != def)
			{
				continue;
			}
			if (tmpThing.stackCount > count)
			{
				tmpThing.stackCount -= count;
				break;
			}
			innerContainer.Remove(tmpThing);
			if (destroy)
			{
				tmpThing.Destroy();
			}
			break;
		}
	}

	public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
	{
		innerContainer.ClearAndDestroyContents(mode);
	}

	public bool Contains(Thing item)
	{
		return innerContainer.Contains(item);
	}

	public int Count(ThingDef def)
	{
		int num = 0;
		foreach (Thing item in innerContainer)
		{
			if (item.def == def)
			{
				num += item.stackCount;
			}
		}
		return num;
	}

	public int Count(Func<Thing, bool> validator)
	{
		int num = 0;
		foreach (Thing item in innerContainer)
		{
			if (validator(item))
			{
				num += item.stackCount;
			}
		}
		return num;
	}

	public void AddHauledCaravanItem(Thing item)
	{
		if (pawn.carryTracker.innerContainer.TryTransferToContainer(item, innerContainer, item.stackCount, out var resultingTransferredItem, canMergeWithExistingStacks: false) > 0)
		{
			unpackedCaravanItems.Add(resultingTransferredItem);
		}
		CompForbiddable compForbiddable = resultingTransferredItem?.TryGetComp<CompForbiddable>();
		if (compForbiddable != null)
		{
			compForbiddable.Forbidden = false;
		}
	}

	public void TryAddAndUnforbid(Thing item)
	{
		CompForbiddable compForbiddable = item.TryGetComp<CompForbiddable>();
		if (innerContainer.TryAdd(item) && compForbiddable != null)
		{
			compForbiddable.Forbidden = false;
		}
	}

	public void TransferCaravanItemsToCarrier(Pawn_InventoryTracker carrierInventory)
	{
		List<Thing> list = new List<Thing>();
		list.AddRange(pawn.inventory.unpackedCaravanItems);
		foreach (Thing item in list)
		{
			if (MassUtility.IsOverEncumbered(carrierInventory.pawn))
			{
				break;
			}
			if (innerContainer.Contains(item))
			{
				pawn.inventory.innerContainer.TryTransferToContainer(item, carrierInventory.innerContainer, item.stackCount);
			}
			unpackedCaravanItems.Remove(item);
		}
	}

	public void DropAllPackingCaravanThings()
	{
		if (pawn.Spawned)
		{
			DropAllNearPawnHelper(pawn.Position, forbid: false, unforbid: false, caravanHaulOnly: true);
			ClearHaulingCaravanCache();
		}
	}

	public void ClearHaulingCaravanCache()
	{
		unpackedCaravanItems.Clear();
	}

	public bool NotForSale(Thing item)
	{
		return itemsNotForSale.Contains(item);
	}

	public void TryAddItemNotForSale(Thing item)
	{
		if (innerContainer.TryAdd(item, canMergeWithExistingStacks: false))
		{
			itemsNotForSale.Add(item);
		}
	}

	public void Notify_ItemRemoved(Thing item)
	{
		itemsNotForSale.Remove(item);
		unpackedCaravanItems.Remove(item);
		if (unloadEverything && !HasAnyUnloadableThing)
		{
			unloadEverything = false;
		}
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	[IteratorStateMachine(typeof(_003CGetDrugs_003Ed__40))]
	public IEnumerable<Thing> GetDrugs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDrugs_003Ed__40(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetCombatEnhancingDrugs_003Ed__41))]
	public IEnumerable<Thing> GetCombatEnhancingDrugs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCombatEnhancingDrugs_003Ed__41(-2)
		{
			_003C_003E4__this = this
		};
	}

	public Thing FindCombatEnhancingDrug()
	{
		return GetCombatEnhancingDrugs().FirstOrDefault();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__44))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__44(-2)
		{
			_003C_003E4__this = this
		};
	}
}
