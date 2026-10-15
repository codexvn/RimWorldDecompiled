using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_Refuelable : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__41 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Refuelable _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__41(int _003C_003E1__state)
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
				CompProperties_Refuelable compProperties_Refuelable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Refuelable).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00cb;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_Refuelable.destroyOnNoFuel && compProperties_Refuelable.initialFuelPercent <= 0f)
					{
						_003C_003E2__current = "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00cb;
					IL_00cb:
					if ((!compProperties_Refuelable.consumeFuelOnlyWhenUsed || compProperties_Refuelable.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
					{
						_003C_003E2__current = $"Refuelable component set to consume fuel per tick, but parent tickertype is {parentDef.tickerType} instead of {1}";
						_003C_003E1__state = 3;
						return true;
					}
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__41 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__41(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__42 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Refuelable _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__42(int _003C_003E1__state)
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
				CompProperties_Refuelable compProperties_Refuelable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Refuelable).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (((ThingDef)req.Def).building.IsTurret)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "ShotsBeforeRearm".Translate(), ((int)compProperties_Refuelable.fuelCapacity).ToString(), "ShotsBeforeRearmExplanation".Translate(), 3171);
						_003C_003E1__state = 2;
						return true;
					}
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__42 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__42(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public float fuelConsumptionRate = 1f;

	public float fuelCapacity = 2f;

	public float initialFuelPercent;

	public float autoRefuelPercent = 0.3f;

	public float fuelConsumptionPerTickInRain;

	public ThingFilter fuelFilter;

	public bool destroyOnNoFuel;

	public bool consumeFuelOnlyWhenUsed;

	public bool consumeFuelOnlyWhenPowered;

	public bool showFuelGizmo;

	public bool initialAllowAutoRefuel = true;

	public bool showAllowAutoRefuelToggle;

	public bool allowRefuelIfNotEmpty = true;

	public bool fuelIsMortarBarrel;

	public bool targetFuelLevelConfigurable;

	public float initialConfigurableTargetFuelLevel;

	public bool drawOutOfFuelOverlay = true;

	public float minimumFueledThreshold;

	public bool drawFuelGaugeInMap;

	public bool atomicFueling;

	private float fuelMultiplier = 1f;

	public bool factorByDifficulty;

	[MustTranslate]
	public string fuelLabel;

	[MustTranslate]
	public string fuelGizmoLabel;

	[MustTranslate]
	public string outOfFuelMessage;

	[NoTranslate]
	public string fuelIconPath;

	public bool externalTicking;

	public bool hideGizmosIfNotPlayerFaction;

	public bool functionsInVacuum = true;

	public bool canEjectFuel;

	private Texture2D fuelIcon;

	public string FuelLabel
	{
		get
		{
			if (fuelLabel.NullOrEmpty())
			{
				return "Fuel".TranslateSimple();
			}
			return fuelLabel;
		}
	}

	public string FuelGizmoLabel
	{
		get
		{
			if (fuelGizmoLabel.NullOrEmpty())
			{
				return "Fuel".TranslateSimple();
			}
			return fuelGizmoLabel;
		}
	}

	public Texture2D FuelIcon
	{
		get
		{
			if (fuelIcon == null)
			{
				if (!fuelIconPath.NullOrEmpty())
				{
					fuelIcon = ContentFinder<Texture2D>.Get(fuelIconPath);
				}
				else
				{
					ThingDef thingDef = ((fuelFilter.AnyAllowedDef == null) ? ThingDefOf.Chemfuel : fuelFilter.AnyAllowedDef);
					fuelIcon = thingDef.uiIcon;
				}
			}
			return fuelIcon;
		}
	}

	public float FuelMultiplierCurrentDifficulty
	{
		get
		{
			if (factorByDifficulty && Find.Storyteller?.difficulty != null)
			{
				return fuelMultiplier / Find.Storyteller.difficulty.maintenanceCostFactor;
			}
			return fuelMultiplier;
		}
	}

	public CompProperties_Refuelable()
	{
		compClass = typeof(CompRefuelable);
	}

	public override void ResolveReferences(ThingDef parentDef)
	{
		base.ResolveReferences(parentDef);
		fuelFilter.ResolveReferences();
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__41))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__41(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__42))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__42(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
