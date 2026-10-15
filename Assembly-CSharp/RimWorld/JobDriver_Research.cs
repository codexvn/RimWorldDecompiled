using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Research : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Toil research;

		public JobDriver_Research _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0(int delta)
		{
			Pawn actor = research.actor;
			float statValue = actor.GetStatValue(StatDefOf.ResearchSpeed);
			statValue *= _003C_003E4__this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor);
			Find.ResearchManager.ResearchPerformed(statValue * (float)delta, actor);
			actor.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta);
			actor.GainComfortFromCellIfPossible(delta, chairsOnly: true);
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			return _003C_003E4__this.Project == null;
		}

		internal bool _003CMakeNewToils_003Eb__2()
		{
			return !_003C_003E4__this.Project.CanBeResearchedAt(_003C_003E4__this.ResearchBench, ignoreResearchBenchPowerStatus: false);
		}

		internal float _003CMakeNewToils_003Eb__3()
		{
			return _003C_003E4__this.Project?.ProgressPercent ?? 0f;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Research _003C_003E4__this;

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
			JobDriver_Research f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.research = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.research.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = _003C_003E8__1.research.actor;
					float statValue = actor.GetStatValue(StatDefOf.ResearchSpeed);
					statValue *= _003C_003E8__1._003C_003E4__this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor);
					Find.ResearchManager.ResearchPerformed(statValue * (float)delta, actor);
					actor.skills.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta);
					actor.GainComfortFromCellIfPossible(delta, chairsOnly: true);
				};
				_003C_003E8__1.research.FailOn(() => _003C_003E8__1._003C_003E4__this.Project == null);
				_003C_003E8__1.research.FailOn(() => !_003C_003E8__1._003C_003E4__this.Project.CanBeResearchedAt(_003C_003E8__1._003C_003E4__this.ResearchBench, ignoreResearchBenchPowerStatus: false));
				_003C_003E8__1.research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E8__1.research.WithEffect(EffecterDefOf.Research, TargetIndex.A, null);
				_003C_003E8__1.research.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.Project?.ProgressPercent ?? 0f);
				_003C_003E8__1.research.defaultCompleteMode = ToilCompleteMode.Delay;
				_003C_003E8__1.research.defaultDuration = 4000;
				_003C_003E8__1.research.activeSkill = () => SkillDefOf.Intellectual;
				_003C_003E2__current = _003C_003E8__1.research;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(2);
				_003C_003E1__state = 3;
				return true;
			case 3:
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

	private const int JobEndInterval = 4000;

	private ResearchProjectDef Project => Find.ResearchManager.GetProject();

	private Building_ResearchBench ResearchBench => (Building_ResearchBench)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(ResearchBench, job, 1, -1, null, errorOnFailed))
		{
			if (ResearchBench.def.hasInteractionCell)
			{
				return pawn.ReserveSittableOrSpot(ResearchBench.InteractionCell, job, errorOnFailed);
			}
			return true;
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
}
