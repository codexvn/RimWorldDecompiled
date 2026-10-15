using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_RemovePaintFloor : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RemovePaintFloor _003C_003E4__this;

		private Toil _003CcheckFinished_003E5__2;

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
			_003CcheckFinished_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_RemovePaintFloor CS_0024_003C_003E8__locals18 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CcheckFinished_003E5__2 = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
				_003C_003E2__current = _003CcheckFinished_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).JumpIf(() => CS_0024_003C_003E8__locals18.Map.designationManager.DesignationAt(CS_0024_003C_003E8__locals18.TargetLocA, DesignationDefOf.RemovePaintFloor) == null, _003CcheckFinished_003E5__2);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals18.removalTime = 0f;
				};
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals18.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals18.PaintTarget);
					CS_0024_003C_003E8__locals18.removalTime += CS_0024_003C_003E8__locals18.pawn.GetStatValue(StatDefOf.WorkSpeedGlobal) / 60f * (float)delta;
					if (CS_0024_003C_003E8__locals18.removalTime >= 1f)
					{
						CS_0024_003C_003E8__locals18.Map.terrainGrid.SetTerrainColor(CS_0024_003C_003E8__locals18.PaintTarget, null);
						CS_0024_003C_003E8__locals18.Map.designationManager.TryRemoveDesignation(CS_0024_003C_003E8__locals18.PaintTarget, DesignationDefOf.RemovePaintFloor);
						CS_0024_003C_003E8__locals18.ReadyForNextToil();
						GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Dye), CS_0024_003C_003E8__locals18.pawn.Position, CS_0024_003C_003E8__locals18.pawn.Map, ThingPlaceMode.Near, null, null, null);
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.PlaySustainerOrSound(SoundDefOf.Interact_RemovePaint);
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals18.removalTime / 1f, interpolateBetweenActorAndTarget: true);
				toil.JumpIf(() => CS_0024_003C_003E8__locals18.Map.designationManager.DesignationAt(CS_0024_003C_003E8__locals18.TargetLocA, DesignationDefOf.RemovePaintFloor) == null, _003CcheckFinished_003E5__2);
				toil.handlingFacing = true;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CcheckFinished_003E5__2);
				_003C_003E1__state = 5;
				return true;
			case 5:
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

	private const float RemoveTimeSecondsBase = 1f;

	private IntVec3 PaintTarget => job.GetTarget(TargetIndex.A).Cell;

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
}
