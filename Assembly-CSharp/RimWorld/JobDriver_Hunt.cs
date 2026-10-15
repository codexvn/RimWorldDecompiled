using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Hunt : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Hunt _003C_003E4__this;

		private Toil _003CstartCollectCorpseLabel_003E5__2;

		private Toil _003CslaughterLabel_003E5__3;

		private Toil _003CgotoCastPos_003E5__4;

		private Toil _003CslaughterIfPossible_003E5__5;

		private Toil _003CcarryToCell_003E5__6;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CstartCollectCorpseLabel_003E5__2 = null;
			_003CslaughterLabel_003E5__3 = null;
			_003CgotoCastPos_003E5__4 = null;
			_003CslaughterIfPossible_003E5__5 = null;
			_003CcarryToCell_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Hunt CS_0024_003C_003E8__locals18 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals18.FailOn(delegate
				{
					if (CS_0024_003C_003E8__locals18.Victim == null)
					{
						return true;
					}
					if (CS_0024_003C_003E8__locals18.Victim.IsForbidden(CS_0024_003C_003E8__locals18.pawn))
					{
						return true;
					}
					if (!CS_0024_003C_003E8__locals18.job.ignoreDesignations)
					{
						Pawn victim2 = CS_0024_003C_003E8__locals18.Victim;
						if (victim2 != null && !victim2.Dead && CS_0024_003C_003E8__locals18.Map.designationManager.DesignationOn(victim2, DesignationDefOf.Hunt) == null)
						{
							return true;
						}
					}
					return false;
				});
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals18.jobStartTick = Find.TickManager.TicksGame;
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Combat.TrySetJobToUseAttackVerb(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CstartCollectCorpseLabel_003E5__2 = Toils_General.Label();
				_003CslaughterLabel_003E5__3 = Toils_General.Label();
				_003CgotoCastPos_003E5__4 = Toils_Combat.GotoCastPosition(TargetIndex.A, TargetIndex.None, closeIfDowned: true, 0.95f).JumpIfDespawnedOrNull(TargetIndex.A, _003CstartCollectCorpseLabel_003E5__2).FailOn(() => Find.TickManager.TicksGame > CS_0024_003C_003E8__locals18.jobStartTick + 5000);
				_003C_003E2__current = _003CgotoCastPos_003E5__4;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003CslaughterIfPossible_003E5__5 = Toils_Jump.JumpIf(_003CslaughterLabel_003E5__3, delegate
				{
					Pawn victim = CS_0024_003C_003E8__locals18.Victim;
					if (victim.RaceProps.DeathActionWorker != null && victim.RaceProps.DeathActionWorker.DangerousInMelee)
					{
						return false;
					}
					return victim.Downed ? true : false;
				});
				_003C_003E2__current = _003CslaughterIfPossible_003E5__5;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIfTargetNotHittable(TargetIndex.A, _003CgotoCastPos_003E5__4);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Combat.CastVerb(TargetIndex.A, canHitNonTargetPawns: false).JumpIfDespawnedOrNull(TargetIndex.A, _003CstartCollectCorpseLabel_003E5__2).FailOn(() => Find.TickManager.TicksGame > CS_0024_003C_003E8__locals18.jobStartTick + 5000);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.JumpIfTargetDespawnedOrNull(TargetIndex.A, _003CstartCollectCorpseLabel_003E5__2);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CslaughterIfPossible_003E5__5);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CslaughterLabel_003E5__3;
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnMobile(TargetIndex.A);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.WaitWith(TargetIndex.A, 180, useProgressBar: true).FailOnMobile(TargetIndex.A);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (!CS_0024_003C_003E8__locals18.Victim.Dead)
					{
						ExecutionUtility.DoHuntingExecution(CS_0024_003C_003E8__locals18.pawn, CS_0024_003C_003E8__locals18.Victim);
						CS_0024_003C_003E8__locals18.pawn.records.Increment(RecordDefOf.AnimalsSlaughtered);
						if (CS_0024_003C_003E8__locals18.pawn.InMentalState)
						{
							CS_0024_003C_003E8__locals18.pawn.MentalState.Notify_SlaughteredTarget();
						}
					}
				});
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Jump.Jump(_003CstartCollectCorpseLabel_003E5__2);
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCollectCorpseLabel_003E5__2;
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals18.StartCollectCorpseToil();
				_003C_003E1__state = 15;
				return true;
			case 15:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 16;
				return true;
			case 16:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 17;
				return true;
			case 17:
				_003C_003E1__state = -1;
				_003CcarryToCell_003E5__6 = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				_003C_003E2__current = _003CcarryToCell_003E5__6;
				_003C_003E1__state = 18;
				return true;
			case 18:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, _003CcarryToCell_003E5__6, storageMode: true);
				_003C_003E1__state = 19;
				return true;
			case 19:
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	private int jobStartTick = -1;

	private const TargetIndex VictimInd = TargetIndex.A;

	private const TargetIndex CorpseInd = TargetIndex.A;

	private const TargetIndex StoreCellInd = TargetIndex.B;

	private const int MaxHuntTicks = 5000;

	public const float MaxRangeFactor = 0.95f;

	public Pawn Victim
	{
		get
		{
			Corpse corpse = Corpse;
			if (corpse != null)
			{
				return corpse.InnerPawn;
			}
			return (Pawn)job.GetTarget(TargetIndex.A).Thing;
		}
	}

	private Corpse Corpse => job.GetTarget(TargetIndex.A).Thing as Corpse;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref jobStartTick, "jobStartTick", 0);
	}

	public override string GetReport()
	{
		if (Victim != null)
		{
			return JobUtility.GetResolvedJobReport(job.def.reportString, Victim);
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil StartCollectCorpseToil()
	{
		Toil toil = ToilMaker.MakeToil("StartCollectCorpseToil");
		toil.initAction = delegate
		{
			if (Victim == null)
			{
				toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			else
			{
				TaleRecorder.RecordTale(TaleDefOf.Hunted, pawn, Victim);
				Corpse corpse = Victim.Corpse;
				if (corpse == null || !pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly))
				{
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
				}
				else
				{
					corpse.SetForbidden(value: false);
					if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, pawn, base.Map, StoragePriority.Unstored, pawn.Faction, out var foundCell))
					{
						pawn.Reserve(corpse, job);
						pawn.Reserve(foundCell, job);
						job.SetTarget(TargetIndex.B, foundCell);
						job.SetTarget(TargetIndex.A, corpse);
						job.count = 1;
						job.haulMode = HaulMode.ToCellStorage;
					}
					else
					{
						pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
					}
				}
			}
		};
		return toil;
	}
}
