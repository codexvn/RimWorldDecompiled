using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_AffectRoof : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass12_0
	{
		public JobDriver_AffectRoof _003C_003E4__this;

		public Toil doWork;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003E4__this.workLeft = 65f;
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			float num = doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
			_003C_003E4__this.workLeft -= num;
			if (_003C_003E4__this.workLeft <= 0f)
			{
				_003C_003E4__this.DoEffect();
				_003C_003E4__this.ReadyForNextToil();
			}
		}

		internal float _003CMakeNewToils_003Eb__2()
		{
			return 1f - _003C_003E4__this.workLeft / 65f;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__12 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_AffectRoof _003C_003E4__this;

		private _003C_003Ec__DisplayClass12_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__12(int _003C_003E1__state)
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
			JobDriver_AffectRoof jobDriver_AffectRoof = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass12_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_AffectRoof.FailOnDespawnedOrNull(TargetIndex.B);
				_003C_003E2__current = Toils_Goto.Goto(TargetIndex.B, jobDriver_AffectRoof.PathEndMode);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.doWork = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.doWork.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.workLeft = 65f;
				};
				_003C_003E8__1.doWork.tickIntervalAction = delegate(int delta)
				{
					float num2 = _003C_003E8__1.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
					_003C_003E8__1._003C_003E4__this.workLeft = _003C_003E8__1._003C_003E4__this.workLeft - num2;
					if (_003C_003E8__1._003C_003E4__this.workLeft <= 0f)
					{
						_003C_003E8__1._003C_003E4__this.DoEffect();
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				_003C_003E8__1.doWork.FailOnCannotTouch(TargetIndex.B, jobDriver_AffectRoof.PathEndMode);
				_003C_003E8__1.doWork.PlaySoundAtStart(SoundDefOf.Roof_Start);
				_003C_003E8__1.doWork.PlaySoundAtEnd(SoundDefOf.Roof_Finish);
				_003C_003E8__1.doWork.WithEffect(EffecterDefOf.RoofWork, TargetIndex.A, null);
				_003C_003E8__1.doWork.FailOn(jobDriver_AffectRoof.DoWorkFailOn);
				_003C_003E8__1.doWork.WithProgressBar(TargetIndex.A, () => 1f - _003C_003E8__1._003C_003E4__this.workLeft / 65f);
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
			_003CMakeNewToils_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__12(0)
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

	private float workLeft;

	private const TargetIndex CellInd = TargetIndex.A;

	private const TargetIndex GotoTargetInd = TargetIndex.B;

	private const float BaseWorkAmount = 65f;

	protected IntVec3 Cell => job.GetTarget(TargetIndex.A).Cell;

	protected abstract PathEndMode PathEndMode { get; }

	protected abstract void DoEffect();

	protected abstract bool DoWorkFailOn();

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Cell, job, 1, -1, ReservationLayerDefOf.Ceiling, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__12))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}
}
