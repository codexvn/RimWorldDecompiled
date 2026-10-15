using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_OperateDeepDrill : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass1_0
	{
		public JobDriver_OperateDeepDrill _003C_003E4__this;

		public Toil work;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return !_003C_003E4__this.job.targetA.Thing.TryGetComp<CompDeepDrill>().CanDrillNow();
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Pawn actor = work.actor;
			((Building)actor.CurJob.targetA.Thing).GetComp<CompDeepDrill>().DrillWorkDone(actor, delta);
			actor.skills?.Learn(SkillDefOf.Mining, 0.065f * (float)delta);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__1 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_OperateDeepDrill _003C_003E4__this;

		private _003C_003Ec__DisplayClass1_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__1(int _003C_003E1__state)
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
			JobDriver_OperateDeepDrill f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass1_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				f.FailOnBurningImmobile(TargetIndex.A);
				f.FailOnThingHavingDesignation(TargetIndex.A, DesignationDefOf.Uninstall);
				f.FailOn(() => !_003C_003E8__1._003C_003E4__this.job.targetA.Thing.TryGetComp<CompDeepDrill>().CanDrillNow());
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.work = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.work.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.work.actor;
					((Building)actor.CurJob.targetA.Thing).GetComp<CompDeepDrill>().DrillWorkDone(actor, delta);
					actor.skills?.Learn(SkillDefOf.Mining, 0.065f * (float)delta);
				};
				_003C_003E8__1.work.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.work.WithEffect(EffecterDefOf.Drill, TargetIndex.A, null);
				_003C_003E8__1.work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E8__1.work.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E8__1.work.activeSkill = () => SkillDefOf.Mining;
				_003C_003E2__current = _003C_003E8__1.work;
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
			_003CMakeNewToils_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__1(0)
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

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__1))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__1(-2)
		{
			_003C_003E4__this = this
		};
	}
}
