using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Reign : JobDriver_Meditate
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Reign _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Reign CS_0024_003C_003E8__locals19 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals19.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals19.job.SetTarget(TargetIndex.B, CS_0024_003C_003E8__locals19.Throne.InteractionCell + CS_0024_003C_003E8__locals19.Throne.Rotation.FacingCell);
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(CS_0024_003C_003E8__locals19.Throne.GetRoom()) != null);
				toil.FailOn(() => !MeditationUtility.CanMeditateNow(CS_0024_003C_003E8__locals19.pawn) || !MeditationUtility.SafeEnvironmentalConditions(CS_0024_003C_003E8__locals19.pawn, CS_0024_003C_003E8__locals19.TargetLocA, CS_0024_003C_003E8__locals19.Map));
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = CS_0024_003C_003E8__locals19.job.def.joyDuration;
				toil.tickAction = delegate
				{
					if (CS_0024_003C_003E8__locals19.pawn.mindState.applyThroneThoughtsTick == 0)
					{
						CS_0024_003C_003E8__locals19.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 10000;
					}
					else if (CS_0024_003C_003E8__locals19.pawn.mindState.applyThroneThoughtsTick <= Find.TickManager.TicksGame)
					{
						CS_0024_003C_003E8__locals19.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 60000;
						ThoughtDef thoughtDef = null;
						if (CS_0024_003C_003E8__locals19.Throne.GetRoom().Role == RoomRoleDefOf.ThroneRoom)
						{
							thoughtDef = ThoughtDefOf.ReignedInThroneroom;
						}
						if (thoughtDef != null)
						{
							int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(CS_0024_003C_003E8__locals19.Throne.GetRoom().GetStat(RoomStatDefOf.Impressiveness));
							if (thoughtDef.stages[scoreStageIndex] != null)
							{
								CS_0024_003C_003E8__locals19.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex));
							}
						}
					}
					CS_0024_003C_003E8__locals19.rotateToFace = TargetIndex.B;
					CS_0024_003C_003E8__locals19.MeditationTick();
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
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

	protected const TargetIndex FacingInd = TargetIndex.B;

	protected const int ApplyThoughtInitialTicks = 10000;

	private Building_Throne Throne => base.TargetThingA as Building_Throne;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Throne, job, 1, -1, null, errorOnFailed);
	}

	public override string GetReport()
	{
		return ReportStringProcessed(job.def.reportString) + ": " + Throne.LabelShort.CapitalizeFirst() + "." + PsyfocusPerDayReport();
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		return false;
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
