using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class JobDriver_TakeToBed : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_TakeToBed _003C_003E4__this;

		private Toil _003CgoToTakee_003E5__2;

		private Toil _003CcheckArrestResistance_003E5__3;

		private Toil _003CstartCarrying_003E5__4;

		private Toil _003CgoToBed_003E5__5;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgoToTakee_003E5__2 = null;
			_003CcheckArrestResistance_003E5__3 = null;
			_003CstartCarrying_003E5__4 = null;
			_003CgoToBed_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_TakeToBed CS_0024_003C_003E8__locals57 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals57.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals57.FailOnDestroyedOrNull(TargetIndex.B);
				CS_0024_003C_003E8__locals57.FailOnAggroMentalStateAndHostile(TargetIndex.A);
				CS_0024_003C_003E8__locals57.FailOn(delegate
				{
					if (CS_0024_003C_003E8__locals57.job.def.makeTargetPrisoner)
					{
						if (!CS_0024_003C_003E8__locals57.DropBed.ForPrisoners)
						{
							return true;
						}
					}
					else if (CS_0024_003C_003E8__locals57.DropBed.ForPrisoners != CS_0024_003C_003E8__locals57.Takee.IsPrisoner)
					{
						return true;
					}
					return false;
				});
				_003C_003E2__current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.B, TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals57.AddFinishAction(delegate(JobCondition jobCondition)
				{
					if (CS_0024_003C_003E8__locals57.job.def.makeTargetPrisoner && CS_0024_003C_003E8__locals57.Takee.ownership.OwnedBed == CS_0024_003C_003E8__locals57.DropBed && CS_0024_003C_003E8__locals57.Takee.Position != RestUtility.GetBedSleepingSlotPosFor(CS_0024_003C_003E8__locals57.Takee, CS_0024_003C_003E8__locals57.DropBed))
					{
						CS_0024_003C_003E8__locals57.Takee.ownership.UnclaimBed();
					}
					if (jobCondition != JobCondition.Ongoing && CS_0024_003C_003E8__locals57.pawn.carryTracker.CarriedThing != null)
					{
						CS_0024_003C_003E8__locals57.pawn.carryTracker.TryDropCarriedThing(CS_0024_003C_003E8__locals57.pawn.Position, ThingPlaceMode.Direct, out var _);
					}
				});
				_003CgoToTakee_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.B)
					.FailOn(() => CS_0024_003C_003E8__locals57.job.def == JobDefOf.Arrest && !CS_0024_003C_003E8__locals57.Takee.CanBeArrestedBy(CS_0024_003C_003E8__locals57.pawn))
					.FailOn(() => !CS_0024_003C_003E8__locals57.pawn.CanReach(CS_0024_003C_003E8__locals57.DropBed, PathEndMode.OnCell, Danger.Deadly))
					.FailOn(() => (CS_0024_003C_003E8__locals57.job.def == JobDefOf.Rescue || CS_0024_003C_003E8__locals57.job.def == JobDefOf.Capture) && !CS_0024_003C_003E8__locals57.Takee.Downed)
					.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003CcheckArrestResistance_003E5__3 = ToilMaker.MakeToil("MakeNewToils");
				_003CcheckArrestResistance_003E5__3.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals57.job.def.makeTargetPrisoner)
					{
						Pawn pawn = (Pawn)CS_0024_003C_003E8__locals57.job.targetA.Thing;
						pawn.GetLord()?.Notify_PawnAttemptArrested(pawn);
						GenClamor.DoClamor(pawn, 10f, ClamorDefOf.Harm);
						if (!pawn.IsPrisoner && !pawn.IsSlave)
						{
							bool num2 = pawn.IsQuestLodger();
							Faction homeFaction = pawn.HomeFaction;
							QuestUtility.SendQuestTargetSignals(pawn.questTags, "Arrested", pawn.Named("SUBJECT"));
							if (pawn.Faction != null)
							{
								QuestUtility.SendQuestTargetSignals(pawn.Faction.questTags, "FactionMemberArrested", pawn.Faction.Named("FACTION"));
							}
							if (num2)
							{
								pawn.SetFaction(homeFaction);
							}
						}
						if (CS_0024_003C_003E8__locals57.job.def == JobDefOf.Arrest && !pawn.CheckAcceptArrest(CS_0024_003C_003E8__locals57.pawn))
						{
							CS_0024_003C_003E8__locals57.pawn.jobs.EndCurrentJob(JobCondition.Incompletable);
						}
					}
				};
				_003C_003E2__current = Toils_Jump.JumpIf(_003CcheckArrestResistance_003E5__3, () => CS_0024_003C_003E8__locals57.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals57.Takee));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToTakee_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcheckArrestResistance_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003CstartCarrying_003E5__4 = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003CstartCarrying_003E5__4.FailOnBedNoLongerUsable(TargetIndex.B, TargetIndex.A);
				_003CstartCarrying_003E5__4.AddPreInitAction(CS_0024_003C_003E8__locals57.CheckMakeTakeeGuest);
				_003CstartCarrying_003E5__4.AddFinishAction(delegate
				{
					if (CS_0024_003C_003E8__locals57.pawn.Faction == CS_0024_003C_003E8__locals57.Takee.Faction)
					{
						CS_0024_003C_003E8__locals57.CheckMakeTakeePrisoner();
					}
				});
				_003CgoToBed_003E5__5 = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOn(() => !CS_0024_003C_003E8__locals57.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals57.Takee));
				_003CgoToBed_003E5__5.FailOnBedNoLongerUsable(TargetIndex.B, TargetIndex.A);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CgoToBed_003E5__5, () => CS_0024_003C_003E8__locals57.pawn.IsCarryingPawn(CS_0024_003C_003E8__locals57.Takee));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCarrying_003E5__4;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgoToBed_003E5__5;
				_003C_003E1__state = 7;
				return true;
			case 7:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					CS_0024_003C_003E8__locals57.CheckMakeTakeePrisoner();
					if (CS_0024_003C_003E8__locals57.Takee.playerSettings == null)
					{
						CS_0024_003C_003E8__locals57.Takee.playerSettings = new Pawn_PlayerSettings(CS_0024_003C_003E8__locals57.Takee);
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 8;
				return true;
			}
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Release(TargetIndex.B);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Bed.TuckIntoBed(CS_0024_003C_003E8__locals57.DropBed, CS_0024_003C_003E8__locals57.pawn, CS_0024_003C_003E8__locals57.Takee, CS_0024_003C_003E8__locals57.TakeeRescued);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (!CS_0024_003C_003E8__locals57.job.ritualTag.NullOrEmpty())
					{
						if (CS_0024_003C_003E8__locals57.Takee.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual)
						{
							lordJob_Ritual.AddTagForPawn(CS_0024_003C_003E8__locals57.Takee, CS_0024_003C_003E8__locals57.job.ritualTag);
						}
						if (CS_0024_003C_003E8__locals57.pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual2)
						{
							lordJob_Ritual2.AddTagForPawn(CS_0024_003C_003E8__locals57.pawn, CS_0024_003C_003E8__locals57.job.ritualTag);
						}
					}
				});
				_003C_003E1__state = 11;
				return true;
			case 11:
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
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private const TargetIndex TakeeIndex = TargetIndex.A;

	private const TargetIndex BedIndex = TargetIndex.B;

	protected Pawn Takee => (Pawn)job.GetTarget(TargetIndex.A).Thing;

	protected Building_Bed DropBed => (Building_Bed)job.GetTarget(TargetIndex.B).Thing;

	private bool TakeeRescued
	{
		get
		{
			if (Takee.RaceProps.Humanlike && job.def != JobDefOf.Arrest && !Takee.IsPrisonerOfColony)
			{
				if (Takee.ageTracker.CurLifeStage.alwaysDowned)
				{
					return HealthAIUtility.ShouldSeekMedicalRest(Takee);
				}
				return true;
			}
			return false;
		}
	}

	public override string GetReport()
	{
		if (job.def == JobDefOf.Rescue && !TakeeRescued)
		{
			return "TakingToBed".Translate(Takee);
		}
		return base.GetReport();
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		Takee.ClearAllReservations();
		if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DropBed, job, DropBed.SleepingSlotsCount, 0, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void CheckMakeTakeePrisoner()
	{
		if (job.def.makeTargetPrisoner)
		{
			if (Takee.guest.Released)
			{
				Takee.guest.Released = false;
				Takee.guest.SetExclusiveInteraction(PrisonerInteractionModeDefOf.MaintainOnly);
				GenGuest.RemoveHealthyPrisonerReleasedThoughts(Takee);
			}
			if (!Takee.IsPrisonerOfColony)
			{
				Takee.guest.CapturedBy(Faction.OfPlayer, pawn);
			}
		}
	}

	private void CheckMakeTakeeGuest()
	{
		if (!job.def.makeTargetPrisoner && Takee.Faction != Faction.OfPlayer && Takee.HostFaction != Faction.OfPlayer && Takee.guest != null && !Takee.IsWildMan() && Takee.DevelopmentalStage != DevelopmentalStage.Baby)
		{
			Takee.guest.SetGuestStatus(Faction.OfPlayer);
			QuestUtility.SendQuestTargetSignals(Takee.questTags, "Rescued", Takee.Named("SUBJECT"));
		}
	}
}
