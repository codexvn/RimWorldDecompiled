using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Workwatching : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Workwatching _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_Workwatching CS_0024_003C_003E8__locals28 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals28.FailOnDespawnedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals28.FailOnChildLearningConditions();
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					Pawn adultToFollow = CS_0024_003C_003E8__locals28.AdultToFollow;
					if (!LearningGiver_Workwatching.ChildCanLearnFromAdultJob(CS_0024_003C_003E8__locals28.pawn, adultToFollow))
					{
						CS_0024_003C_003E8__locals28.consecutiveTicksNoSkillJob += delta;
						if (CS_0024_003C_003E8__locals28.consecutiveTicksNoSkillJob >= 5)
						{
							Pawn pawn = LearningGiver_Workwatching.AdultToWorkwatch(CS_0024_003C_003E8__locals28.pawn);
							if (pawn == null)
							{
								CS_0024_003C_003E8__locals28.EndJobWith(JobCondition.Incompletable);
							}
							else
							{
								CS_0024_003C_003E8__locals28.consecutiveTicksNoSkillJob = 0;
								CS_0024_003C_003E8__locals28.job.SetTarget(TargetIndex.A, pawn);
							}
							return;
						}
					}
					else
					{
						CS_0024_003C_003E8__locals28.consecutiveTicksNoSkillJob = 0;
						if (CS_0024_003C_003E8__locals28.pawn.Spawned && CS_0024_003C_003E8__locals28.pawn.Map == adultToFollow.MapHeld && CS_0024_003C_003E8__locals28.pawn.Position.InHorDistOf(adultToFollow.Position, 12f))
						{
							float num2 = LearningUtility.LearningRateFactor(CS_0024_003C_003E8__locals28.pawn) * LearningDesireDefOf.Workwatching.xpPerTick * (float)delta;
							SkillDef skillDef = adultToFollow.CurJob?.RecipeDef?.workSkill;
							if (skillDef != null)
							{
								CS_0024_003C_003E8__locals28.pawn.skills.Learn(skillDef, num2);
							}
							else
							{
								List<SkillDef> list = adultToFollow.CurJob?.workGiverDef?.workType?.relevantSkills;
								num2 /= (float)list.Count;
								foreach (SkillDef item in list)
								{
									CS_0024_003C_003E8__locals28.pawn.skills.Learn(item, num2);
								}
							}
						}
					}
					if (!LearningUtility.LearningTickCheckEnd(CS_0024_003C_003E8__locals28.pawn, delta) && (!((adultToFollow.Position - CS_0024_003C_003E8__locals28.pawn.Position).LengthHorizontal <= 3f) || !CS_0024_003C_003E8__locals28.pawn.Position.WithinRegions(adultToFollow.Position, CS_0024_003C_003E8__locals28.pawn.Map, 2, TraverseParms.For(CS_0024_003C_003E8__locals28.pawn))))
					{
						if (!CS_0024_003C_003E8__locals28.pawn.CanReach(adultToFollow, PathEndMode.Touch, Danger.Deadly) || adultToFollow.IsForbidden(CS_0024_003C_003E8__locals28.pawn))
						{
							CS_0024_003C_003E8__locals28.EndJobWith(JobCondition.Incompletable);
						}
						else if (!CS_0024_003C_003E8__locals28.pawn.pather.Moving || CS_0024_003C_003E8__locals28.pawn.pather.Destination != adultToFollow)
						{
							CS_0024_003C_003E8__locals28.pawn.pather.StartPath(adultToFollow, PathEndMode.Touch);
						}
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E2__current = toil;
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const TargetIndex AdultInd = TargetIndex.A;

	private const int FollowDistance = 3;

	private const int StopWatchingNoSkillJobTicks = 5;

	private const float LearningObservationDistance = 12f;

	private int consecutiveTicksNoSkillJob;

	private Pawn AdultToFollow => (Pawn)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref consecutiveTicksNoSkillJob, "consecutiveTicksNoSkillJob", 0);
	}
}
