using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public sealed class HaulDestinationManager
{
	[CompilerGenerated]
	private sealed class _003Cget_AllSlots_003Ed__22 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HaulDestinationManager _003C_003E4__this;

		private int _003Ci_003E5__2;

		private List<IntVec3> _003CcellsList_003E5__3;

		private int _003Cj_003E5__4;

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
		public _003Cget_AllSlots_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcellsList_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			HaulDestinationManager haulDestinationManager = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0086;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_00b0;
			IL_0086:
			if (_003Cj_003E5__4 < haulDestinationManager.allGroupsInOrder.Count)
			{
				_003C_003E2__current = _003CcellsList_003E5__3[_003Cj_003E5__4];
				_003C_003E1__state = 1;
				return true;
			}
			_003CcellsList_003E5__3 = null;
			_003Ci_003E5__2++;
			goto IL_00b0;
			IL_00b0:
			if (_003Ci_003E5__2 < haulDestinationManager.allGroupsInOrder.Count)
			{
				_003CcellsList_003E5__3 = haulDestinationManager.allGroupsInOrder[_003Ci_003E5__2].CellsList;
				_003Cj_003E5__4 = 0;
				goto IL_0086;
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
			_003Cget_AllSlots_003Ed__22 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllSlots_003Ed__22(0)
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

	private Map map;

	private readonly List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

	private readonly List<IHaulSource> allHaulSourcesInOrder = new List<IHaulSource>();

	private readonly List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

	private readonly SlotGroup[,,] groupGrid;

	public IEnumerable<IHaulDestination> AllHaulDestinations => allHaulDestinationsInOrder;

	public List<IHaulDestination> AllHaulDestinationsListForReading => allHaulDestinationsInOrder;

	public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder => allHaulDestinationsInOrder;

	public List<IHaulSource> AllHaulSourcesListInPriorityOrder => allHaulSourcesInOrder;

	public IEnumerable<SlotGroup> AllGroups => allGroupsInOrder;

	public List<SlotGroup> AllGroupsListForReading => allGroupsInOrder;

	public List<IHaulSource> AllHaulSourcesListForReading => allHaulSourcesInOrder;

	public List<SlotGroup> AllGroupsListInPriorityOrder => allGroupsInOrder;

	public IEnumerable<IntVec3> AllSlots
	{
		[IteratorStateMachine(typeof(_003Cget_AllSlots_003Ed__22))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllSlots_003Ed__22(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public HaulDestinationManager(Map map)
	{
		this.map = map;
		groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
	}

	public void AddHaulDestination(IHaulDestination haulDestination)
	{
		if (allHaulDestinationsInOrder.Contains(haulDestination))
		{
			Log.Error("Double-added haul destination " + haulDestination.ToStringSafe());
			return;
		}
		allHaulDestinationsInOrder.Add(haulDestination);
		allHaulDestinationsInOrder.InsertionSort(CompareHaulDestinationPrioritiesDescending);
		if (!(haulDestination is ISlotGroupParent slotGroupParent))
		{
			return;
		}
		SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
		if (slotGroup == null)
		{
			Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe());
			return;
		}
		allGroupsInOrder.Add(slotGroup);
		allGroupsInOrder.InsertionSort(CompareSlotGroupPrioritiesDescending);
		List<IntVec3> cellsList = slotGroup.CellsList;
		for (int i = 0; i < cellsList.Count; i++)
		{
			SetCellFor(cellsList[i], slotGroup);
		}
		map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
		map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
	}

	public void RemoveHaulDestination(IHaulDestination haulDestination)
	{
		if (!allHaulDestinationsInOrder.Contains(haulDestination))
		{
			Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe());
			return;
		}
		allHaulDestinationsInOrder.Remove(haulDestination);
		if (!(haulDestination is ISlotGroupParent slotGroupParent))
		{
			return;
		}
		SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
		if (slotGroup == null)
		{
			Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe());
			return;
		}
		allGroupsInOrder.Remove(slotGroup);
		List<IntVec3> cellsList = slotGroup.CellsList;
		for (int i = 0; i < cellsList.Count; i++)
		{
			IntVec3 intVec = cellsList[i];
			groupGrid[intVec.x, intVec.y, intVec.z] = null;
		}
		map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
		map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
	}

	public void AddHaulSource(IHaulSource source)
	{
		if (allHaulSourcesInOrder.Contains(source))
		{
			Log.Error("Double-added haul destination " + source.ToStringSafe());
			return;
		}
		allHaulSourcesInOrder.Add(source);
		allHaulSourcesInOrder.InsertionSort(CompareHaulSourcePrioritiesDescending);
		map.listerHaulables.Notify_HaulSourceChanged(source);
	}

	public void RemoveHaulSource(IHaulSource source)
	{
		if (!allHaulSourcesInOrder.Remove(source))
		{
			Log.Error("Removing haul source that isn't registered " + source.ToStringSafe());
		}
	}

	public void Notify_HaulDestinationChangedPriority()
	{
		allHaulDestinationsInOrder.InsertionSort(CompareHaulDestinationPrioritiesDescending);
		allGroupsInOrder.InsertionSort(CompareSlotGroupPrioritiesDescending);
		allHaulSourcesInOrder.InsertionSort(CompareHaulSourcePrioritiesDescending);
	}

	private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
	{
		return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
	}

	private static int CompareHaulSourcePrioritiesDescending(IHaulSource a, IHaulSource b)
	{
		return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
	}

	private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
	{
		return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
	}

	public SlotGroup SlotGroupAt(IntVec3 loc)
	{
		return groupGrid[loc.x, loc.y, loc.z];
	}

	public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
	{
		return SlotGroupAt(loc)?.parent;
	}

	public void SetCellFor(IntVec3 c, SlotGroup group)
	{
		if (SlotGroupAt(c) != null)
		{
			string[] obj = new string[5]
			{
				group?.ToString(),
				" overwriting slot group square ",
				null,
				null,
				null
			};
			IntVec3 intVec = c;
			obj[2] = intVec.ToString();
			obj[3] = " of ";
			obj[4] = SlotGroupAt(c)?.ToString();
			Log.Error(string.Concat(obj));
		}
		groupGrid[c.x, c.y, c.z] = group;
	}

	public void ClearCellFor(IntVec3 c, SlotGroup group)
	{
		if (SlotGroupAt(c) != group)
		{
			string[] obj = new string[5]
			{
				group?.ToString(),
				" clearing group grid square ",
				null,
				null,
				null
			};
			IntVec3 intVec = c;
			obj[2] = intVec.ToString();
			obj[3] = " containing ";
			obj[4] = SlotGroupAt(c)?.ToString();
			Log.Error(string.Concat(obj));
		}
		groupGrid[c.x, c.y, c.z] = null;
	}
}
