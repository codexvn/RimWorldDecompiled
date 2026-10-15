using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Floordrawing : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Floordrawing _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
			JobDriver_Floordrawing CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals16.FailOnForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals16.FailOnForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals16.FailOnChildLearningConditions();
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals16.pawn.jobs.posture = PawnPosture.Standing;
				};
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals16.pawn.rotationTracker.FaceCell(CS_0024_003C_003E8__locals16.TargetB.Cell);
					if (!LearningUtility.LearningTickCheckEnd(CS_0024_003C_003E8__locals16.pawn, delta) && CS_0024_003C_003E8__locals16.pawn.IsHashIntervalTick(2500, delta))
					{
						if (!FilthMaker.TryMakeFilth(CS_0024_003C_003E8__locals16.TargetB.Cell, CS_0024_003C_003E8__locals16.pawn.Map, ThingDefOf.Filth_Floordrawing))
						{
							CS_0024_003C_003E8__locals16.pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
						}
						else
						{
							List<Thing> thingList = CS_0024_003C_003E8__locals16.TargetB.Cell.GetThingList(CS_0024_003C_003E8__locals16.pawn.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								if (thingList[i].def == ThingDefOf.Filth_Floordrawing)
								{
									CS_0024_003C_003E8__locals16.pawn.Reserve(thingList[i], CS_0024_003C_003E8__locals16.job);
								}
							}
						}
					}
				};
				toil.WithEffect(EffecterDefOf.Floordrawing, TargetIndex.A, null);
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = CS_0024_003C_003E8__locals16.job.def.learningDuration;
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
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
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

	private const int DrawingIntervalTicks = 2500;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(base.TargetA, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(base.TargetB, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
