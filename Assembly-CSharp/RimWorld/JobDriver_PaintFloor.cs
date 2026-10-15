using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_PaintFloor : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CCollectDyeToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Toil _003Cextract_003E5__2;

		private Toil _003CjumpIfHaveTargetInQueue_003E5__3;

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
		public _003CCollectDyeToils_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cextract_003E5__2 = null;
			_003CjumpIfHaveTargetInQueue_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Cextract_003E5__2 = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B, failIfCountFromQueueTooBig: false);
				_003C_003E2__current = _003Cextract_003E5__2;
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003CjumpIfHaveTargetInQueue_003E5__3 = Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, _003Cextract_003E5__2);
				Toil toil = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: true);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CjumpIfHaveTargetInQueue_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
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
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CCollectDyeToils_003Ed__8(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PaintFloor _003C_003E4__this;

		private Toil _003CextractFromQueue_003E5__2;

		private Toil _003CcheckFinished_003E5__3;

		private IEnumerator<Toil> _003C_003E7__wrap3;

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
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CextractFromQueue_003E5__2 = null;
			_003CcheckFinished_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_PaintFloor CS_0024_003C_003E8__locals21 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CextractFromQueue_003E5__2 = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A);
					_003CcheckFinished_003E5__3 = Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
					_003C_003E2__current = Toils_Jump.JumpIf(_003CextractFromQueue_003E5__2, () => CS_0024_003C_003E8__locals21.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap3 = CS_0024_003C_003E8__locals21.CollectDyeToils().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00d3;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00d3;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CextractFromQueue_003E5__2;
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).JumpIf(() => CS_0024_003C_003E8__locals21.Map.designationManager.DesignationAt(CS_0024_003C_003E8__locals21.TargetLocA, DesignationDefOf.PaintFloor) == null, _003CcheckFinished_003E5__3);
					_003C_003E1__state = 5;
					return true;
				case 5:
				{
					_003C_003E1__state = -1;
					Toil toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						CS_0024_003C_003E8__locals21.paintingTime = 0f;
					};
					toil.tickIntervalAction = delegate(int delta)
					{
						CS_0024_003C_003E8__locals21.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals21.PaintTarget);
						CS_0024_003C_003E8__locals21.paintingTime += CS_0024_003C_003E8__locals21.pawn.GetStatValue(StatDefOf.WorkSpeedGlobal) / 60f * (float)delta;
						CS_0024_003C_003E8__locals21.pawn.skills?.Learn(SkillDefOf.Artistic, 0.1f * (float)delta);
						if (CS_0024_003C_003E8__locals21.paintingTime >= 1f)
						{
							CS_0024_003C_003E8__locals21.pawn.carryTracker.CarriedThing?.SplitOff(1)?.Destroy();
							Designation designation = CS_0024_003C_003E8__locals21.Map.designationManager.DesignationAt(CS_0024_003C_003E8__locals21.PaintTarget, DesignationDefOf.PaintFloor);
							if (designation != null)
							{
								CS_0024_003C_003E8__locals21.Map.terrainGrid.SetTerrainColor(CS_0024_003C_003E8__locals21.PaintTarget, designation.colorDef);
								CS_0024_003C_003E8__locals21.Map.designationManager.RemoveDesignation(designation);
							}
							CS_0024_003C_003E8__locals21.ReadyForNextToil();
						}
					};
					toil.defaultCompleteMode = ToilCompleteMode.Never;
					toil.WithEffect(EffecterDefOf.Paint, TargetIndex.A, null);
					toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals21.paintingTime / 1f, interpolateBetweenActorAndTarget: true);
					toil.JumpIf(() => CS_0024_003C_003E8__locals21.Map.designationManager.DesignationAt(CS_0024_003C_003E8__locals21.TargetLocA, DesignationDefOf.PaintFloor) == null, _003CcheckFinished_003E5__3);
					toil.activeSkill = () => SkillDefOf.Artistic;
					toil.handlingFacing = true;
					_003C_003E2__current = toil;
					_003C_003E1__state = 6;
					return true;
				}
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.Jump(_003CcheckFinished_003E5__3);
					_003C_003E1__state = 7;
					return true;
				case 7:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00d3:
					if (_003C_003E7__wrap3.MoveNext())
					{
						Toil current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					_003C_003E2__current = _003CcheckFinished_003E5__3;
					_003C_003E1__state = 3;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
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

	private float paintingTime;

	private const TargetIndex PaintTargetIndex = TargetIndex.A;

	private const TargetIndex DyeIndex = TargetIndex.B;

	private const float PaintTimeSecondsBase = 1f;

	private IntVec3 PaintTarget => job.GetTarget(TargetIndex.A).Cell;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job, 1, -1, ReservationLayerDefOf.Floor);
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
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

	[IteratorStateMachine(typeof(_003CCollectDyeToils_003Ed__8))]
	private IEnumerable<Toil> CollectDyeToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCollectDyeToils_003Ed__8(-2);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref paintingTime, "paintingTime", 0f);
	}
}
