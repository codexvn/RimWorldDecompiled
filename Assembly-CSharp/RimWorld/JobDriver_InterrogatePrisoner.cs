using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001375 RID: 4981
	public class JobDriver_InterrogatePrisoner : JobDriver
	{
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06007A2F RID: 31279 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x0024D683 File Offset: 0x0024B883
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModsConfig.AnomalyActive)
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
				yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, PrisonerInteractionModeDefOf.Interrogate);
				yield return Toils_Interpersonal.WaitToBeAbleToInteract(this.pawn);
				yield return Toils_Interpersonal.GotoInteractablePosition(TargetIndex.A);
				yield return Toils_Interpersonal.Interrogate(this.pawn, this.Prisoner);
				num = i;
			}
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield break;
		}

		// Token: 0x040052B7 RID: 21175
		private const int NumTalks = 3;
	}
}
