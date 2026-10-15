using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_CleanFilth : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public JobDriver_CleanFilth _003C_003E4__this;

		public Toil clean;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003E4__this.cleaningWorkDone = 0f;
			_003C_003E4__this.totalCleaningWorkDone = 0f;
			_003C_003E4__this.totalCleaningWorkRequired = _003C_003E4__this.Filth.def.filth.cleaningWorkToReduceThickness * (float)_003C_003E4__this.Filth.thickness;
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			Filth filth = _003C_003E4__this.Filth;
			float statValueAbstract = filth.Position.GetTerrain(filth.Map).GetStatValueAbstract(StatDefOf.CleaningTimeFactor);
			float num = _003C_003E4__this.pawn.GetStatValue(StatDefOf.CleaningSpeed) * (float)delta;
			if (statValueAbstract != 0f)
			{
				num /= statValueAbstract;
			}
			_003C_003E4__this.cleaningWorkDone += num;
			_003C_003E4__this.totalCleaningWorkDone += num;
			if (_003C_003E4__this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
			{
				filth.ThinFilth();
				_003C_003E4__this.cleaningWorkDone = 0f;
				if (filth.Destroyed)
				{
					clean.actor.records.Increment(RecordDefOf.MessesCleaned);
					_003C_003E4__this.ReadyForNextToil();
				}
			}
		}

		internal float _003CMakeNewToils_003Eb__2()
		{
			return _003C_003E4__this.totalCleaningWorkDone / _003C_003E4__this.totalCleaningWorkRequired;
		}

		internal SoundDef _003CMakeNewToils_003Eb__3()
		{
			ThingDef def = _003C_003E4__this.Filth.def;
			if (!def.filth.cleaningSound.NullOrUndefined())
			{
				return def.filth.cleaningSound;
			}
			return SoundDefOf.Interact_CleanFilth;
		}

		internal bool _003CMakeNewToils_003Eb__4()
		{
			return clean.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing?.Destroyed ?? false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CleanFilth _003C_003E4__this;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

		private Toil _003CinitExtractTargetFromQueue_003E5__2;

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
			_003CinitExtractTargetFromQueue_003E5__2 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003CinitExtractTargetFromQueue_003E5__2 = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
				_003C_003E2__current = _003CinitExtractTargetFromQueue_003E5__2;
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
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CinitExtractTargetFromQueue_003E5__2).JumpIfOutsideHomeArea(TargetIndex.A, _003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E8__1.clean = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.clean.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.cleaningWorkDone = 0f;
					_003C_003E8__1._003C_003E4__this.totalCleaningWorkDone = 0f;
					_003C_003E8__1._003C_003E4__this.totalCleaningWorkRequired = _003C_003E8__1._003C_003E4__this.Filth.def.filth.cleaningWorkToReduceThickness * (float)_003C_003E8__1._003C_003E4__this.Filth.thickness;
				};
				_003C_003E8__1.clean.tickIntervalAction = delegate(int delta)
				{
					Filth filth = _003C_003E8__1._003C_003E4__this.Filth;
					float statValueAbstract = filth.Position.GetTerrain(filth.Map).GetStatValueAbstract(StatDefOf.CleaningTimeFactor);
					float num = _003C_003E8__1._003C_003E4__this.pawn.GetStatValue(StatDefOf.CleaningSpeed) * (float)delta;
					if (statValueAbstract != 0f)
					{
						num /= statValueAbstract;
					}
					_003C_003E8__1._003C_003E4__this.cleaningWorkDone = _003C_003E8__1._003C_003E4__this.cleaningWorkDone + num;
					_003C_003E8__1._003C_003E4__this.totalCleaningWorkDone = _003C_003E8__1._003C_003E4__this.totalCleaningWorkDone + num;
					if (_003C_003E8__1._003C_003E4__this.cleaningWorkDone > filth.def.filth.cleaningWorkToReduceThickness)
					{
						filth.ThinFilth();
						_003C_003E8__1._003C_003E4__this.cleaningWorkDone = 0f;
						if (filth.Destroyed)
						{
							_003C_003E8__1.clean.actor.records.Increment(RecordDefOf.MessesCleaned);
							_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
						}
					}
				};
				_003C_003E8__1.clean.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.clean.WithEffect(EffecterDefOf.Clean, TargetIndex.A, null);
				_003C_003E8__1.clean.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.totalCleaningWorkDone / _003C_003E8__1._003C_003E4__this.totalCleaningWorkRequired, interpolateBetweenActorAndTarget: true);
				_003C_003E8__1.clean.PlaySustainerOrSound(delegate
				{
					ThingDef def = _003C_003E8__1._003C_003E4__this.Filth.def;
					return (!def.filth.cleaningSound.NullOrUndefined()) ? def.filth.cleaningSound : SoundDefOf.Interact_CleanFilth;
				});
				_003C_003E8__1.clean.JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, _003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E8__1.clean.JumpIfOutsideHomeArea(TargetIndex.A, _003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E8__1.clean.JumpIf(() => _003C_003E8__1.clean.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing?.Destroyed ?? false, _003CinitExtractTargetFromQueue_003E5__2);
				_003C_003E2__current = _003C_003E8__1.clean;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CinitExtractTargetFromQueue_003E5__2);
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

	private float cleaningWorkDone;

	private float totalCleaningWorkDone;

	private float totalCleaningWorkRequired;

	private const TargetIndex FilthInd = TargetIndex.A;

	private Filth Filth => (Filth)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
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

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref cleaningWorkDone, "cleaningWorkDone", 0f);
		Scribe_Values.Look(ref totalCleaningWorkDone, "totalCleaningWorkDone", 0f);
		Scribe_Values.Look(ref totalCleaningWorkRequired, "totalCleaningWorkRequired", 0f);
	}
}
