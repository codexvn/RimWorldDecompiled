using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RevenantWander : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RevenantWander _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
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
			JobDriver_RevenantWander CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals12.Comp.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
				Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (Find.TickManager.TicksGame > CS_0024_003C_003E8__locals12.Comp.revenantLastLeftSmear + 60000 && Rand.MTBEventOccurs(2f, 60000f, delta))
					{
						RevenantUtility.CreateRevenantSmear(CS_0024_003C_003E8__locals12.pawn);
					}
					if (Find.TickManager.TicksGame >= CS_0024_003C_003E8__locals12.Comp.nextHypnosis && Rand.MTBEventOccurs(2500f, 1f, delta))
					{
						Pawn pawn = RevenantUtility.ScanForTarget(CS_0024_003C_003E8__locals12.pawn);
						if (pawn != null)
						{
							CS_0024_003C_003E8__locals12.pawn.mindState.enemyTarget = pawn;
							CS_0024_003C_003E8__locals12.Comp.revenantState = RevenantState.Attack;
							CS_0024_003C_003E8__locals12.EndJobWith(JobCondition.InterruptForced);
						}
					}
					if (Rand.MTBEventOccurs(900f, 1f, delta))
					{
						Pawn closestTargetInRadius = RevenantUtility.GetClosestTargetInRadius(CS_0024_003C_003E8__locals12.pawn, 10f);
						if (closestTargetInRadius != null)
						{
							CS_0024_003C_003E8__locals12.pawn.mindState.enemyTarget = closestTargetInRadius;
							CS_0024_003C_003E8__locals12.Comp.revenantState = RevenantState.Attack;
							CS_0024_003C_003E8__locals12.EndJobWith(JobCondition.InterruptForced);
						}
					}
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private const int SmearMTBDays = 2;

	private const int MinSmearInterval = 60000;

	private CompRevenant Comp => pawn.TryGetComp<CompRevenant>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
