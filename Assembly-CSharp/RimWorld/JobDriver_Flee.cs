using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Flee : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Flee _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
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
			JobDriver_Flee CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
				toil3.atomicWithPrevious = true;
				toil3.defaultCompleteMode = ToilCompleteMode.Instant;
				toil3.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals14.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(CS_0024_003C_003E8__locals14.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				};
				_003C_003E2__current = toil3;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				toil2.tickAction = delegate
				{
					if (CS_0024_003C_003E8__locals14.job.exitMapOnArrival && CS_0024_003C_003E8__locals14.pawn.Map.exitMapGrid.IsExitCell(CS_0024_003C_003E8__locals14.pawn.Position))
					{
						CS_0024_003C_003E8__locals14.ExitMap();
					}
				};
				toil2.FailOn(() => CS_0024_003C_003E8__locals14.pawn.Downed && !CS_0024_003C_003E8__locals14.pawn.health.CanCrawl);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals14.job.exitMapOnArrival && (CS_0024_003C_003E8__locals14.pawn.Position.OnEdge(CS_0024_003C_003E8__locals14.pawn.Map) || CS_0024_003C_003E8__locals14.pawn.Map.exitMapGrid.IsExitCell(CS_0024_003C_003E8__locals14.pawn.Position)))
					{
						CS_0024_003C_003E8__locals14.ExitMap();
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
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
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
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

	protected const TargetIndex DestInd = TargetIndex.A;

	protected const TargetIndex DangerInd = TargetIndex.B;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.GetTarget(TargetIndex.A).Cell);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void ExitMap()
	{
		pawn.ExitMap(allowedToJoinOrCreateCaravan: true, CellRect.WholeMap(base.Map).GetClosestEdge(pawn.Position));
	}
}
