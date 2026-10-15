using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination, IStorageGroupMember, IHaulEnroute, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003CAllSlotCells_003Ed__32 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Storage _003C_003E4__this;

		private IEnumerator<IntVec3> _003C_003E7__wrap1;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CAllSlotCells_003Ed__32(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_Storage building_Storage = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!building_Storage.Spawned)
					{
						return false;
					}
					_003C_003E7__wrap1 = GenAdj.CellsOccupiedBy(building_Storage).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CAllSlotCells_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CAllSlotCells_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__54 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Storage _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__54(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_Storage building_Storage = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_Storage).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00df;
				case 3:
					_003C_003E1__state = -5;
					goto IL_0155;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_0231;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(building_Storage.GetStoreSettings()).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00df;
					IL_0155:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					if (Find.Selector.NumSelected != 1)
					{
						break;
					}
					_003C_003E7__wrap2 = building_Storage.slotGroup.HeldThings.GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_0231;
					IL_00df:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (!building_Storage.StorageTabVisible || building_Storage.MapHeld == null)
					{
						break;
					}
					_003C_003E7__wrap1 = StorageGroupUtility.StorageGroupMemberGizmos(building_Storage).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0155;
					IL_0231:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Thing current4 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = ContainingSelectionUtility.CreateSelectStorageGizmo("CommandSelectStoredThing".Translate(current4), ("CommandSelectStoredThingDesc".Translate() + "\n\n" + current4.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + current4.GetInspectString()).Resolve(), current4, current4, groupable: false);
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap2 = null;
					break;
				}
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__54 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__54(0)
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

	public StorageSettings settings;

	public StorageGroup storageGroup;

	public string label;

	public SlotGroup slotGroup;

	private List<IntVec3> cachedOccupiedCells;

	private static StringBuilder sb = new StringBuilder();

	StorageGroup IStorageGroupMember.Group
	{
		get
		{
			return storageGroup;
		}
		set
		{
			storageGroup = value;
		}
	}

	bool IStorageGroupMember.DrawConnectionOverlay => base.Spawned;

	Map IStorageGroupMember.Map => base.MapHeld;

	string IStorageGroupMember.StorageGroupTag => def.building.storageGroupTag;

	StorageSettings IStorageGroupMember.StoreSettings => GetStoreSettings();

	StorageSettings IStorageGroupMember.ParentStoreSettings => GetParentStoreSettings();

	StorageSettings IStorageGroupMember.ThingStoreSettings => settings;

	bool IStorageGroupMember.DrawStorageTab => true;

	bool IStorageGroupMember.ShowRenameButton => base.Faction == Faction.OfPlayer;

	public bool StorageTabVisible => true;

	public bool IgnoreStoredThingsBeauty => def.building.ignoreStoredThingsBeauty;

	public string GroupingLabel => def.building.groupingLabel;

	public int GroupingOrder => def.building.groupingOrder;

	public bool HaulDestinationEnabled => true;

	public Building_Storage()
	{
		slotGroup = new SlotGroup(this);
	}

	public SlotGroup GetSlotGroup()
	{
		return slotGroup;
	}

	public virtual void Notify_ReceivedThing(Thing newItem)
	{
		if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
		{
			LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
		}
	}

	public virtual void Notify_LostThing(Thing newItem)
	{
	}

	[IteratorStateMachine(typeof(_003CAllSlotCells_003Ed__32))]
	public virtual IEnumerable<IntVec3> AllSlotCells()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSlotCells_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	public List<IntVec3> AllSlotCellsList()
	{
		return cachedOccupiedCells ?? (cachedOccupiedCells = AllSlotCells().ToList());
	}

	public StorageSettings GetStoreSettings()
	{
		if (storageGroup != null)
		{
			return storageGroup.GetStoreSettings();
		}
		return settings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		StorageSettings fixedStorageSettings = def.building.fixedStorageSettings;
		if (fixedStorageSettings != null)
		{
			return fixedStorageSettings;
		}
		return StorageSettings.EverStorableFixedSettings();
	}

	public void Notify_SettingsChanged()
	{
		if (base.Spawned && slotGroup != null)
		{
			base.Map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
		}
	}

	public string SlotYielderLabel()
	{
		return LabelCap;
	}

	public bool Accepts(Thing t)
	{
		return GetStoreSettings().AllowedToAccept(t);
	}

	public int SpaceRemainingFor(ThingDef _)
	{
		return slotGroup.HeldThingsCount - def.building.maxItemsInCell * def.Size.Area;
	}

	public override void PostMake()
	{
		base.PostMake();
		settings = new StorageSettings(this);
		if (def.building.defaultStorageSettings != null)
		{
			settings.CopyFrom(def.building.defaultStorageSettings);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		cachedOccupiedCells = null;
		base.SpawnSetup(map, respawningAfterLoad);
		if (storageGroup != null && map != storageGroup.Map)
		{
			StorageSettings storeSettings = storageGroup.GetStoreSettings();
			storageGroup.RemoveMember(this);
			storageGroup = null;
			settings.CopyFrom(storeSettings);
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.DeSpawn(mode);
		cachedOccupiedCells = null;
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		base.Destroy(mode);
		if (storageGroup != null)
		{
			storageGroup?.RemoveMember(this);
			storageGroup = null;
		}
		BillUtility.Notify_ISlotGroupRemoved(slotGroup);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref settings, "settings", this);
		Scribe_References.Look(ref storageGroup, "storageGroup");
		Scribe_Values.Look(ref label, "label");
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		StorageGroupUtility.DrawSelectionOverlaysFor(this);
	}

	public override string GetInspectString()
	{
		sb.Clear();
		sb.Append(base.GetInspectString());
		if (base.Spawned)
		{
			if (storageGroup != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append($"{"StorageGroupLabel".Translate()}: {storageGroup.RenamableLabel.CapitalizeFirst()} ");
				if (storageGroup.MemberCount > 1)
				{
					sb.Append($"({"NumBuildings".Translate(storageGroup.MemberCount)})");
				}
				else
				{
					sb.Append($"({"OneBuilding".Translate()})");
				}
			}
			if (slotGroup.HeldThings.Any())
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("StoresThings".Translate());
				sb.Append(": ");
				sb.Append(slotGroup.HeldThings.Select((Thing x) => x.LabelShortCap).Distinct().ToCommaList());
				sb.Append(".");
			}
		}
		return sb.ToString();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__54))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__54(-2)
		{
			_003C_003E4__this = this
		};
	}
}
