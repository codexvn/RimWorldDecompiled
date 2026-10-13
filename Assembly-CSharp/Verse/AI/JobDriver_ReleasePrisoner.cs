using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E5C RID: 3676
	public class JobDriver_ReleasePrisoner : JobDriver
	{
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005C97 RID: 23703 RVA: 0x001D89DC File Offset: 0x001D6BDC
		private Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x001D8A02 File Offset: 0x001D6C02
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Prisoner, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x001D8A25 File Offset: 0x001D6C25
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			this.FailOn(() => ((Pawn)((Thing)this.GetActor().CurJob.GetTarget(TargetIndex.A))).guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Release));
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, false, false);
			Toil setReleased = ToilMaker.MakeToil("MakeNewToils");
			setReleased.initAction = delegate()
			{
				Pawn pawn = setReleased.actor.jobs.curJob.targetA.Thing as Pawn;
				GenGuest.PrisonerRelease(pawn);
				if (!PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Map.Tile))
				{
					GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "Released", pawn.Named("SUBJECT"));
			};
			yield return setReleased;
			yield break;
		}

		// Token: 0x040042F7 RID: 17143
		private const TargetIndex PrisonerInd = TargetIndex.A;

		// Token: 0x040042F8 RID: 17144
		private const TargetIndex ReleaseCellInd = TargetIndex.B;
	}
}
