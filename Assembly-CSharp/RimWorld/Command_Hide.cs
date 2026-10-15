using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public abstract class Command_Hide : Command_Toggle
{
	[CompilerGenerated]
	private sealed class _003CZoneTypeOptions_003Ed__9<T> : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable where T : Zone
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string label;

		public string _003C_003E3__label;

		private Texture2D zoneIcon;

		public Texture2D _003C_003E3__zoneIcon;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CZoneTypeOptions_003Ed__9(int _003C_003E1__state)
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
				_003C_003E2__current = new FloatMenuOption("ShowAllZoneTypes".Translate(label), delegate
				{
					ToggleZones<T>(hidden: false);
				}, zoneIcon, Color.white);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("HideAllZoneTypes".Translate(label), delegate
				{
					ToggleZones<T>(hidden: true);
				}, zoneIcon, Color.gray);
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CZoneTypeOptions_003Ed__9<T> _003CZoneTypeOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CZoneTypeOptions_003Ed__ = this;
			}
			else
			{
				_003CZoneTypeOptions_003Ed__ = new _003CZoneTypeOptions_003Ed__9<T>(0);
			}
			_003CZoneTypeOptions_003Ed__.label = _003C_003E3__label;
			_003CZoneTypeOptions_003Ed__.zoneIcon = _003C_003E3__zoneIcon;
			return _003CZoneTypeOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_RightClickFloatMenuOptions_003Ed__8 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Command_Hide _003C_003E4__this;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003Cget_RightClickFloatMenuOptions_003Ed__8(int _003C_003E1__state)
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
				Command_Hide command_Hide = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = command_Hide.GetOptions().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003Cget_RightClickFloatMenuOptions_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RightClickFloatMenuOptions_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected readonly IHideable hideable;

	public IHideable Hideable => hideable;

	public override string Label => Hideable.Hidden ? "CommandUnhideLabel".Translate() : "CommandHideLabel".Translate();

	public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
	{
		[IteratorStateMachine(typeof(_003Cget_RightClickFloatMenuOptions_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RightClickFloatMenuOptions_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Command_Hide(IHideable hideable)
	{
		this.hideable = hideable;
		icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone");
		defaultDesc = "CommandHideZoneDesc".Translate();
		hotKey = KeyBindingDefOf.Misc2;
		isActive = () => hideable.Hidden;
		toggleAction = delegate
		{
			hideable.Hidden = !hideable.Hidden;
		};
	}

	protected abstract IEnumerable<FloatMenuOption> GetOptions();

	[IteratorStateMachine(typeof(_003CZoneTypeOptions_003Ed__9<>))]
	protected static IEnumerable<FloatMenuOption> ZoneTypeOptions<T>(string label, Texture2D zoneIcon) where T : Zone
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CZoneTypeOptions_003Ed__9<T>(-2)
		{
			_003C_003E3__label = label,
			_003C_003E3__zoneIcon = zoneIcon
		};
	}

	protected static void ToggleAll(bool hidden)
	{
		foreach (Zone allZone in Find.CurrentMap.zoneManager.AllZones)
		{
			allZone.Hidden = hidden;
		}
		TogglePlans(hidden);
	}

	protected static void TogglePlans(bool hidden)
	{
		foreach (Plan allPlan in Find.CurrentMap.planManager.AllPlans)
		{
			allPlan.Hidden = hidden;
		}
	}

	protected static void ToggleZones<T>(bool hidden) where T : Zone
	{
		foreach (Zone allZone in Find.CurrentMap.zoneManager.AllZones)
		{
			if (allZone is T)
			{
				allZone.Hidden = hidden;
			}
		}
	}
}
