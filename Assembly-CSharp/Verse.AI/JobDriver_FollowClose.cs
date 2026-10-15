using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_FollowClose : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FollowClose _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_FollowClose CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals24.FailOnDespawnedOrNull(TargetIndex.A);
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					Pawn followee = CS_0024_003C_003E8__locals24.Followee;
					float followRadius = CS_0024_003C_003E8__locals24.job.followRadius;
					if (!CS_0024_003C_003E8__locals24.pawn.pather.Moving || CS_0024_003C_003E8__locals24.pawn.IsHashIntervalTick(30, delta))
					{
						bool flag = false;
						if (CS_0024_003C_003E8__locals24.CurrentlyWalkingToFollowee)
						{
							if (NearFollowee(CS_0024_003C_003E8__locals24.pawn, followee, followRadius))
							{
								flag = true;
							}
						}
						else
						{
							float radius = followRadius * 1.2f;
							if (NearFollowee(CS_0024_003C_003E8__locals24.pawn, followee, radius))
							{
								flag = true;
							}
							else
							{
								if (!CS_0024_003C_003E8__locals24.pawn.CanReach(followee, PathEndMode.Touch, Danger.Deadly))
								{
									CS_0024_003C_003E8__locals24.EndJobWith(JobCondition.Incompletable);
									return;
								}
								CS_0024_003C_003E8__locals24.pawn.pather.StartPath(followee, PathEndMode.Touch);
								CS_0024_003C_003E8__locals24.locomotionUrgencySameAs = null;
							}
						}
						if (flag)
						{
							if (NearDestinationOrNotMoving(CS_0024_003C_003E8__locals24.pawn, followee, followRadius))
							{
								CS_0024_003C_003E8__locals24.EndJobWith(JobCondition.Succeeded);
							}
							else
							{
								IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
								if (!CS_0024_003C_003E8__locals24.pawn.pather.Moving || CS_0024_003C_003E8__locals24.pawn.pather.Destination.HasThing || !CS_0024_003C_003E8__locals24.pawn.pather.Destination.Cell.InHorDistOf(lastPassableCellInPath, followRadius))
								{
									IntVec3 intVec = CellFinder.RandomClosewalkCellNear(lastPassableCellInPath, CS_0024_003C_003E8__locals24.Map, Mathf.FloorToInt(followRadius));
									if (intVec == CS_0024_003C_003E8__locals24.pawn.Position)
									{
										CS_0024_003C_003E8__locals24.EndJobWith(JobCondition.Succeeded);
									}
									else if (intVec.IsValid && CS_0024_003C_003E8__locals24.pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Deadly))
									{
										CS_0024_003C_003E8__locals24.pawn.pather.StartPath(intVec, PathEndMode.OnCell);
										CS_0024_003C_003E8__locals24.locomotionUrgencySameAs = followee;
									}
									else
									{
										CS_0024_003C_003E8__locals24.EndJobWith(JobCondition.Incompletable);
									}
								}
							}
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const TargetIndex FolloweeInd = TargetIndex.A;

	private const int CheckPathIntervalTicks = 30;

	private Pawn Followee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	private bool CurrentlyWalkingToFollowee
	{
		get
		{
			if (pawn.pather.Moving)
			{
				return pawn.pather.Destination == Followee;
			}
			return false;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (job.followRadius <= 0f)
		{
			Log.Error("Follow radius is <= 0. pawn=" + pawn.ToStringSafe());
			job.followRadius = 10f;
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool IsContinuation(Job j)
	{
		return job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
	}

	public static bool FarEnoughAndPossibleToStartJob(Pawn follower, Pawn followee, float radius)
	{
		if (radius <= 0f)
		{
			string text = "Checking follow job with radius <= 0. pawn=" + follower.ToStringSafe();
			if (follower.mindState != null && follower.mindState.duty != null)
			{
				text = text + " duty=" + follower.mindState.duty.def;
			}
			Log.ErrorOnce(text, follower.thingIDNumber ^ 0x324308F9);
			return false;
		}
		if (!follower.CanReach(followee, PathEndMode.OnCell, Danger.Deadly))
		{
			return false;
		}
		float radius2 = radius * 1.2f;
		if (NearFollowee(follower, followee, radius2))
		{
			if (!NearDestinationOrNotMoving(follower, followee, radius2))
			{
				return follower.CanReach(followee.pather.LastPassableCellInPath, PathEndMode.OnCell, Danger.Deadly);
			}
			return false;
		}
		return true;
	}

	private static bool NearFollowee(Pawn follower, Pawn followee, float radius)
	{
		if (follower.Position.AdjacentTo8WayOrInside(followee.Position))
		{
			return true;
		}
		if (follower.Position.InHorDistOf(followee.Position, radius))
		{
			return GenSight.LineOfSight(follower.Position, followee.Position, follower.Map);
		}
		return false;
	}

	private static bool NearDestinationOrNotMoving(Pawn follower, Pawn followee, float radius)
	{
		if (!followee.pather.Moving)
		{
			return true;
		}
		IntVec3 lastPassableCellInPath = followee.pather.LastPassableCellInPath;
		if (!lastPassableCellInPath.IsValid)
		{
			return true;
		}
		if (follower.Position.AdjacentTo8WayOrInside(lastPassableCellInPath))
		{
			return true;
		}
		return follower.Position.InHorDistOf(lastPassableCellInPath, radius);
	}
}
