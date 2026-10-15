using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class JobDriver_EnterPortal : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EnterPortal _003C_003E4__this;

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
			JobDriver_EnterPortal CS_0024_003C_003E8__locals30 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals30.FailOnDespawnedOrNull(CS_0024_003C_003E8__locals30.PortalInd);
				CS_0024_003C_003E8__locals30.FailOn(() => !((MapPortal)CS_0024_003C_003E8__locals30.TargetThingA).IsEnterable(out var _));
				_003C_003E2__current = Toils_Goto.GotoThing(CS_0024_003C_003E8__locals30.PortalInd, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_General.Wait(90).FailOnCannotTouch(CS_0024_003C_003E8__locals30.PortalInd, PathEndMode.Touch).WithProgressBarToilDelay(CS_0024_003C_003E8__locals30.PortalInd, interpolateBetweenActorAndTarget: true);
				toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, (Action<int>)delegate
				{
					CS_0024_003C_003E8__locals30.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals30.TargetA);
				});
				toil2.handlingFacing = true;
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
					MapPortal mapPortal = CS_0024_003C_003E8__locals30.TargetThingA as MapPortal;
					Map otherMap = mapPortal.GetOtherMap();
					IntVec3 intVec = mapPortal.GetDestinationLocation();
					if (!intVec.Standable(otherMap))
					{
						intVec = CellFinder.StandableCellNear(intVec, otherMap, 5f);
					}
					if (intVec == IntVec3.Invalid)
					{
						Messages.Message("UnableToEnterPortal".Translate(CS_0024_003C_003E8__locals30.TargetThingA.Label), CS_0024_003C_003E8__locals30.TargetThingA, MessageTypeDefOf.NegativeEvent);
					}
					else
					{
						bool flag = false;
						bool fireAtWill = false;
						if (CS_0024_003C_003E8__locals30.pawn.IsPlayerControlled)
						{
							flag = CS_0024_003C_003E8__locals30.pawn.Drafted;
							fireAtWill = CS_0024_003C_003E8__locals30.pawn.drafter.FireAtWill;
						}
						CS_0024_003C_003E8__locals30.pawn.DeSpawnOrDeselect();
						GenSpawn.Spawn(CS_0024_003C_003E8__locals30.pawn, intVec, otherMap, Rot4.Random);
						mapPortal.OnEntered(CS_0024_003C_003E8__locals30.pawn);
						if (CS_0024_003C_003E8__locals30.pawn.inventory != null)
						{
							CS_0024_003C_003E8__locals30.pawn.inventory.UnloadEverything = !otherMap.IsPocketMap;
						}
						if (CS_0024_003C_003E8__locals30.pawn.IsPlayerControlled && (flag || mapPortal.AutoDraftOnEnter))
						{
							CS_0024_003C_003E8__locals30.pawn.drafter.Drafted = true;
							CS_0024_003C_003E8__locals30.pawn.drafter.FireAtWill = fireAtWill;
						}
						if (CS_0024_003C_003E8__locals30.pawn.carryTracker?.CarriedThing != null && !CS_0024_003C_003E8__locals30.pawn.Drafted)
						{
							CS_0024_003C_003E8__locals30.pawn.carryTracker.TryDropCarriedThing(CS_0024_003C_003E8__locals30.pawn.Position, ThingPlaceMode.Direct, out var _);
						}
						CS_0024_003C_003E8__locals30.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
						CS_0024_003C_003E8__locals30.pawn.GetLord()?.Notify_PawnLost(CS_0024_003C_003E8__locals30.pawn, PawnLostCondition.ExitedMap, null);
					}
				};
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

	private TargetIndex PortalInd = TargetIndex.A;

	private const int EnterDelay = 90;

	public MapPortal MapPortal => base.TargetThingA as MapPortal;

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
