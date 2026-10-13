using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200136D RID: 4973
	public class JobDriver_ConvertPrisoner : JobDriver
	{
		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06007A01 RID: 31233 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007A03 RID: 31235 RVA: 0x0024CE4B File Offset: 0x0024B04B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Ideoligion conversion"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure);
			int num;
			for (int i = 0; i < 3; i = num + 1)
			{
				yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, PrisonerInteractionModeDefOf.Convert);
				yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
				yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				yield return Toils_Interpersonal.ConvinceRecruitee(this.pawn, this.Prisoner, InteractionDefOf.Chitchat);
				num = i;
			}
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, PrisonerInteractionModeDefOf.Convert);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Prisoner.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryConvert(TargetIndex.A);
			yield break;
		}

		// Token: 0x040052A3 RID: 21155
		private const int NumTalks = 3;
	}
}
