using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_CreateXenogerm : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__4 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CreateXenogerm _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__4(int _003C_003E1__state)
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
			JobDriver_CreateXenogerm CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals12.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals12.FailOn(() => !CS_0024_003C_003E8__locals12.Xenogerminator.CanBeWorkedOnNow.Accepted);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					float workAmount = CS_0024_003C_003E8__locals12.pawn.GetStatValue(StatDefOf.ResearchSpeed) * CS_0024_003C_003E8__locals12.Xenogerminator.GetStatValue(StatDefOf.AssemblySpeedFactor) * (float)delta;
					CS_0024_003C_003E8__locals12.Xenogerminator.DoWork(workAmount);
					CS_0024_003C_003E8__locals12.pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta);
					CS_0024_003C_003E8__locals12.pawn.GainComfortFromCellIfPossible(delta, chairsOnly: true);
					if (CS_0024_003C_003E8__locals12.Xenogerminator.ProgressPercent >= 1f)
					{
						CS_0024_003C_003E8__locals12.Xenogerminator.Finish();
						CS_0024_003C_003E8__locals12.pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
					}
				};
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				toil.WithEffect(EffecterDefOf.GeneAssembler_Working, TargetIndex.A, null);
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals12.Xenogerminator.ProgressPercent, interpolateBetweenActorAndTarget: false, 0f);
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.defaultDuration = 4000;
				toil.activeSkill = () => SkillDefOf.Intellectual;
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
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
			_003CMakeNewToils_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__4(0)
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

	private const int JobEndInterval = 4000;

	private Building_GeneAssembler Xenogerminator => (Building_GeneAssembler)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Xenogerminator, job, 1, -1, null, errorOnFailed))
		{
			return pawn.ReserveSittableOrSpot(Xenogerminator.InteractionCell, job, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__4))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
