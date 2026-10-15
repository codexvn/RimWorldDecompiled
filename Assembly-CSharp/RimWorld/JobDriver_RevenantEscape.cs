using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RevenantEscape : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RevenantEscape _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
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
			JobDriver_RevenantEscape CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals15.Comp.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
				Toil toil2 = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (Rand.MTBEventOccurs(SmearMTBTicks, 1f, delta))
					{
						RevenantUtility.CreateRevenantSmear(CS_0024_003C_003E8__locals15.pawn);
					}
					if (Rand.MTBEventOccurs(EscapedCheckInterval, 1f, delta) && Find.TickManager.TicksGame > CS_0024_003C_003E8__locals15.pawn.mindState.lastEngageTargetTick + MinEscapeTime && RevenantUtility.NearbyHumanlikePawnCount(CS_0024_003C_003E8__locals15.pawn.Position, CS_0024_003C_003E8__locals15.pawn.Map, 20f) == 0)
					{
						CS_0024_003C_003E8__locals15.ReadyForNextToil();
					}
				});
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
				{
					CS_0024_003C_003E8__locals15.Comp.escapeSecondStageStartedTick = Find.TickManager.TicksGame;
				});
				toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (Find.AnalysisManager.TryGetAnalysisProgress(CS_0024_003C_003E8__locals15.Comp.biosignature, out var details) && (details.Satisfied || details.timesDone >= 1) && Find.TickManager.TicksGame < CS_0024_003C_003E8__locals15.Comp.escapeSecondStageStartedTick + SmearLeavingDuration && Rand.MTBEventOccurs(SmearMTBTicks, 1f, delta))
					{
						RevenantUtility.CreateRevenantSmear(CS_0024_003C_003E8__locals15.pawn);
					}
					if (Find.TickManager.TicksGame > CS_0024_003C_003E8__locals15.Comp.escapeSecondStageStartedTick + MinEscapeTime * 2 && RevenantUtility.NearbyHumanlikePawnCount(CS_0024_003C_003E8__locals15.pawn.Position, CS_0024_003C_003E8__locals15.pawn.Map, 20f) == 0)
					{
						CS_0024_003C_003E8__locals15.pawn.TryGetComp<CompRevenant>().revenantState = RevenantState.Sleep;
						CS_0024_003C_003E8__locals15.EndJobWith(JobCondition.Succeeded);
					}
				});
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	private static int MinEscapeTime = 300;

	private static int EscapedCheckInterval = 120;

	private static int SmearMTBTicks = 60;

	private static readonly int SmearLeavingDuration = Mathf.FloorToInt((float)MinEscapeTime * 1.4f);

	private CompRevenant Comp => pawn.TryGetComp<CompRevenant>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.targetA.Cell);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
