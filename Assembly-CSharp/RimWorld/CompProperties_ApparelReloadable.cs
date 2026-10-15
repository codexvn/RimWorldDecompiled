using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_ApparelReloadable : CompProperties_ApparelVerbOwnerCharged
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__10 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_ApparelReloadable _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__10(int _003C_003E1__state)
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
				CompProperties_ApparelReloadable compProperties_ApparelReloadable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties_ApparelVerbOwner)compProperties_ApparelReloadable).ConfigErrors(parentDef).GetEnumerator();
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
					if (compProperties_ApparelReloadable.ammoDef != null && compProperties_ApparelReloadable.ammoCountToRefill == 0 && compProperties_ApparelReloadable.ammoCountPerCharge == 0)
					{
						_003C_003E2__current = "Reloadable component has ammoDef but one of ammoCountToRefill or ammoCountPerCharge must be set";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00cb;
					IL_00cb:
					if (compProperties_ApparelReloadable.ammoCountToRefill != 0 && compProperties_ApparelReloadable.ammoCountPerCharge != 0)
					{
						_003C_003E2__current = "Reloadable component: specify only one of ammoCountToRefill and ammoCountPerCharge";
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
			_003CConfigErrors_003Ed__10 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__10(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__11 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_ApparelReloadable _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__11(int _003C_003E1__state)
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
				CompProperties_ApparelReloadable compProperties_ApparelReloadable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_ApparelReloadable).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -1;
					goto IL_010e;
				case 3:
					_003C_003E1__state = -1;
					goto IL_024c;
				case 4:
					_003C_003E1__state = -1;
					goto IL_024c;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!req.HasThing)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadMaxCharges_Name".Translate(compProperties_ApparelReloadable.ChargeNounArgument), compProperties_ApparelReloadable.maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(compProperties_ApparelReloadable.ChargeNounArgument), 2749);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_010e;
					IL_024c:
					if (compProperties_ApparelReloadable.destroyOnEmpty)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadDestroyOnEmpty_Name".Translate(compProperties_ApparelReloadable.ChargeNounArgument), "Yes".Translate(), "Stat_Thing_ReloadDestroyOnEmpty_Desc".Translate(compProperties_ApparelReloadable.ChargeNounArgument), 2749);
						_003C_003E1__state = 5;
						return true;
					}
					break;
					IL_010e:
					if (compProperties_ApparelReloadable.ammoDef != null)
					{
						if (compProperties_ApparelReloadable.ammoCountToRefill != 0)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadRefill_Name".Translate(compProperties_ApparelReloadable.ChargeNounArgument), $"{compProperties_ApparelReloadable.ammoCountToRefill} {compProperties_ApparelReloadable.ammoDef.label}", "Stat_Thing_ReloadRefill_Desc".Translate(compProperties_ApparelReloadable.ChargeNounArgument), 2749);
							_003C_003E1__state = 3;
							return true;
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadPerCharge_Name".Translate(compProperties_ApparelReloadable.ChargeNounArgument), $"{compProperties_ApparelReloadable.ammoCountPerCharge} {compProperties_ApparelReloadable.ammoDef.label}", "Stat_Thing_ReloadPerCharge_Desc".Translate(compProperties_ApparelReloadable.ChargeNounArgument), 2749);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_024c;
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
			_003CSpecialDisplayStats_003Ed__11 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__11(0)
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

	public ThingDef ammoDef;

	public int ammoCountToRefill;

	public int ammoCountPerCharge;

	public int baseReloadTicks = 60;

	public bool replenishAfterCooldown;

	public SoundDef soundReload;

	[MustTranslate]
	public string cooldownGerund = "on cooldown";

	public NamedArgument CooldownVerbArgument => cooldownGerund.CapitalizeFirst().Named("COOLDOWNGERUND");

	public CompProperties_ApparelReloadable()
	{
		compClass = typeof(CompApparelReloadable);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__10))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__10(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__11))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
