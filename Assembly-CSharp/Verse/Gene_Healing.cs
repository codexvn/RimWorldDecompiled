using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class Gene_Healing : Gene
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__5 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Gene_Healing _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__5(int _003C_003E1__state)
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
			Gene_Healing CS_0024_003C_003E8__locals3 = _003C_003E4__this;
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
						defaultLabel = "DEV: Heal permanent wound",
						action = delegate
						{
							HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(CS_0024_003C_003E8__locals3.pawn, CS_0024_003C_003E8__locals3.LabelCap);
							CS_0024_003C_003E8__locals3.ResetInterval();
						}
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
			_003CGetGizmos_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__5(0)
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

	private int ticksToHeal;

	private static readonly IntRange HealingIntervalTicksRange = new IntRange(900000, 1800000);

	public override void PostAdd()
	{
		base.PostAdd();
		ResetInterval();
	}

	public override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		ticksToHeal -= delta;
		if (ticksToHeal <= 0)
		{
			HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(pawn, LabelCap);
			ResetInterval();
		}
	}

	private void ResetInterval()
	{
		ticksToHeal = HealingIntervalTicksRange.RandomInRange;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__5))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksToHeal, "ticksToHeal", 0);
	}
}
