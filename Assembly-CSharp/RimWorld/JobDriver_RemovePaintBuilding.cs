using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RemovePaintBuilding : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RemovePaintBuilding _003C_003E4__this;

		private Toil _003CextractFromQueue_003E5__2;

		private Toil _003CgoToBuilding_003E5__3;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CextractFromQueue_003E5__2 = null;
			_003CgoToBuilding_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RemovePaintBuilding CS_0024_003C_003E8__locals13 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CextractFromQueue_003E5__2 = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
				_003CgoToBuilding_003E5__3 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CextractFromQueue_003E5__2);
				_003C_003E2__current = _003CextractFromQueue_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToBuilding_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals13.removalTime = 0f;
				};
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals13.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals13.PaintTarget);
					CS_0024_003C_003E8__locals13.removalTime += CS_0024_003C_003E8__locals13.pawn.GetStatValue(StatDefOf.WorkSpeedGlobal) / 60f * (float)delta;
					if (CS_0024_003C_003E8__locals13.removalTime >= 1f)
					{
						if (CS_0024_003C_003E8__locals13.PaintTarget is Building building)
						{
							building.ChangePaint(null);
						}
						CS_0024_003C_003E8__locals13.PaintTarget.Map.designationManager.TryRemoveDesignationOn(CS_0024_003C_003E8__locals13.PaintTarget, DesignationDefOf.RemovePaintBuilding);
						CS_0024_003C_003E8__locals13.ReadyForNextToil();
						GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Dye), CS_0024_003C_003E8__locals13.pawn.Position, CS_0024_003C_003E8__locals13.pawn.Map, ThingPlaceMode.Near, null, null, null);
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.PlaySustainerOrSound(SoundDefOf.Interact_RemovePaint);
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals13.removalTime / 1f, interpolateBetweenActorAndTarget: true);
				toil.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CextractFromQueue_003E5__2);
				toil.handlingFacing = true;
				_003C_003E2__current = toil;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CextractFromQueue_003E5__2);
				_003C_003E1__state = 6;
				return true;
			case 6:
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private float removalTime;

	private const TargetIndex PaintTargetIndex = TargetIndex.A;

	private const float PaintTimeSecondsBase = 1f;

	private Thing PaintTarget => job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref removalTime, "removalTime", 0f);
	}
}
