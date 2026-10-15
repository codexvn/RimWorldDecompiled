using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompRechargeable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRechargeable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__11(int _003C_003E1__state)
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
			CompRechargeable CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					break;
				}
				if (!CS_0024_003C_003E8__locals7.Charged)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Recharge",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals7.Props.chargedSoundDef != null)
							{
								CS_0024_003C_003E8__locals7.Props.chargedSoundDef.PlayOneShot(new TargetInfo(CS_0024_003C_003E8__locals7.parent.Position, CS_0024_003C_003E8__locals7.parent.Map));
							}
							CS_0024_003C_003E8__locals7.ticksUntilCharged = 0;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Discharge",
					action = CS_0024_003C_003E8__locals7.Discharge
				};
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__11(0)
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

	private int ticksUntilCharged;

	private Effecter progressBar;

	private CompPowerTrader compPowerCached;

	private CompProperties_Rechargeable Props => (CompProperties_Rechargeable)props;

	public bool Charged => ticksUntilCharged == 0;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		ticksUntilCharged = Props.ticksToRecharge;
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.WillReplace)
	{
		if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (compPowerCached == null)
		{
			compPowerCached = parent.TryGetComp<CompPowerTrader>();
		}
		if (ticksUntilCharged > 0 && compPowerCached.PowerOn)
		{
			if (ticksUntilCharged == 1 && Props.chargedSoundDef != null)
			{
				Props.chargedSoundDef.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
			}
			ticksUntilCharged--;
		}
		if (!Charged && parent.Spawned)
		{
			if (progressBar == null)
			{
				progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			progressBar.EffectTick(parent, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = 1f - Mathf.Clamp01((float)ticksUntilCharged / (float)Props.ticksToRecharge);
				mote.offsetZ = -0.5f;
			}
		}
		else if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
	}

	public void Discharge()
	{
		ticksUntilCharged = Props.ticksToRecharge;
		if (Props.dischargeSoundDef != null)
		{
			Props.dischargeSoundDef.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__11))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (Charged)
		{
			return "RechargeableReady".Translate() + ".";
		}
		return "RechargeableCharging".Translate() + ": " + "DurationLeft".Translate(ticksUntilCharged.ToStringTicksToPeriod(allowSeconds: true, shortForm: true)) + ".";
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ticksUntilCharged, "ticksUntilCharged", 0);
	}
}
