using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200136F RID: 4975
	public class JobDriver_EnslaveOrReduceWillPrisoner : JobDriver
	{
		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06007A0F RID: 31247 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x0024D0FF File Offset: 0x0024B2FF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure);
			PrisonerInteractionModeDef interaction = this.Prisoner.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Enslave) ? PrisonerInteractionModeDefOf.Enslave : PrisonerInteractionModeDefOf.ReduceWill;
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, interaction);
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ReduceWill(this.pawn, this.Prisoner);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, interaction);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
			yield return Toils_Interpersonal.ReduceWill(this.pawn, this.Prisoner);
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, interaction);
			yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A).FailOn(() => !this.Prisoner.guest.ScheduledForInteraction);
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield return Toils_Interpersonal.TryEnslave(TargetIndex.A);
			yield break;
		}
	}
}
