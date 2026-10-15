using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_GatherAnimalBodyResources : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public Toil wait;

		public JobDriver_GatherAnimalBodyResources _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Pawn actor = wait.actor;
			Pawn pawn = (Pawn)_003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
			actor.pather.StopDead();
			PawnUtility.ForceWait(pawn, 15000, null, maintainPosture: true);
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Pawn actor = wait.actor;
			actor.skills.Learn(SkillDefOf.Animals, 0.13f * (float)delta);
			_003C_003E4__this.gatherProgress += actor.GetStatValue(StatDefOf.AnimalGatherSpeed) * (float)delta;
			if (_003C_003E4__this.gatherProgress >= _003C_003E4__this.WorkTotal)
			{
				_003C_003E4__this.GetComp((Pawn)(Thing)_003C_003E4__this.job.GetTarget(TargetIndex.A)).Gathered(_003C_003E4__this.pawn);
				actor.jobs.EndCurrentJob(JobCondition.Succeeded);
			}
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			Pawn pawn = (Pawn)_003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
			if (pawn != null && pawn.CurJobDef == JobDefOf.Wait_MaintainPosture)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}

		internal JobCondition _003CMakeNewToils_003Eb__3()
		{
			if (!_003C_003E4__this.GetComp((Pawn)(Thing)_003C_003E4__this.job.GetTarget(TargetIndex.A)).ActiveAndFull)
			{
				return JobCondition.Incompletable;
			}
			return JobCondition.Ongoing;
		}

		internal float _003CMakeNewToils_003Eb__4()
		{
			return _003C_003E4__this.gatherProgress / _003C_003E4__this.WorkTotal;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_GatherAnimalBodyResources _003C_003E4__this;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_GatherAnimalBodyResources f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				f.FailOnDowned(TargetIndex.A);
				f.FailOnNotCasualInterruptible(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.wait = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.wait.initAction = delegate
				{
					Pawn actor = _003C_003E8__1.wait.actor;
					Pawn pawn = (Pawn)_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
					actor.pather.StopDead();
					PawnUtility.ForceWait(pawn, 15000, null, maintainPosture: true);
				};
				_003C_003E8__1.wait.tickIntervalAction = delegate(int delta)
				{
					Pawn actor2 = _003C_003E8__1.wait.actor;
					actor2.skills.Learn(SkillDefOf.Animals, 0.13f * (float)delta);
					_003C_003E8__1._003C_003E4__this.gatherProgress = _003C_003E8__1._003C_003E4__this.gatherProgress + actor2.GetStatValue(StatDefOf.AnimalGatherSpeed) * (float)delta;
					if (_003C_003E8__1._003C_003E4__this.gatherProgress >= _003C_003E8__1._003C_003E4__this.WorkTotal)
					{
						_003C_003E8__1._003C_003E4__this.GetComp((Pawn)(Thing)_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A)).Gathered(_003C_003E8__1._003C_003E4__this.pawn);
						actor2.jobs.EndCurrentJob(JobCondition.Succeeded);
					}
				};
				_003C_003E8__1.wait.AddFinishAction(delegate
				{
					Pawn pawn2 = (Pawn)_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A).Thing;
					if (pawn2 != null && pawn2.CurJobDef == JobDefOf.Wait_MaintainPosture)
					{
						pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}
				});
				_003C_003E8__1.wait.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E8__1.wait.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.wait.AddEndCondition(() => _003C_003E8__1._003C_003E4__this.GetComp((Pawn)(Thing)_003C_003E8__1._003C_003E4__this.job.GetTarget(TargetIndex.A)).ActiveAndFull ? JobCondition.Ongoing : JobCondition.Incompletable);
				_003C_003E8__1.wait.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.wait.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.gatherProgress / _003C_003E8__1._003C_003E4__this.WorkTotal);
				_003C_003E8__1.wait.activeSkill = () => SkillDefOf.Animals;
				_003C_003E2__current = _003C_003E8__1.wait;
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

	private float gatherProgress;

	protected const TargetIndex AnimalInd = TargetIndex.A;

	protected abstract float WorkTotal { get; }

	protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref gatherProgress, "gatherProgress", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
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
}
