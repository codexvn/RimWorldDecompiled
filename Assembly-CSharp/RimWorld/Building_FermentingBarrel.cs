using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_FermentingBarrel : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__36 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_FermentingBarrel _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__36(int _003C_003E1__state)
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
				Building_FermentingBarrel CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals6).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e2;
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
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					if (!CS_0024_003C_003E8__locals6.Empty)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Set progress to 1",
							action = delegate
							{
								CS_0024_003C_003E8__locals6.Progress = 1f;
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e2;
					IL_00e2:
					if (CS_0024_003C_003E8__locals6.SpaceLeftForWort > 0)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Fill",
							action = delegate
							{
								CS_0024_003C_003E8__locals6.Progress = 1f;
								CS_0024_003C_003E8__locals6.wortCount = 25;
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__36(0)
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

	private int wortCount;

	private float progressInt;

	private Material barFilledCachedMat;

	public const int MaxCapacity = 25;

	private const int BaseFermentationDuration = 360000;

	public const float MinIdealTemperature = 7f;

	private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

	private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

	private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

	private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

	public float Progress
	{
		get
		{
			return progressInt;
		}
		set
		{
			if (value != progressInt)
			{
				progressInt = value;
				barFilledCachedMat = null;
			}
		}
	}

	private Material BarFilledMat
	{
		get
		{
			if (barFilledCachedMat == null)
			{
				barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(BarZeroProgressColor, BarFermentedColor, Progress));
			}
			return barFilledCachedMat;
		}
	}

	public int SpaceLeftForWort
	{
		get
		{
			if (!Fermented)
			{
				return 25 - wortCount;
			}
			return 0;
		}
	}

	private bool Empty => wortCount <= 0;

	public bool Fermented
	{
		get
		{
			if (!Empty)
			{
				return Progress >= 1f;
			}
			return false;
		}
	}

	private float CurrentTempProgressSpeedFactor
	{
		get
		{
			CompProperties_TemperatureRuinable compProperties = def.GetCompProperties<CompProperties_TemperatureRuinable>();
			float ambientTemperature = base.AmbientTemperature;
			if (ambientTemperature < compProperties.minSafeTemperature)
			{
				return 0.1f;
			}
			if (ambientTemperature < 7f)
			{
				return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
			}
			return 1f;
		}
	}

	private float ProgressPerTickAtCurrentTemp => 2.7777778E-06f * CurrentTempProgressSpeedFactor;

	private int EstimatedTicksLeft => Mathf.Max(Mathf.RoundToInt((1f - Progress) / ProgressPerTickAtCurrentTemp), 0);

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref wortCount, "wortCount", 0);
		Scribe_Values.Look(ref progressInt, "progress", 0f);
	}

	public override void TickRare()
	{
		base.TickRare();
		if (!Empty)
		{
			Progress = Mathf.Min(Progress + 250f * ProgressPerTickAtCurrentTemp, 1f);
		}
	}

	public void AddWort(int count)
	{
		GetComp<CompTemperatureRuinable>().Reset();
		if (Fermented)
		{
			Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.");
			return;
		}
		int num = Mathf.Min(count, 25 - wortCount);
		if (num > 0)
		{
			Progress = GenMath.WeightedAverage(0f, num, Progress, wortCount);
			wortCount += num;
		}
	}

	protected override void ReceiveCompSignal(string signal)
	{
		if (signal == "RuinedByTemperature")
		{
			Reset();
		}
	}

	private void Reset()
	{
		wortCount = 0;
		Progress = 0f;
	}

	public void AddWort(Thing wort)
	{
		int num = Mathf.Min(wort.stackCount, 25 - wortCount);
		if (num > 0)
		{
			AddWort(num);
			wort.SplitOff(num).Destroy();
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (stringBuilder.Length != 0)
		{
			stringBuilder.AppendLine();
		}
		CompTemperatureRuinable comp = GetComp<CompTemperatureRuinable>();
		if (!Empty && !comp.Ruined)
		{
			if (Fermented)
			{
				stringBuilder.AppendLine("ContainsBeer".Translate(wortCount, 25));
			}
			else
			{
				stringBuilder.AppendLine("ContainsWort".Translate(wortCount, 25));
			}
		}
		if (!Empty)
		{
			if (Fermented)
			{
				stringBuilder.AppendLine("Fermented".Translate());
			}
			else
			{
				stringBuilder.AppendLine("FermentationProgress".Translate(Progress.ToStringPercent(), EstimatedTicksLeft.ToStringTicksToPeriod()));
				if (CurrentTempProgressSpeedFactor != 1f)
				{
					stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(CurrentTempProgressSpeedFactor.ToStringPercent()));
				}
			}
		}
		stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
		stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
		return stringBuilder.ToString().TrimEndNewlines();
	}

	public Thing TakeOutBeer()
	{
		if (!Fermented)
		{
			Log.Warning("Tried to get beer but it's not yet fermented.");
			return null;
		}
		Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer);
		thing.stackCount = wortCount;
		Reset();
		return thing;
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (!Empty)
		{
			Vector3 center = drawLoc;
			center.y += 0.03658537f;
			center.z += 0.25f;
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = center;
			r.size = BarSize;
			r.fillPercent = (float)wortCount / 25f;
			r.filledMat = BarFilledMat;
			r.unfilledMat = BarUnfilledMat;
			r.margin = 0.1f;
			r.rotation = Rot4.North;
			GenDraw.DrawFillableBar(r);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__36))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}
}
