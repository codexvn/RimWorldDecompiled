using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_SmoothWall : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public JobDriver_SmoothWall _003C_003E4__this;

		public Toil doWork;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			if (!_003C_003E4__this.job.ignoreDesignations && _003C_003E4__this.Map.designationManager.DesignationAt(_003C_003E4__this.TargetLocA, _003C_003E4__this.DesDef) == null)
			{
				return true;
			}
			return false;
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			_003C_003E4__this.workLeft = _003C_003E4__this.BaseWorkAmount;
		}

		internal void _003CMakeNewToils_003Eb__2(int delta)
		{
			float num = doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed) * 1.7f * (float)delta;
			_003C_003E4__this.workLeft -= num;
			if (doWork.actor.skills != null)
			{
				doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f * (float)delta);
			}
			if (_003C_003E4__this.workLeft <= 0f)
			{
				_003C_003E4__this.DoEffect();
				_003C_003E4__this.Map.designationManager.DesignationAt(_003C_003E4__this.TargetLocA, _003C_003E4__this.DesDef)?.Delete();
				_003C_003E4__this.ReadyForNextToil();
			}
		}

		internal float _003CMakeNewToils_003Eb__3()
		{
			return 1f - _003C_003E4__this.workLeft / (float)_003C_003E4__this.BaseWorkAmount;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_SmoothWall _003C_003E4__this;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_SmoothWall f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOn(() => (!_003C_003E8__1._003C_003E4__this.job.ignoreDesignations && _003C_003E8__1._003C_003E4__this.Map.designationManager.DesignationAt(_003C_003E8__1._003C_003E4__this.TargetLocA, _003C_003E8__1._003C_003E4__this.DesDef) == null) ? true : false);
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.doWork = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.doWork.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.workLeft = _003C_003E8__1._003C_003E4__this.BaseWorkAmount;
				};
				_003C_003E8__1.doWork.tickIntervalAction = delegate(int delta)
				{
					float num2 = _003C_003E8__1.doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed) * 1.7f * (float)delta;
					_003C_003E8__1._003C_003E4__this.workLeft = _003C_003E8__1._003C_003E4__this.workLeft - num2;
					if (_003C_003E8__1.doWork.actor.skills != null)
					{
						_003C_003E8__1.doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f * (float)delta);
					}
					if (_003C_003E8__1._003C_003E4__this.workLeft <= 0f)
					{
						_003C_003E8__1._003C_003E4__this.DoEffect();
						_003C_003E8__1._003C_003E4__this.Map.designationManager.DesignationAt(_003C_003E8__1._003C_003E4__this.TargetLocA, _003C_003E8__1._003C_003E4__this.DesDef)?.Delete();
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				_003C_003E8__1.doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.doWork.WithProgressBar(TargetIndex.A, () => 1f - _003C_003E8__1._003C_003E4__this.workLeft / (float)_003C_003E8__1._003C_003E4__this.BaseWorkAmount);
				_003C_003E8__1.doWork.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.doWork.activeSkill = () => SkillDefOf.Construction;
				_003C_003E2__current = _003C_003E8__1.doWork;
				_003C_003E1__state = 2;
				return true;
			case 2:
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

	private float workLeft = -1000f;

	protected int BaseWorkAmount => 6500;

	protected DesignationDef DesDef => DesignationDefOf.SmoothWall;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(job.targetA.Cell, job, 1, -1, null, errorOnFailed);
		}
		return false;
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

	protected void DoEffect()
	{
		SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, pawn), pawn);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
	}
}
