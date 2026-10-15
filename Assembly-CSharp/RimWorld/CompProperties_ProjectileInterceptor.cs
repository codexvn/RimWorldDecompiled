using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_ProjectileInterceptor : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__26 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_ProjectileInterceptor _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__26(int _003C_003E1__state)
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
				CompProperties_ProjectileInterceptor compProperties_ProjectileInterceptor = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_ProjectileInterceptor).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0075;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0075;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0075:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_ProjectileInterceptor.hitPoints > 0 && compProperties_ProjectileInterceptor.chargeIntervalTicks > 0)
					{
						_003C_003E2__current = "Cannot set both hitpoints and charge interval ticks.";
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__26 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__26(0)
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

	public float radius;

	public int cooldownTicks;

	public int disarmedByEmpForTicks;

	public bool interceptGroundProjectiles;

	public bool interceptAirProjectiles;

	public bool interceptNonHostileProjectiles;

	public bool interceptOutgoingProjectiles;

	public bool drawWithNoSelection;

	public int chargeIntervalTicks;

	public int chargeDurationTicks;

	public float minAlpha;

	public float idlePulseSpeed = 0.7f;

	public float minIdleAlpha = -1.7f;

	public int hitPoints = -1;

	public int rechargeHitPointsIntervalTicks = 240;

	[NoTranslate]
	public string gizmoTipKey;

	public bool hitPointsRestoreInstantlyAfterCharge;

	public bool startWithMaxHitPoints = true;

	public bool alwaysShowHitpointsGizmo;

	public bool activated;

	public int activeDuration;

	public Color color = Color.white;

	public EffecterDef reactivateEffect;

	public EffecterDef interceptEffect;

	public SoundDef activeSound;

	public CompProperties_ProjectileInterceptor()
	{
		compClass = typeof(CompProjectileInterceptor);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__26))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__26(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
