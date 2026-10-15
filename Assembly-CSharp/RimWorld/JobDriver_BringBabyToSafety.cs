using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BringBabyToSafety : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BringBabyToSafety _003C_003E4__this;

		private Toil _003CfindBedForBaby_003E5__2;

		private IEnumerator<Toil> _003C_003E7__wrap2;

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
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CfindBedForBaby_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_BringBabyToSafety CS_0024_003C_003E8__locals28 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals28.AddFailCondition(() => !CS_0024_003C_003E8__locals28.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					CS_0024_003C_003E8__locals28.FailOnDestroyedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals28.AddFailCondition(() => !ChildcareUtility.CanSuckle(CS_0024_003C_003E8__locals28.Baby, out var _));
					if (!CS_0024_003C_003E8__locals28.job.playerForced && CS_0024_003C_003E8__locals28.Baby.Spawned && !CS_0024_003C_003E8__locals28.Baby.ComfortableTemperatureAtCell(CS_0024_003C_003E8__locals28.Baby.Position, CS_0024_003C_003E8__locals28.Baby.Map) && (PawnUtility.ShouldSendNotificationAbout(CS_0024_003C_003E8__locals28.pawn) || PawnUtility.ShouldSendNotificationAbout(CS_0024_003C_003E8__locals28.Baby)))
					{
						_003C_003E2__current = Toils_General.Do(delegate
						{
							Messages.Message("MessageTakingBabyToSafeTemperature".Translate(CS_0024_003C_003E8__locals28.pawn.Named("ADULT"), CS_0024_003C_003E8__locals28.Baby.Named("BABY")), new LookTargets(CS_0024_003C_003E8__locals28.pawn, CS_0024_003C_003E8__locals28.Baby), MessageTypeDefOf.NeutralEvent);
						});
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00f5;
				}
				case 1:
					_003C_003E1__state = -1;
					goto IL_00f5;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = JobDriver_PickupToHold.Toils(CS_0024_003C_003E8__locals28).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_018a;
				case 3:
					_003C_003E1__state = -3;
					goto IL_018a;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Reserve.ReserveDestinationOrThing(TargetIndex.B);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell).FailOnInvalidOrDestroyed(TargetIndex.B).FailOnForbidden(TargetIndex.B)
						.FailOnSomeonePhysicallyInteracting(TargetIndex.B)
						.FailOnDestroyedNullOrForbidden(TargetIndex.A)
						.FailOn(() => !CS_0024_003C_003E8__locals28.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals28.Baby) || CS_0024_003C_003E8__locals28.pawn.Downed || CS_0024_003C_003E8__locals28.pawn.Drafted);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Reserve.ReleaseDestinationOrThing(TargetIndex.B);
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Bed.TuckIntoBed(TargetIndex.B, TargetIndex.A);
					_003C_003E1__state = 8;
					return true;
				case 8:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_018a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E2__current = _003CfindBedForBaby_003E5__2;
					_003C_003E1__state = 4;
					return true;
					IL_00f5:
					_003CfindBedForBaby_003E5__2 = CS_0024_003C_003E8__locals28.FindBedForBaby();
					_003C_003E2__current = Toils_Jump.JumpIf(_003CfindBedForBaby_003E5__2, () => CS_0024_003C_003E8__locals28.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals28.Baby)).FailOn(() => !CS_0024_003C_003E8__locals28.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals28.Baby) && (CS_0024_003C_003E8__locals28.pawn.Downed || CS_0024_003C_003E8__locals28.pawn.Drafted));
					_003C_003E1__state = 2;
					return true;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
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

	private const TargetIndex BabyInd = TargetIndex.A;

	private const TargetIndex BabyBedInd = TargetIndex.B;

	private Pawn Baby => (Pawn)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Baby, job, 1, -1, null, errorOnFailed);
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

	private Toil FindBedForBaby()
	{
		Toil toil = ToilMaker.MakeToil("FindBedForBaby");
		toil.initAction = delegate
		{
			LocalTargetInfo pack = ChildcareUtility.SafePlaceForBaby(Baby, pawn);
			LocalTargetInfo pack2 = LocalTargetInfo.Invalid;
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(Baby))
			{
				pack2 = JobGiver_PrepareCaravan_GatherDownedPawns.FindRandomDropCell(pawn, Baby);
			}
			if (pack.IsValid)
			{
				if (pack2.IsValid && pack2.Cell.DistanceTo(pawn.Position) < pack.Cell.DistanceTo(pawn.Position))
				{
					toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
				}
				else
				{
					toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack);
				}
			}
			else if (pack2.IsValid)
			{
				toil.GetActor().CurJob.SetTarget(TargetIndex.B, pack2);
			}
			else
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		};
		toil.FailOn(() => !pawn.IsCarryingPawn(Baby) || pawn.Downed || pawn.Drafted);
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}
}
