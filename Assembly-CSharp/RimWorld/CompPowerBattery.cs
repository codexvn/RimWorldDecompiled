using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompPowerBattery : CompPower
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPowerBattery _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__21(int _003C_003E1__state)
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
				CompPowerBattery CS_0024_003C_003E8__locals3 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompPower)CS_0024_003C_003E8__locals3).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Empty",
						action = delegate
						{
							CS_0024_003C_003E8__locals3.SetStoredEnergyPct(0f);
						}
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					{
						_003C_003E1__state = -1;
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
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Fill",
							action = delegate
							{
								CS_0024_003C_003E8__locals3.SetStoredEnergyPct(1f);
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
			_003CCompGetGizmosExtra_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__21(0)
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

	private float storedEnergy;

	private CompStunnable stunnableComp;

	private const float SelfDischargingWatts = 5f;

	public float AmountCanAccept
	{
		get
		{
			if (parent.IsBrokenDown() || StunnedByEMP)
			{
				return 0f;
			}
			CompProperties_Battery compProperties_Battery = Props;
			return (compProperties_Battery.storedEnergyMax - storedEnergy) / compProperties_Battery.efficiency;
		}
	}

	public float StoredEnergy => storedEnergy;

	public float StoredEnergyPct => storedEnergy / Props.storedEnergyMax;

	public new CompProperties_Battery Props => (CompProperties_Battery)props;

	public bool StunnedByEMP
	{
		get
		{
			if (stunnableComp != null)
			{
				if (stunnableComp.StunHandler.Stunned)
				{
					return stunnableComp.StunHandler.StunFromEMP;
				}
				return false;
			}
			return false;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		stunnableComp = parent.GetComp<CompStunnable>();
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref storedEnergy, "storedPower", 0f);
		CompProperties_Battery compProperties_Battery = Props;
		if (storedEnergy > compProperties_Battery.storedEnergyMax)
		{
			storedEnergy = compProperties_Battery.storedEnergyMax;
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, storedEnergy));
	}

	public void AddEnergy(float amount)
	{
		if (amount < 0f)
		{
			Log.Error("Cannot add negative energy " + amount);
		}
		else if (!StunnedByEMP)
		{
			if (amount > AmountCanAccept)
			{
				amount = AmountCanAccept;
			}
			amount *= Props.efficiency;
			storedEnergy += amount;
		}
	}

	public void DrawPower(float amount)
	{
		storedEnergy -= amount;
		if (storedEnergy < 0f)
		{
			Log.Error("Drawing power we don't have from " + parent);
			storedEnergy = 0f;
		}
	}

	public void SetStoredEnergyPct(float pct)
	{
		pct = Mathf.Clamp01(pct);
		storedEnergy = Props.storedEnergyMax * pct;
	}

	public override void ReceiveCompSignal(string signal)
	{
		if (signal == "Breakdown")
		{
			DrawPower(StoredEnergy);
		}
	}

	public override string CompInspectStringExtra()
	{
		CompProperties_Battery compProperties_Battery = Props;
		string text = "PowerBatteryStored".Translate() + ": " + storedEnergy.ToString("F0") + " / " + compProperties_Battery.storedEnergyMax.ToString("F0") + " Wd";
		text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (compProperties_Battery.efficiency * 100f).ToString("F0") + "%";
		if (storedEnergy > 0f)
		{
			text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
		}
		return text + "\n" + base.CompInspectStringExtra();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__21))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}
}
