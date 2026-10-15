using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class GameCondition_Flashstorm : GameCondition
{
	[CompilerGenerated]
	private sealed class _003CGetPotentiallyAffectedCells_003Ed__21 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec2 center;

		public IntVec2 _003C_003E3__center;

		public GameCondition_Flashstorm _003C_003E4__this;

		private int _003Cx_003E5__2;

		private int _003Cz_003E5__3;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGetPotentiallyAffectedCells_003Ed__21(int _003C_003E1__state)
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
			GameCondition_Flashstorm gameCondition_Flashstorm = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00dd;
			}
			_003C_003E1__state = -1;
			_003Cx_003E5__2 = center.x - gameCondition_Flashstorm.areaRadius;
			goto IL_011a;
			IL_00ed:
			if (_003Cz_003E5__3 <= center.z + gameCondition_Flashstorm.areaRadius)
			{
				if ((center.x - _003Cx_003E5__2) * (center.x - _003Cx_003E5__2) + (center.z - _003Cz_003E5__3) * (center.z - _003Cz_003E5__3) <= gameCondition_Flashstorm.areaRadius * gameCondition_Flashstorm.areaRadius)
				{
					_003C_003E2__current = new IntVec3(_003Cx_003E5__2, 0, _003Cz_003E5__3);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00dd;
			}
			int num2 = _003Cx_003E5__2 + 1;
			_003Cx_003E5__2 = num2;
			goto IL_011a;
			IL_00dd:
			num2 = _003Cz_003E5__3 + 1;
			_003Cz_003E5__3 = num2;
			goto IL_00ed;
			IL_011a:
			if (_003Cx_003E5__2 <= center.x + gameCondition_Flashstorm.areaRadius)
			{
				_003Cz_003E5__3 = center.z - gameCondition_Flashstorm.areaRadius;
				goto IL_00ed;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetPotentiallyAffectedCells_003Ed__21 _003CGetPotentiallyAffectedCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPotentiallyAffectedCells_003Ed__ = this;
			}
			else
			{
				_003CGetPotentiallyAffectedCells_003Ed__ = new _003CGetPotentiallyAffectedCells_003Ed__21(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetPotentiallyAffectedCells_003Ed__.center = _003C_003E3__center;
			return _003CGetPotentiallyAffectedCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static readonly IntRange AreaRadiusRange = new IntRange(45, 60);

	private static readonly IntRange TicksBetweenStrikes = new IntRange(320, 800);

	private const int RainDisableTicksAfterConditionEnds = 30000;

	private const int AvoidConditionCauserExpandRect = 2;

	public IntVec2 centerLocation = IntVec2.Invalid;

	public IntRange areaRadiusOverride = IntRange.Zero;

	public IntRange initialStrikeDelay = IntRange.Zero;

	public bool ambientSound;

	private int areaRadius;

	private int nextLightningTicks;

	private Sustainer soundSustainer;

	public bool avoidConditionCauser;

	public int AreaRadius => areaRadius;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref centerLocation, "centerLocation");
		Scribe_Values.Look(ref areaRadius, "areaRadius", 0);
		Scribe_Values.Look(ref areaRadiusOverride, "areaRadiusOverride");
		Scribe_Values.Look(ref nextLightningTicks, "nextLightningTicks", 0);
		Scribe_Values.Look(ref initialStrikeDelay, "initialStrikeDelay");
		Scribe_Values.Look(ref ambientSound, "ambientSound", defaultValue: false);
		Scribe_Values.Look(ref avoidConditionCauser, "avoidConditionCauser", defaultValue: false);
	}

	public override void Init()
	{
		base.Init();
		areaRadius = ((areaRadiusOverride == IntRange.Zero) ? AreaRadiusRange.RandomInRange : areaRadiusOverride.RandomInRange);
		nextLightningTicks = Find.TickManager.TicksGame + initialStrikeDelay.RandomInRange;
		if (centerLocation.IsInvalid)
		{
			FindGoodCenterLocation();
		}
	}

	public override void GameConditionTick()
	{
		if (Find.TickManager.TicksGame > nextLightningTicks)
		{
			Vector2 vector = Rand.UnitVector2 * Rand.Range(0f, areaRadius);
			IntVec3 intVec = new IntVec3((int)Math.Round(vector.x) + centerLocation.x, 0, (int)Math.Round(vector.y) + centerLocation.z);
			if (IsGoodLocationForStrike(intVec))
			{
				base.SingleMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.SingleMap, intVec));
				nextLightningTicks = Find.TickManager.TicksGame + TicksBetweenStrikes.RandomInRange;
			}
		}
		if (ambientSound)
		{
			if (soundSustainer == null || soundSustainer.Ended)
			{
				soundSustainer = SoundDefOf.FlashstormAmbience.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(centerLocation.ToIntVec3, base.SingleMap), MaintenanceType.PerTick));
			}
			else
			{
				soundSustainer.Maintain();
			}
		}
	}

	public override void End()
	{
		base.SingleMap.weatherDecider.DisableRainFor(30000);
		base.End();
	}

	private void FindGoodCenterLocation()
	{
		if (base.SingleMap.Size.x <= 16 || base.SingleMap.Size.z <= 16)
		{
			throw new Exception("Map too small for flashstorm.");
		}
		for (int i = 0; i < 10; i++)
		{
			centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
			if (IsGoodCenterLocation(centerLocation))
			{
				break;
			}
		}
	}

	private bool IsGoodLocationForStrike(IntVec3 loc)
	{
		if (!loc.InBounds(base.SingleMap) || loc.Roofed(base.SingleMap) || !loc.Standable(base.SingleMap))
		{
			return false;
		}
		if (avoidConditionCauser && conditionCauser != null && conditionCauser.OccupiedRect().ExpandedBy(2).Contains(loc))
		{
			return false;
		}
		return true;
	}

	private bool IsGoodCenterLocation(IntVec2 loc)
	{
		int num = 0;
		int num2 = (int)((float)Math.PI * (float)areaRadius * (float)areaRadius / 2f);
		foreach (IntVec3 potentiallyAffectedCell in GetPotentiallyAffectedCells(loc))
		{
			if (IsGoodLocationForStrike(potentiallyAffectedCell))
			{
				num++;
			}
			if (num >= num2)
			{
				break;
			}
		}
		return num >= num2;
	}

	[IteratorStateMachine(typeof(_003CGetPotentiallyAffectedCells_003Ed__21))]
	private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPotentiallyAffectedCells_003Ed__21(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__center = center
		};
	}
}
