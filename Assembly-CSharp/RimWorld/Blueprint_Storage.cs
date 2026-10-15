using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Blueprint_Storage : Blueprint_Build, IStorageGroupMember, IStoreSettingsParent
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__32 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Blueprint_Storage _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__32(int _003C_003E1__state)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Blueprint_Storage blueprint_Storage = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Blueprint_Build)blueprint_Storage).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00db;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(blueprint_Storage.GetStoreSettings()).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00db;
					IL_00db:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageGroupUtility.StorageGroupMemberGizmos(blueprint_Storage).GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally3();
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

		private void _003C_003Em__Finally3()
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
			_003CGetGizmos_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__32(0)
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
	private sealed class _003CGetInspectTabs_003Ed__28 : IEnumerable<InspectTabBase>, IEnumerable, IEnumerator<InspectTabBase>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private InspectTabBase _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Blueprint_Storage _003C_003E4__this;

		private List<InspectTabBase>.Enumerator _003C_003E7__wrap1;

		InspectTabBase IEnumerator<InspectTabBase>.Current
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
		public _003CGetInspectTabs_003Ed__28(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<InspectTabBase>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Blueprint_Storage blueprint_Storage = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
				}
				else
				{
					_003C_003E1__state = -1;
					if (blueprint_Storage.BuildDef.inspectorTabsResolved == null)
					{
						goto IL_009d;
					}
					_003C_003E7__wrap1 = blueprint_Storage.BuildDef.inspectorTabsResolved.GetEnumerator();
					_003C_003E1__state = -3;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					InspectTabBase current = _003C_003E7__wrap1.Current;
					if (current.VisibleInBlueprintMode)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<InspectTabBase>.Enumerator);
				goto IL_009d;
				IL_009d:
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
		IEnumerator<InspectTabBase> IEnumerable<InspectTabBase>.GetEnumerator()
		{
			_003CGetInspectTabs_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetInspectTabs_003Ed__28(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<InspectTabBase>)this).GetEnumerator();
		}
	}

	protected StorageGroup storageGroup;

	public StorageSettings settings;

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

	Map IStorageGroupMember.Map => base.Map;

	StorageSettings IStorageGroupMember.StoreSettings => GetStoreSettings();

	StorageSettings IStorageGroupMember.ParentStoreSettings => GetParentStoreSettings();

	StorageSettings IStorageGroupMember.ThingStoreSettings => settings;

	string IStorageGroupMember.StorageGroupTag => base.BuildDef.building.storageGroupTag;

	bool IStorageGroupMember.DrawConnectionOverlay => true;

	bool IStorageGroupMember.DrawStorageTab => true;

	bool IStorageGroupMember.ShowRenameButton => base.Faction == Faction.OfPlayer;

	bool IStoreSettingsParent.StorageTabVisible => true;

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
		return base.BuildDef.building.fixedStorageSettings;
	}

	void IStoreSettingsParent.Notify_SettingsChanged()
	{
	}

	public override void PostMake()
	{
		base.PostMake();
		settings = new StorageSettings(this);
		if (base.BuildDef.building.defaultStorageSettings != null)
		{
			settings.CopyFrom(base.BuildDef.building.defaultStorageSettings);
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		storageGroup?.RemoveMember(this);
		storageGroup = null;
		base.Destroy(mode);
	}

	[IteratorStateMachine(typeof(_003CGetInspectTabs_003Ed__28))]
	public override IEnumerable<InspectTabBase> GetInspectTabs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInspectTabs_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected override Thing MakeSolidThing(out bool shouldSelect)
	{
		Frame obj = (Frame)base.MakeSolidThing(out shouldSelect);
		obj.SetStorageGroup(storageGroup);
		obj.storageSettings = new StorageSettings();
		obj.storageSettings.CopyFrom(GetStoreSettings());
		this.SetStorageGroup(null, removeIfEmpty: false);
		return obj;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		StorageGroupUtility.DrawSelectionOverlaysFor(this);
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (storageGroup != null)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "LinkedStorageSettings".Translate() + ": ";
			text = ((storageGroup.MemberCount <= 1) ? ((string)(text + "OneBuilding".Translate().CapitalizeFirst())) : ((string)(text + "NumBuildings".Translate(storageGroup.MemberCount).CapitalizeFirst())));
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__32))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref storageGroup, "storageGroup");
		Scribe_Deep.Look(ref settings, "settings", this);
	}
}
