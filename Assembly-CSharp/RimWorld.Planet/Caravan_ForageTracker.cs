using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class Caravan_ForageTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__10 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Caravan_ForageTracker _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__10(int _003C_003E1__state)
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
			Caravan_ForageTracker @object = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Forage",
						action = @object.Forage
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
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
			_003CGetGizmos_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__10(0)
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

	private Caravan caravan;

	private float progress;

	private const int UpdateProgressIntervalTicks = 15;

	public (ThingDef food, float perDay) ForagedFoodPerDay => ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan);

	public string ForagedFoodPerDayExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			ForagedFoodPerDayCalculator.ForagedFoodPerDay(caravan, stringBuilder);
			return stringBuilder.ToString();
		}
	}

	public Caravan_ForageTracker(Caravan caravan)
	{
		this.caravan = caravan;
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref progress, "progress", 0f);
	}

	public void ForageTrackerTickInterval(int delta)
	{
		if (caravan.IsHashIntervalTick(15, delta))
		{
			UpdateProgressInterval();
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__10))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void UpdateProgressInterval()
	{
		float num = 15f * ForagedFoodPerDayCalculator.GetProgressPerTick(caravan);
		progress += num;
		if (progress >= 1f)
		{
			Forage();
			progress = 0f;
		}
	}

	private void Forage()
	{
		ThingDef foragedFood = caravan.Biome.foragedFood;
		if (foragedFood != null)
		{
			int a = GenMath.RoundRandom(ForagedFoodPerDayCalculator.GetForagedFoodCountPerInterval(caravan));
			int b = Mathf.FloorToInt((caravan.MassCapacity - caravan.MassUsage) / foragedFood.GetStatValueAbstract(StatDefOf.Mass));
			a = Mathf.Min(a, b);
			while (a > 0)
			{
				Thing thing = ThingMaker.MakeThing(foragedFood);
				thing.stackCount = Mathf.Min(a, foragedFood.stackLimit);
				a -= thing.stackCount;
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
		}
	}
}
