using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
{
	[CompilerGenerated]
	private sealed class _003CAllSlotCells_003Ed__21 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone_Stockpile _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CAllSlotCells_003Ed__21(int _003C_003E1__state)
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
			Zone_Stockpile zone_Stockpile = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < zone_Stockpile.cells.Count)
			{
				_003C_003E2__current = zone_Stockpile.cells[_003Ci_003E5__2];
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CAllSlotCells_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CAllSlotCells_003Ed__21(0)
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
	private sealed class _003CGetGizmos_003Ed__18 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone_Stockpile _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__18(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Zone_Stockpile zone_Stockpile = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Hide_ZoneStockpile(zone_Stockpile);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Zone)zone_Stockpile).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0097;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0097;
				case 3:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0097:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(zone_Stockpile.settings).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally2();
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

		private void _003C_003Em__Finally2()
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__18(0)
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

	[CompilerGenerated]
	private sealed class _003CGetZoneAddGizmos_003Ed__19 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetZoneAddGizmos_003Ed__19(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetZoneAddGizmos_003Ed__19(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public StorageSettings settings;

	public SlotGroup slotGroup;

	private static readonly ITab[] ITabs = new ITab[1]
	{
		new ITab_Storage()
	};

	public bool StorageTabVisible => true;

	public bool IgnoreStoredThingsBeauty => false;

	public bool HaulDestinationEnabled => true;

	protected override Color NextZoneColor => ZoneColorUtility.NextStorageZoneColor();

	public int SpaceRemaining => base.CellCount - base.HeldThingsCount;

	public string GroupingLabel => "StockpilePlural".Translate();

	public int GroupingOrder => -50;

	public Zone_Stockpile()
	{
		slotGroup = new SlotGroup(this);
	}

	public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager)
		: base(preset.PresetName(), zoneManager)
	{
		settings = new StorageSettings(this);
		settings.SetFromPreset(preset);
		slotGroup = new SlotGroup(this);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref settings, "settings", this);
	}

	public override void AddCell(IntVec3 sq)
	{
		base.AddCell(sq);
		slotGroup?.Notify_AddedCell(sq);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.Shelves, OpportunityType.GoodToKnow);
	}

	public override void RemoveCell(IntVec3 sq)
	{
		base.RemoveCell(sq);
		slotGroup.Notify_LostCell(sq);
	}

	public override void PostDeregister()
	{
		base.PostDeregister();
		BillUtility.Notify_ISlotGroupRemoved(slotGroup);
	}

	public override IEnumerable<InspectTabBase> GetInspectTabs()
	{
		return ITabs;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__18))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetZoneAddGizmos_003Ed__19))]
	public override IEnumerable<Gizmo> GetZoneAddGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetZoneAddGizmos_003Ed__19(-2);
	}

	public SlotGroup GetSlotGroup()
	{
		return slotGroup;
	}

	[IteratorStateMachine(typeof(_003CAllSlotCells_003Ed__21))]
	public IEnumerable<IntVec3> AllSlotCells()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllSlotCells_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public List<IntVec3> AllSlotCellsList()
	{
		return cells;
	}

	public StorageSettings GetParentStoreSettings()
	{
		return StorageSettings.EverStorableFixedSettings();
	}

	public StorageSettings GetStoreSettings()
	{
		return settings;
	}

	public void Notify_SettingsChanged()
	{
		if (base.Map != null && slotGroup != null)
		{
			base.Map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
		}
	}

	public bool Accepts(Thing t)
	{
		return settings.AllowedToAccept(t);
	}

	public string SlotYielderLabel()
	{
		return label;
	}

	public void Notify_ReceivedThing(Thing newItem)
	{
		if (newItem.def.storedConceptLearnOpportunity != null)
		{
			LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
		}
	}

	public void Notify_LostThing(Thing newItem)
	{
	}
}
