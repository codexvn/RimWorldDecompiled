using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompTempControl : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public float offset;

		public CompTempControl _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__0()
		{
			_003C_003E4__this.InterfaceChangeTargetTemperature(offset);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_1
	{
		public float offset;

		public CompTempControl _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			_003C_003E4__this.InterfaceChangeTargetTemperature(offset);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_2
	{
		public float offset;

		public CompTempControl _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__3()
		{
			_003C_003E4__this.InterfaceChangeTargetTemperature(offset);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_3
	{
		public float offset;

		public CompTempControl _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__4()
		{
			_003C_003E4__this.InterfaceChangeTargetTemperature(offset);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__14 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTempControl _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__14(int _003C_003E1__state)
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
				CompTempControl CS_0024_003C_003E8__locals23 = _003C_003E4__this;
				_003C_003Ec__DisplayClass14_0 CS_0024_003C_003E8__locals27;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals23).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass14_1 CS_0024_003C_003E8__locals20 = new _003C_003Ec__DisplayClass14_1
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals23,
						offset = CS_0024_003C_003E8__locals23.RoundedToCurrentTempModeOffset(-1f)
					};
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals20._003C_003E4__this.InterfaceChangeTargetTemperature(CS_0024_003C_003E8__locals20.offset);
						},
						defaultLabel = CS_0024_003C_003E8__locals20.offset.ToStringTemperatureOffset("F0"),
						defaultDesc = "CommandLowerTempDesc".Translate(),
						hotKey = KeyBindingDefOf.Misc4,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
					};
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals23.TargetTemperature = 21f;
							SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
							CS_0024_003C_003E8__locals23.ThrowCurrentTemperatureText();
						},
						defaultLabel = "CommandResetTemp".Translate(),
						defaultDesc = "CommandResetTempDesc".Translate(),
						hotKey = KeyBindingDefOf.Misc1,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset")
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass14_2 CS_0024_003C_003E8__locals12 = new _003C_003Ec__DisplayClass14_2
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals23,
						offset = CS_0024_003C_003E8__locals23.RoundedToCurrentTempModeOffset(1f)
					};
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals12._003C_003E4__this.InterfaceChangeTargetTemperature(CS_0024_003C_003E8__locals12.offset);
						},
						defaultLabel = "+" + CS_0024_003C_003E8__locals12.offset.ToStringTemperatureOffset("F0"),
						defaultDesc = "CommandRaiseTempDesc".Translate(),
						hotKey = KeyBindingDefOf.Misc2,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
					};
					_003C_003E1__state = 5;
					return true;
				}
				case 5:
				{
					_003C_003E1__state = -1;
					_003C_003Ec__DisplayClass14_3 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass14_3
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals23,
						offset = CS_0024_003C_003E8__locals23.RoundedToCurrentTempModeOffset(10f)
					};
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals6._003C_003E4__this.InterfaceChangeTargetTemperature(CS_0024_003C_003E8__locals6.offset);
						},
						defaultLabel = "+" + CS_0024_003C_003E8__locals6.offset.ToStringTemperatureOffset("F0"),
						defaultDesc = "CommandRaiseTempDesc".Translate(),
						hotKey = KeyBindingDefOf.Misc3,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
					};
					_003C_003E1__state = 6;
					return true;
				}
				case 6:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass14_0
					{
						_003C_003E4__this = CS_0024_003C_003E8__locals23,
						offset = CS_0024_003C_003E8__locals23.RoundedToCurrentTempModeOffset(-10f)
					};
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals27._003C_003E4__this.InterfaceChangeTargetTemperature(CS_0024_003C_003E8__locals27.offset);
						},
						defaultLabel = CS_0024_003C_003E8__locals27.offset.ToStringTemperatureOffset("F0"),
						defaultDesc = "CommandLowerTempDesc".Translate(),
						hotKey = KeyBindingDefOf.Misc5,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
					};
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CCompGetGizmosExtra_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__14(0)
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

	protected const float DefaultTargetTemperature = 21f;

	public bool operatingAtHighPower;

	private CompPowerTrader intPowerTrader;

	public float targetTemperature = -99999f;

	public virtual float TargetTemperature
	{
		get
		{
			return targetTemperature;
		}
		set
		{
			targetTemperature = value;
		}
	}

	public CompProperties_TempControl Props => (CompProperties_TempControl)props;

	public CompPowerTrader PowerTrader => intPowerTrader ?? (intPowerTrader = parent.GetComp<CompPowerTrader>());

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (TargetTemperature < -2000f)
		{
			TargetTemperature = Props.defaultTargetTemperature;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref targetTemperature, "targetTemperature", 0f);
	}

	protected float RoundedToCurrentTempModeOffset(float celsiusTemp)
	{
		return GenTemperature.ConvertTemperatureOffset(Mathf.RoundToInt(GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode)), Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__14))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected void InterfaceChangeTargetTemperature(float offset)
	{
		SoundDefOf.DragSlider.PlayOneShotOnCamera();
		TargetTemperature += offset;
		TargetTemperature = Mathf.Clamp(TargetTemperature, -273.15f, 1000f);
		ThrowCurrentTemperatureText();
	}

	protected void ThrowCurrentTemperatureText()
	{
		MoteMaker.ThrowText(parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), parent.Map, TargetTemperature.ToStringTemperature("F0"), Color.white);
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append((Props.inspectString ?? ((string)"TargetTemperature".Translate())) + ": ");
		stringBuilder.AppendLine(TargetTemperature.ToStringTemperature("F0"));
		if (PowerTrader != null)
		{
			string str = (PowerTrader.Off ? ((string)"PowerConsumptionOff".Translate()) : (operatingAtHighPower ? ((string)"PowerConsumptionHigh".Translate()) : ((string)"PowerConsumptionLow".Translate())));
			stringBuilder.Append($"{"PowerConsumptionMode".Translate()}: {str.CapitalizeFirst()}");
		}
		return stringBuilder.ToString();
	}
}
