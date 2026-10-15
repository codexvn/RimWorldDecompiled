using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class SlotGroup : ISlotGroup
{
	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__17 : IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		public SlotGroup _003C_003E4__this;

		private List<IntVec3> _003CcellsList_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CGetEnumerator_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcellsList_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			SlotGroup slotGroup = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CcellsList_003E5__2 = slotGroup.CellsList;
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003CcellsList_003E5__2.Count)
			{
				_003C_003E2__current = _003CcellsList_003E5__2[_003Ci_003E5__3];
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
	}

	[CompilerGenerated]
	private sealed class _003Cget_HeldThings_003Ed__10 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SlotGroup _003C_003E4__this;

		private List<IntVec3> _003CcellsList_003E5__2;

		private Map _003Cmap_003E5__3;

		private int _003Ci_003E5__4;

		private List<Thing> _003CthingList_003E5__5;

		private int _003Cj_003E5__6;

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
		public _003Cget_HeldThings_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcellsList_003E5__2 = null;
			_003Cmap_003E5__3 = null;
			_003CthingList_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			SlotGroup slotGroup = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00ba;
			}
			_003C_003E1__state = -1;
			_003CcellsList_003E5__2 = slotGroup.CellsList;
			_003Cmap_003E5__3 = slotGroup.Map;
			_003Ci_003E5__4 = 0;
			goto IL_00f4;
			IL_00ca:
			if (_003Cj_003E5__6 < _003CthingList_003E5__5.Count)
			{
				if (_003CthingList_003E5__5[_003Cj_003E5__6].def.EverStorable(willMinifyIfPossible: false))
				{
					_003C_003E2__current = _003CthingList_003E5__5[_003Cj_003E5__6];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ba;
			}
			_003CthingList_003E5__5 = null;
			_003Ci_003E5__4++;
			goto IL_00f4;
			IL_00ba:
			_003Cj_003E5__6++;
			goto IL_00ca;
			IL_00f4:
			if (_003Ci_003E5__4 < _003CcellsList_003E5__2.Count)
			{
				_003CthingList_003E5__5 = _003Cmap_003E5__3.thingGrid.ThingsListAt(_003CcellsList_003E5__2[_003Ci_003E5__4]);
				_003Cj_003E5__6 = 0;
				goto IL_00ca;
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
			_003Cget_HeldThings_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_HeldThings_003Ed__10(0)
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

	public ISlotGroupParent parent;

	private Map Map => parent.Map;

	public StorageSettings Settings => parent.GetStoreSettings();

	public string GroupingLabel => parent.GroupingLabel;

	public int GroupingOrder => parent.GroupingOrder;

	public IEnumerable<Thing> HeldThings
	{
		[IteratorStateMachine(typeof(_003Cget_HeldThings_003Ed__10))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_HeldThings_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public int HeldThingsCount
	{
		get
		{
			int num = 0;
			List<IntVec3> cellsList = CellsList;
			ThingGrid thingGrid = Map.thingGrid;
			for (int i = 0; i < cellsList.Count; i++)
			{
				List<Thing> list = thingGrid.ThingsListAt(cellsList[i]);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].def.EverStorable(willMinifyIfPossible: false))
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	public List<IntVec3> CellsList => parent.AllSlotCellsList();

	public StorageGroup StorageGroup
	{
		get
		{
			if (parent is IStorageGroupMember storageGroupMember)
			{
				return storageGroupMember.Group;
			}
			return null;
		}
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__17))]
	public IEnumerator<IntVec3> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__17(0)
		{
			_003C_003E4__this = this
		};
	}

	public SlotGroup(ISlotGroupParent parent)
	{
		this.parent = parent;
	}

	public void Notify_AddedCell(IntVec3 c)
	{
		Map.haulDestinationManager.SetCellFor(c, this);
		Map.listerHaulables.RecalcAllInCell(c);
		Map.listerMergeables.RecalcAllInCell(c);
	}

	public void Notify_LostCell(IntVec3 c)
	{
		Map.haulDestinationManager.ClearCellFor(c, this);
		Map.listerHaulables.RecalcAllInCell(c);
		Map.listerMergeables.RecalcAllInCell(c);
	}

	public void RemoveHaulDesignationOnStoredThings()
	{
		if (parent.Map == null)
		{
			return;
		}
		foreach (Thing heldThing in HeldThings)
		{
			if (Settings.AllowedToAccept(heldThing))
			{
				Designation designation = Map.designationManager.DesignationOn(heldThing, DesignationDefOf.Haul);
				if (designation != null)
				{
					Map.designationManager.RemoveDesignation(designation);
				}
			}
		}
	}

	public override string ToString()
	{
		if (parent != null)
		{
			return parent.ToString();
		}
		return "NullParent";
	}

	public string GetName()
	{
		if (parent is Zone_Stockpile zone_Stockpile)
		{
			return zone_Stockpile.label;
		}
		if (parent is Building_Storage building_Storage)
		{
			return building_Storage.Label;
		}
		return "UnresolvedSlotGroupName";
	}

	public static string GetGroupLabel(ISlotGroup group)
	{
		if (group is SlotGroup slotGroup)
		{
			return slotGroup.parent.SlotYielderLabel();
		}
		if (group is IRenameable renameable)
		{
			return renameable.RenamableLabel;
		}
		return "UNABLE TO GET LABEL";
	}
}
