using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompEquippableAbilityReloadable : CompEquippableAbility, IReloadableComp, ICompWithCharges
{
	[CompilerGenerated]
	private sealed class _003CCompGetEquippedGizmosExtra_003Ed__24 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompEquippableAbilityReloadable _003C_003E4__this;

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
		public _003CCompGetEquippedGizmosExtra_003Ed__24(int _003C_003E1__state)
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
				CompEquippableAbilityReloadable CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompEquippable)CS_0024_003C_003E8__locals5).CompGetEquippedGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals5.AbilityForReading == null)
					{
						return false;
					}
					if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals5.NeedsReload(allowForcedReload: false))
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Reload to full",
							action = delegate
							{
								CS_0024_003C_003E8__locals5.RemainingCharges = CS_0024_003C_003E8__locals5.MaxCharges;
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetEquippedGizmosExtra_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetEquippedGizmosExtra_003Ed__24(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__23 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompEquippableAbilityReloadable _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__23(int _003C_003E1__state)
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
				CompEquippableAbilityReloadable compEquippableAbilityReloadable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<StatDrawEntry> enumerable = ((ThingComp)compEquippableAbilityReloadable).SpecialDisplayStats();
					if (enumerable != null)
					{
						_003C_003E7__wrap1 = enumerable.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0079;
					}
					goto IL_0093;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0079;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0079:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_0093;
					IL_0093:
					if (compEquippableAbilityReloadable.AbilityForReading == null)
					{
						return false;
					}
					if (compEquippableAbilityReloadable.AbilityForReading.UsesCharges)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadChargesRemaining_Name".Translate(compEquippableAbilityReloadable.Props.ChargeNounArgument), compEquippableAbilityReloadable.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(compEquippableAbilityReloadable.Props.ChargeNounArgument), 5440);
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
			_003CSpecialDisplayStats_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private int replenishInTicks = -1;

	public CompProperties_EquippableAbilityReloadable Props => props as CompProperties_EquippableAbilityReloadable;

	public Thing ReloadableThing => parent;

	public ThingDef AmmoDef => Props.ammoDef;

	public int MaxCharges => Props.maxCharges;

	public int BaseReloadTicks => Props.baseReloadTicks;

	public string LabelRemaining => $"{RemainingCharges} / {MaxCharges}";

	public bool AbilityUsesCharges => base.AbilityForReading?.UsesCharges ?? false;

	public int RemainingCharges
	{
		get
		{
			return base.AbilityForReading?.RemainingCharges ?? 0;
		}
		set
		{
			if (base.AbilityForReading != null)
			{
				base.AbilityForReading.RemainingCharges = value;
			}
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref replenishInTicks, "replenishInTicks", -1);
	}

	public override void PostPostMake()
	{
		base.PostPostMake();
		Notify_PropsChanged();
	}

	public void Notify_PropsChanged()
	{
		if (base.AbilityForReading != null)
		{
			base.AbilityForReading.maxCharges = MaxCharges;
			RemainingCharges = MaxCharges;
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (base.AbilityForReading != null && Props.replenishAfterCooldown && RemainingCharges == 0)
		{
			if (replenishInTicks > 0)
			{
				replenishInTicks--;
			}
			else
			{
				RemainingCharges = MaxCharges;
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		if (base.AbilityForReading == null)
		{
			return null;
		}
		if (!AbilityUsesCharges)
		{
			return null;
		}
		return "ChargesRemaining".Translate(Props.ChargeNounArgument) + ": " + LabelRemaining;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__23))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetEquippedGizmosExtra_003Ed__24))]
	public override IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetEquippedGizmosExtra_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool NeedsReload(bool allowForcedReload)
	{
		if (base.AbilityForReading == null)
		{
			return false;
		}
		if (Props.ammoDef == null)
		{
			return false;
		}
		if (Props.ammoCountToRefill != 0)
		{
			if (!allowForcedReload)
			{
				return RemainingCharges == 0;
			}
			return RemainingCharges != MaxCharges;
		}
		return RemainingCharges != MaxCharges;
	}

	public void ReloadFrom(Thing ammo)
	{
		if (!NeedsReload(allowForcedReload: true))
		{
			return;
		}
		if (Props.ammoCountToRefill != 0)
		{
			if (ammo.stackCount < Props.ammoCountToRefill)
			{
				return;
			}
			ammo.SplitOff(Props.ammoCountToRefill).Destroy();
			RemainingCharges = MaxCharges;
		}
		else
		{
			if (ammo.stackCount < Props.ammoCountPerCharge)
			{
				return;
			}
			int num = Mathf.Clamp(ammo.stackCount / Props.ammoCountPerCharge, 0, MaxCharges - RemainingCharges);
			ammo.SplitOff(num * Props.ammoCountPerCharge).Destroy();
			RemainingCharges += num;
		}
		if (Props.soundReload != null)
		{
			Props.soundReload.PlayOneShot(new TargetInfo(parent.PositionHeld, parent.MapHeld));
		}
	}

	public int MinAmmoNeeded(bool allowForcedReload)
	{
		if (!NeedsReload(allowForcedReload))
		{
			return 0;
		}
		if (Props.ammoCountToRefill != 0)
		{
			return Props.ammoCountToRefill;
		}
		return Props.ammoCountPerCharge;
	}

	public int MaxAmmoNeeded(bool allowForcedReload)
	{
		if (!NeedsReload(allowForcedReload))
		{
			return 0;
		}
		if (Props.ammoCountToRefill != 0)
		{
			return Props.ammoCountToRefill;
		}
		return Props.ammoCountPerCharge * (MaxCharges - RemainingCharges);
	}

	public int MaxAmmoAmount()
	{
		if (base.AbilityForReading == null)
		{
			return 0;
		}
		if (Props.ammoDef == null)
		{
			return 0;
		}
		if (Props.ammoCountToRefill == 0)
		{
			return Props.ammoCountPerCharge * MaxCharges;
		}
		return Props.ammoCountToRefill;
	}

	public override void UsedOnce()
	{
		if (base.AbilityForReading != null && Props.replenishAfterCooldown && RemainingCharges == 0)
		{
			replenishInTicks = Props.baseReloadTicks;
		}
	}

	public bool CanBeUsed(out string reason)
	{
		reason = null;
		if (base.AbilityForReading == null)
		{
			return false;
		}
		if (RemainingCharges <= 0)
		{
			reason = DisabledReason(MinAmmoNeeded(allowForcedReload: false), MaxAmmoNeeded(allowForcedReload: false));
			return false;
		}
		return true;
	}

	public string DisabledReason(int minNeeded, int maxNeeded)
	{
		if (base.AbilityForReading == null)
		{
			return null;
		}
		if (Props.replenishAfterCooldown)
		{
			return "CommandReload_Cooldown".Translate(Props.CooldownVerbArgument, replenishInTicks.ToStringTicksToPeriod().Named("TIME"));
		}
		if (Props.ammoDef == null)
		{
			return "CommandReload_NoCharges".Translate(Props.ChargeNounArgument);
		}
		return TranslatorFormattedStringExtensions.Translate(arg3: ((Props.ammoCountToRefill == 0) ? ((minNeeded == maxNeeded) ? minNeeded.ToString() : $"{minNeeded}-{maxNeeded}") : Props.ammoCountToRefill.ToString()).Named("COUNT"), key: "CommandReload_NoAmmo", arg1: Props.ChargeNounArgument, arg2: NamedArgumentUtility.Named(Props.ammoDef, "AMMO"));
	}
}
