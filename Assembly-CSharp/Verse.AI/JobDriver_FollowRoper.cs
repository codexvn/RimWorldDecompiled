using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_FollowRoper : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FollowRoper _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
			JobDriver_FollowRoper CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals6.FailOnDespawnedOrNull(TargetIndex.A);
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate
				{
					Pawn pawn = CS_0024_003C_003E8__locals6.job.GetTarget(TargetIndex.A).Thing as Pawn;
					if (!CS_0024_003C_003E8__locals6.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly))
					{
						CS_0024_003C_003E8__locals6.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						IntVec3 intVec = IntVec3.Invalid;
						if (pawn.jobs.curDriver is JobDriver_RopeToDestination)
						{
							intVec = pawn.CurJob.GetTarget(TargetIndex.B).Cell;
						}
						if (intVec.IsValid && pawn.Position == intVec)
						{
							CS_0024_003C_003E8__locals6.GotoStandCell(intVec);
						}
						else
						{
							CS_0024_003C_003E8__locals6.FollowRoper(pawn);
						}
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
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
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
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

	private const TargetIndex RoperInd = TargetIndex.A;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void FollowRoper(Pawn roper)
	{
		LocalTargetInfo localTargetInfo = new LocalTargetInfo(roper);
		PathEndMode peMode = PathEndMode.Touch;
		if (roper.Position.InHorDistOf(pawn.Position, 7.2f) && roper.pather.Moving && roper.pather.curPath != null)
		{
			int marchingPos = Mathf.Abs(roper.roping.Ropees.IndexOf(pawn));
			IntVec3 intVec = MarchingOrder(roper, marchingPos);
			if (intVec.IsValid)
			{
				localTargetInfo = intVec;
				peMode = PathEndMode.OnCell;
			}
		}
		if (!pawn.pather.Moving || pawn.pather.Destination != localTargetInfo)
		{
			pawn.pather.StartPath(localTargetInfo, peMode);
		}
	}

	private IntVec3 MarchingOrder(Pawn roper, int marchingPos)
	{
		PawnPath curPath = roper.pather.curPath;
		if (curPath.NodesLeftCount <= 0)
		{
			return IntVec3.Invalid;
		}
		Map map = pawn.Map;
		int value = -2 - marchingPos % 4;
		value = Mathf.Clamp(value, -curPath.NodesConsumedCount, curPath.NodesLeftCount);
		IntVec3 intVec = curPath.Peek(value);
		int num = Mathf.Abs(pawn.HashOffset()) % 3;
		if (value + 1 < curPath.NodesLeftCount && num != 0)
		{
			IntVec3 orig = curPath.Peek(value + 1) - intVec;
			IntVec3 intVec2 = ((num == 1) ? (intVec + orig.RotatedBy(Rot4.East)) : (intVec + orig.RotatedBy(Rot4.West)));
			if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2.GetDistrict(map) == intVec.GetDistrict(map))
			{
				intVec = intVec2;
			}
		}
		return intVec;
	}

	private void GotoStandCell(IntVec3 standCell)
	{
		if (!pawn.pather.Moving || pawn.pather.Destination != standCell)
		{
			pawn.pather.StartPath(standCell, PathEndMode.OnCell);
		}
	}

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}
}
