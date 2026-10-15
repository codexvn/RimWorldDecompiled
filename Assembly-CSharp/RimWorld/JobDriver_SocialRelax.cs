using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_SocialRelax : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__12 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_SocialRelax _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__12(int _003C_003E1__state)
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
			JobDriver_SocialRelax CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals14.EndOnDespawnedOrNull(TargetIndex.A);
				if (CS_0024_003C_003E8__locals14.HasChair)
				{
					CS_0024_003C_003E8__locals14.EndOnDespawnedOrNull(TargetIndex.B);
				}
				if (CS_0024_003C_003E8__locals14.HasDrink)
				{
					CS_0024_003C_003E8__locals14.FailOnDestroyedNullOrForbidden(TargetIndex.C);
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.C);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00a4;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.C);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a4;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals14.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals14.ClosestGatherSpotParentCell);
					CS_0024_003C_003E8__locals14.pawn.GainComfortFromCellIfPossible(delta);
					JoyUtility.JoyTickCheckEnd(CS_0024_003C_003E8__locals14.pawn, delta, JoyTickFullJoyAction.GoToNextToil);
				};
				toil.handlingFacing = true;
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = CS_0024_003C_003E8__locals14.job.def.joyDuration;
				toil.AddFinishAction(delegate
				{
					JoyUtility.TryGainRecRoomThought(CS_0024_003C_003E8__locals14.pawn);
				});
				toil.socialMode = RandomSocialMode.SuperActive;
				Toils_Ingest.AddIngestionEffects(toil, CS_0024_003C_003E8__locals14.pawn, TargetIndex.C, TargetIndex.None);
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals14.HasDrink)
				{
					_003C_003E2__current = Toils_Ingest.FinalizeIngest(CS_0024_003C_003E8__locals14.pawn, TargetIndex.C);
					_003C_003E1__state = 5;
					return true;
				}
				break;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00a4:
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 3;
				return true;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__12(0)
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

	private const TargetIndex GatherSpotParentInd = TargetIndex.A;

	private const TargetIndex ChairOrSpotInd = TargetIndex.B;

	private const TargetIndex OptionalIngestibleInd = TargetIndex.C;

	private Thing GatherSpotParent => job.GetTarget(TargetIndex.A).Thing;

	private bool HasChair => job.GetTarget(TargetIndex.B).HasThing;

	private bool HasDrink => job.GetTarget(TargetIndex.C).HasThing;

	private IntVec3 ClosestGatherSpotParentCell => GatherSpotParent.OccupiedRect().ClosestCellTo(pawn.Position);

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!HasChair || !Toils_Ingest.TryFindFreeSittingSpotOnThing(job.GetTarget(TargetIndex.B).Thing, pawn, out var cell))
		{
			cell = job.GetTarget(TargetIndex.B).Cell;
		}
		if (!pawn.ReserveSittableOrSpot(cell, pawn.CurJob))
		{
			return false;
		}
		job.SetTarget(TargetIndex.B, cell);
		if (HasDrink && !pawn.Reserve(job.GetTarget(TargetIndex.C), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__12))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
	{
		IntVec3 closestGatherSpotParentCell = ClosestGatherSpotParentCell;
		return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, closestGatherSpotParentCell, pawn);
	}
}
