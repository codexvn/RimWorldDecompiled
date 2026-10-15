using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ClearSnowAndSand : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public Toil clearToil;

		public JobDriver_ClearSnowAndSand _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__0(int delta)
		{
			float statValue = clearToil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed);
			_003C_003E4__this.workDone += statValue;
			if (_003C_003E4__this.workDone >= _003C_003E4__this.TotalNeededWork)
			{
				_003C_003E4__this.Map.snowGrid.SetDepth(_003C_003E4__this.TargetLocA, 0f);
				_003C_003E4__this.Map.sandGrid?.SetDepth(_003C_003E4__this.TargetLocA, 0f);
				_003C_003E4__this.ReadyForNextToil();
			}
		}

		internal float _003CMakeNewToils_003Eb__2()
		{
			return _003C_003E4__this.workDone / _003C_003E4__this.TotalNeededWork;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ClearSnowAndSand _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
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
			JobDriver_ClearSnowAndSand jobDriver_ClearSnowAndSand = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1.clearToil = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.clearToil.tickIntervalAction = delegate
				{
					float statValue = _003C_003E8__1.clearToil.actor.GetStatValue(StatDefOf.GeneralLaborSpeed);
					_003C_003E8__1._003C_003E4__this.workDone = _003C_003E8__1._003C_003E4__this.workDone + statValue;
					if (_003C_003E8__1._003C_003E4__this.workDone >= _003C_003E8__1._003C_003E4__this.TotalNeededWork)
					{
						_003C_003E8__1._003C_003E4__this.Map.snowGrid.SetDepth(_003C_003E8__1._003C_003E4__this.TargetLocA, 0f);
						_003C_003E8__1._003C_003E4__this.Map.sandGrid?.SetDepth(_003C_003E8__1._003C_003E4__this.TargetLocA, 0f);
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
				};
				EffecterDef effectDef = (jobDriver_ClearSnowAndSand.IsSand(jobDriver_ClearSnowAndSand.TargetLocA) ? EffecterDefOf.ClearSand : EffecterDefOf.ClearSnow);
				_003C_003E8__1.clearToil.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.clearToil.WithEffect(effectDef, TargetIndex.A, null);
				_003C_003E8__1.clearToil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth);
				_003C_003E8__1.clearToil.WithProgressBar(TargetIndex.A, () => _003C_003E8__1._003C_003E4__this.workDone / _003C_003E8__1._003C_003E4__this.TotalNeededWork, interpolateBetweenActorAndTarget: true);
				_003C_003E8__1.clearToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E2__current = _003C_003E8__1.clearToil;
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private float workDone;

	private const float ClearWorkPerDepositedDepth = 50f;

	private float TotalNeededWork => 50f * (base.Map.snowGrid.GetDepth(base.TargetLocA) + base.Map.sandGrid?.GetDepth(base.TargetLocA)).GetValueOrDefault();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool IsSand(IntVec3 cell)
	{
		float? num = base.Map?.sandGrid?.GetDepth(cell);
		if (num.HasValue)
		{
			return num.GetValueOrDefault() > base.Map?.snowGrid.GetDepth(cell);
		}
		return false;
	}

	public override string GetReport()
	{
		string reportStringOverride = GetReportStringOverride();
		if (!reportStringOverride.NullOrEmpty())
		{
			return reportStringOverride;
		}
		LocalTargetInfo localTargetInfo = (job.targetA.IsValid ? job.targetA : job.targetQueueA.FirstValid());
		if (localTargetInfo.Cell.IsValid && IsSand(localTargetInfo.Cell))
		{
			return "JobClearingSand".Translate();
		}
		return ReportStringProcessed(job.def.reportString);
	}
}
