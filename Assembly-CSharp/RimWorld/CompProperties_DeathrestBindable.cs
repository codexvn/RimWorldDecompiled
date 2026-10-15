using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_DeathrestBindable : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__11 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_DeathrestBindable _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompProperties_DeathrestBindable compProperties_DeathrestBindable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Mathf.Approximately(compProperties_DeathrestBindable.deathrestEffectivenessFactor, 1f))
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_DeathrestEffectiveness".Translate(), compProperties_DeathrestBindable.deathrestEffectivenessFactor.ToStringPercent(), "StatsReport_DeathrestEffectiveness_Desc".Translate(), 900);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_008d;
			case 1:
				_003C_003E1__state = -1;
				goto IL_008d;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_008d:
				if (!compProperties_DeathrestBindable.mustBeLayingInToBind)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_DeathrestConnectionLimit".Translate(), (compProperties_DeathrestBindable.stackLimit <= 0) ? "Unlimited".Translate().ToString() : compProperties_DeathrestBindable.stackLimit.ToString(), "StatsReport_DeathrestConnectionLimit_Desc".Translate(), 910);
					_003C_003E1__state = 2;
					return true;
				}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__11(0)
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

	public int stackLimit;

	public bool countsTowardsBuildingLimit = true;

	public bool displayTimeActive = true;

	public float deathrestEffectivenessFactor = 1f;

	public bool mustBeLayingInToBind;

	public float hemogenLimitOffset;

	public HediffDef hediffToApply;

	public SoundDef soundWorking;

	public SoundDef soundStart;

	public SoundDef soundEnd;

	public CompProperties_DeathrestBindable()
	{
		compClass = typeof(CompDeathrestBindable);
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__11))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}
}
