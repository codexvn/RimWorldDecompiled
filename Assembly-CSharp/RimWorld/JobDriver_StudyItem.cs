using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_StudyItem : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Toil study;

		public JobDriver_StudyItem _003C_003E4__this;

		public CompStudiable comp;

		internal void _003CGetStudyToils_003Eb__0(int delta)
		{
			Pawn actor = study.actor;
			float num = 0.08f;
			if (!actor.WorkTypeIsDisabled(WorkTypeDefOf.Research))
			{
				num = actor.GetStatValue(StatDefOf.ResearchSpeed);
			}
			num *= _003C_003E4__this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor) * (float)delta;
			comp.Study(actor, num);
			if (comp.Completed)
			{
				_003C_003E4__this.pawn.jobs.curDriver.ReadyForNextToil();
			}
		}

		internal float _003CGetStudyToils_003Eb__1()
		{
			return comp.ProgressPercent;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetStudyToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_StudyItem _003C_003E4__this;

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
		public _003CGetStudyToils_003Ed__10(int _003C_003E1__state)
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
			JobDriver_StudyItem jobDriver_StudyItem = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals16 = new _003C_003Ec__DisplayClass10_0();
				CS_0024_003C_003E8__locals16._003C_003E4__this = _003C_003E4__this;
				CS_0024_003C_003E8__locals16.study = ToilMaker.MakeToil("GetStudyToils");
				CS_0024_003C_003E8__locals16.comp = jobDriver_StudyItem.ThingToStudy.TryGetComp<CompStudiable>();
				CS_0024_003C_003E8__locals16.study.tickIntervalAction = delegate(int delta)
				{
					Pawn actor = CS_0024_003C_003E8__locals16.study.actor;
					float num2 = 0.08f;
					if (!actor.WorkTypeIsDisabled(WorkTypeDefOf.Research))
					{
						num2 = actor.GetStatValue(StatDefOf.ResearchSpeed);
					}
					num2 *= CS_0024_003C_003E8__locals16._003C_003E4__this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor) * (float)delta;
					CS_0024_003C_003E8__locals16.comp.Study(actor, num2);
					if (CS_0024_003C_003E8__locals16.comp.Completed)
					{
						CS_0024_003C_003E8__locals16._003C_003E4__this.pawn.jobs.curDriver.ReadyForNextToil();
					}
				};
				CS_0024_003C_003E8__locals16.study.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				CS_0024_003C_003E8__locals16.study.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals16.comp.ProgressPercent);
				CS_0024_003C_003E8__locals16.study.defaultCompleteMode = ToilCompleteMode.Delay;
				CS_0024_003C_003E8__locals16.study.defaultDuration = 4000;
				CS_0024_003C_003E8__locals16.study.activeSkill = () => SkillDefOf.Intellectual;
				_003C_003E2__current = CS_0024_003C_003E8__locals16.study;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
			_003CGetStudyToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetStudyToils_003Ed__10(0)
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

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_StudyItem _003C_003E4__this;

		private bool _003CrequiresResearchBench_003E5__2;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 6)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_StudyItem CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CrequiresResearchBench_003E5__2 = !CS_0024_003C_003E8__locals5.CanStudyInPlace;
					if (_003CrequiresResearchBench_003E5__2)
					{
						CS_0024_003C_003E8__locals5.FailOnDespawnedNullOrForbidden(TargetIndex.B);
						CS_0024_003C_003E8__locals5.FailOnBurningImmobile(TargetIndex.B);
						_003C_003E2__current = Toils_General.DoAtomic(delegate
						{
							CS_0024_003C_003E8__locals5.job.count = 1;
						});
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0091;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0091;
				case 2:
					_003C_003E1__state = -1;
					if (_003CrequiresResearchBench_003E5__2)
					{
						_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.A);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0138;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, storageMode: false);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0138;
				case 6:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_0091:
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					_003C_003E1__state = 2;
					return true;
					IL_0138:
					_003C_003E7__wrap2 = CS_0024_003C_003E8__locals5.GetStudyToils().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Toil current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 6;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				return false;
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	protected const TargetIndex ThingToStudyIndex = TargetIndex.A;

	protected const TargetIndex ResearchBenchInd = TargetIndex.B;

	protected const TargetIndex HaulCell = TargetIndex.C;

	protected Thing ThingToStudy => base.TargetThingA;

	protected bool CanStudyInPlace => ThingToStudy.TryGetComp<CompAnalyzable>()?.Props.canStudyInPlace ?? false;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(ThingToStudy, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (!CanStudyInPlace)
		{
			if (!pawn.Reserve(base.TargetB, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (!pawn.Reserve(base.TargetC, job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetStudyToils_003Ed__10))]
	protected virtual IEnumerable<Toil> GetStudyToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetStudyToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}
}
