using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorageGroup : IExposable, ILoadReferenceable, IStoreSettingsParent, ISlotGroup, IRenameable
{
	[CompilerGenerated]
	private sealed class _003Cget_HeldThings_003Ed__29 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorageGroup _003C_003E4__this;

		private List<IStorageGroupMember>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

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
		public _003Cget_HeldThings_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<IStorageGroupMember>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StorageGroup storageGroup = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_009a;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = storageGroup.members.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00b4;
				IL_009a:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Thing current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00b4;
				IL_00b4:
				ISlotGroupParent slotGroupParent;
				do
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						slotGroupParent = _003C_003E7__wrap1.Current as ISlotGroupParent;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<IStorageGroupMember>.Enumerator);
					return false;
				}
				while (slotGroupParent == null);
				_003C_003E7__wrap2 = slotGroupParent.GetSlotGroup().HeldThings.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_009a;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003Cget_HeldThings_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_HeldThings_003Ed__29(0)
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

	public int loadID;

	private Map map;

	[LoadAlias("buildings")]
	public List<IStorageGroupMember> members = new List<IStorageGroupMember>();

	private StorageSettings settings;

	private string label;

	private static readonly List<IntVec3> tmpCellsList = new List<IntVec3>(128);

	private static readonly List<IHaulSource> tmpHaulSourceList = new List<IHaulSource>(64);

	public string RenamableLabel
	{
		get
		{
			return label ?? BaseLabel;
		}
		set
		{
			label = value;
		}
	}

	public string BaseLabel => "StorageGroup".Translate();

	public string GroupingLabel => "GroupPlural".Translate();

	public int GroupingOrder => -100;

	public string InspectLabel => RenamableLabel;

	public Map Map => map;

	public int MemberCount => members.Count;

	public bool StorageTabVisible => true;

	StorageSettings ISlotGroup.Settings => GetStoreSettings();

	StorageGroup ISlotGroup.StorageGroup => this;

	public IEnumerable<Thing> HeldThings
	{
		[IteratorStateMachine(typeof(_003Cget_HeldThings_003Ed__29))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_HeldThings_003Ed__29(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public List<IntVec3> CellsList
	{
		get
		{
			tmpCellsList.Clear();
			foreach (IStorageGroupMember member in members)
			{
				if (member is ISlotGroupParent slotGroupParent)
				{
					tmpCellsList.AddRange(slotGroupParent.GetSlotGroup().CellsList);
				}
			}
			return tmpCellsList;
		}
	}

	public List<IHaulSource> HaulSourcesList
	{
		get
		{
			tmpHaulSourceList.Clear();
			foreach (IStorageGroupMember member in members)
			{
				if (member is IHaulSource item)
				{
					tmpHaulSourceList.Add(item);
				}
			}
			return tmpHaulSourceList;
		}
	}

	public StorageGroup()
	{
	}

	public StorageGroup(Map map, string label = null)
	{
		this.map = map;
		settings = new StorageSettings(this);
		RenamableLabel = label ?? StorageGroupManager.NewStorageName(BaseLabel);
	}

	public void InitFrom(IStorageGroupMember member)
	{
		settings.CopyFrom(member.StoreSettings);
	}

	public void RemoveMember(IStorageGroupMember member, bool removeIfEmpty = true)
	{
		if (members.Remove(member))
		{
			if (member is IStoreSettingsParent storeSettingsParent)
			{
				storeSettingsParent.Notify_SettingsChanged();
			}
			if (removeIfEmpty)
			{
				map.storageGroups.Notify_MemberRemoved(this);
			}
			BillUtility.Notify_ISlotGroupRemoved(this);
		}
	}

	public StorageSettings GetStoreSettings()
	{
		return settings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		if (members.Any())
		{
			return members[0].ParentStoreSettings;
		}
		return null;
	}

	public void Notify_SettingsChanged()
	{
		foreach (IStorageGroupMember member in members)
		{
			if (member is ISlotGroupParent slotGroupParent)
			{
				slotGroupParent.Notify_SettingsChanged();
			}
		}
	}

	public string GetUniqueLoadID()
	{
		return "StorageGroup_" + loadID;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref loadID, "loadID", 0);
		Scribe_Values.Look(ref label, "label");
		Scribe_References.Look(ref map, "map");
		Scribe_Collections.Look(ref members, "members", LookMode.Reference);
		Scribe_Deep.Look(ref settings, "settings", this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			members.RemoveAll((IStorageGroupMember x) => x == null);
		}
	}
}
