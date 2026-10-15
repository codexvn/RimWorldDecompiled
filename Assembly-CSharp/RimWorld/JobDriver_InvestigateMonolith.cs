using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_InvestigateMonolith : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_InvestigateMonolith _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__4(int _003C_003E1__state)
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
			JobDriver_InvestigateMonolith CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals6.job.targetB == null)
				{
					CS_0024_003C_003E8__locals6.job.targetB = CS_0024_003C_003E8__locals6.TargetThingA.SpawnedParentOrMe;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				int ticks = Mathf.RoundToInt(300f);
				Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: true, maintainPosture: true, maintainSleep: false, TargetIndex.A);
				toil.WithEffect(EffecterDefOf.MonolithStage1, () => CS_0024_003C_003E8__locals6.TargetA, null);
				toil.PlaySustainerOrSound(SoundDefOf.VoidMonolith_InspectLoop);
				toil.tickAction = (Action)Delegate.Combine(toil.tickAction, (Action)delegate
				{
					Find.TickManager.slower.SignalForceNormalSpeed();
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals6.VoidMonolith.Investigate(CS_0024_003C_003E8__locals6.pawn);
				});
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			_003CMakeNewToils_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__4(0)
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

	private const int InvestigateSeconds = 5;

	private Building_VoidMonolith VoidMonolith => base.TargetThingA as Building_VoidMonolith;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(VoidMonolith, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (!pawn.ReserveSittableOrSpot(VoidMonolith.InteractionCell, job, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__4))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
