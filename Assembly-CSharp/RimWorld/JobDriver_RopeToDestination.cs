using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_RopeToDestination : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RopeToDestination _003C_003E4__this;

		private Toil _003CfindAnotherAnimal_003E5__2;

		private Toil _003CtopOfLoop_003E5__3;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CfindAnotherAnimal_003E5__2 = null;
			_003CtopOfLoop_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RopeToDestination CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals12.UpdateDestination();
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals12.AddFinishAction(delegate
				{
					CS_0024_003C_003E8__locals12.pawn?.roping?.DropRopes();
				});
				_003CfindAnotherAnimal_003E5__2 = Toils_General.Label();
				_003CtopOfLoop_003E5__3 = Toils_General.Label();
				_003C_003E2__current = _003CtopOfLoop_003E5__3;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CfindAnotherAnimal_003E5__2, () => (CS_0024_003C_003E8__locals12.job.GetTarget(TargetIndex.A).Thing as Pawn)?.roping.RopedByPawn == CS_0024_003C_003E8__locals12.pawn);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_Rope.GotoRopeAttachmentInteractionCell(TargetIndex.A);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Rope.RopePawn(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CfindAnotherAnimal_003E5__2;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CtopOfLoop_003E5__3, CS_0024_003C_003E8__locals12.FindAnotherAnimalToRope);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003CfindAnotherAnimal_003E5__2 = null;
				_003CtopOfLoop_003E5__3 = null;
				_003CtopOfLoop_003E5__3 = Toils_General.Label();
				_003C_003E2__current = _003CtopOfLoop_003E5__3;
				_003C_003E1__state = 9;
				return true;
			case 9:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
				CS_0024_003C_003E8__locals12.MatchLocomotionUrgency(toil);
				toil.AddPreTickIntervalAction(delegate
				{
					CS_0024_003C_003E8__locals12.ProcessRopeesThatHaveArrived(roperWaitingAtDest: false);
				});
				toil.FailOn(() => !CS_0024_003C_003E8__locals12.pawn.roping.IsRopingOthers);
				_003C_003E2__current = toil;
				_003C_003E1__state = 10;
				return true;
			}
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CtopOfLoop_003E5__3, CS_0024_003C_003E8__locals12.UpdateDestination);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003CtopOfLoop_003E5__3 = null;
				_003CtopOfLoop_003E5__3 = Toils_General.Wait(60, TargetIndex.A);
				_003CtopOfLoop_003E5__3.AddPreTickIntervalAction(delegate
				{
					CS_0024_003C_003E8__locals12.ProcessRopeesThatHaveArrived(roperWaitingAtDest: true);
				});
				_003C_003E2__current = _003CtopOfLoop_003E5__3;
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIf(_003CtopOfLoop_003E5__3, () => CS_0024_003C_003E8__locals12.pawn.roping.IsRopingOthers);
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003CtopOfLoop_003E5__3 = null;
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
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private const TargetIndex AnimalInd = TargetIndex.A;

	public const TargetIndex DestCellInd = TargetIndex.B;

	private const int MaxAnimalsToRope = 10;

	private const int RopeMoreAnimalsScanRadius = 10;

	private static readonly List<Pawn> tmpRopees = new List<Pawn>();

	protected abstract bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest);

	protected abstract void ProcessArrivedRopee(Pawn ropee);

	protected abstract bool ShouldOpportunisticallyRopeAnimal(Pawn animal);

	protected virtual Thing FindDistantAnimalToRope()
	{
		return null;
	}

	protected virtual bool UpdateDestination()
	{
		return false;
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.roping.Ropees.NullOrEmpty())
		{
			foreach (Pawn ropee in pawn.roping.Ropees)
			{
				pawn.Reserve(ropee, job, 1, -1, null, errorOnFailed);
			}
		}
		UpdateDestination();
		pawn.Map.pawnDestinationReservationManager.Reserve(pawn, job, job.GetTarget(TargetIndex.B).Cell);
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void ProcessRopeesThatHaveArrived(bool roperWaitingAtDest)
	{
		tmpRopees.Clear();
		tmpRopees.AddRange(pawn.roping.Ropees);
		foreach (Pawn tmpRopee in tmpRopees)
		{
			if (HasRopeeArrived(tmpRopee, roperWaitingAtDest))
			{
				pawn.roping.DropRope(tmpRopee);
				if (tmpRopee.jobs != null && tmpRopee.CurJob != null && tmpRopee.jobs.curDriver is JobDriver_FollowRoper)
				{
					tmpRopee.jobs.EndCurrentJob(JobCondition.InterruptForced);
				}
				ProcessArrivedRopee(tmpRopee);
			}
		}
	}

	private void MatchLocomotionUrgency(Toil toil)
	{
		toil.AddPreInitAction(delegate
		{
			locomotionUrgencySameAs = SlowestRopee();
		});
		toil.AddFinishAction(delegate
		{
			locomotionUrgencySameAs = null;
		});
	}

	private Pawn SlowestRopee()
	{
		if (!pawn.roping.Ropees.TryMaxBy((Pawn p) => p.TicksPerMoveCardinal, out var value))
		{
			return null;
		}
		return value;
	}

	private bool FindAnotherAnimalToRope()
	{
		int num = pawn.mindState?.duty?.ropeeLimit ?? 10;
		if (pawn.roping.Ropees.Count >= num)
		{
			return false;
		}
		Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.Touch, TraverseParms.For(pawn), 10f, AnimalValidator);
		if (thing2 == null)
		{
			thing2 = FindDistantAnimalToRope();
		}
		if (thing2 != null)
		{
			job.SetTarget(TargetIndex.A, thing2);
			return true;
		}
		return false;
		bool AnimalValidator(Thing thing)
		{
			if (!(thing is Pawn animal))
			{
				return false;
			}
			return ShouldOpportunisticallyRopeAnimal(animal);
		}
	}
}
