using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200149D RID: 5277
	public class JobDriver_PrisonerBloodfeed : JobDriver
	{
		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06007EB9 RID: 32441 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x00262265 File Offset: 0x00260465
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.Prisoner.IsPrisonerOfColony || !this.Prisoner.guest.PrisonerIsSecure || this.Prisoner.InAggroMentalState || this.Prisoner.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Bloodfeed));
			yield return Toils_Interpersonal.GotoPrisoner(this.pawn, this.Prisoner, PrisonerInteractionModeDefOf.Bloodfeed);
			yield return Toils_General.WaitWith(TargetIndex.A, 120, true, false, false, TargetIndex.None, PathEndMode.Touch).PlaySustainerOrSound(SoundDefOf.Bloodfeed_Cast, 1f);
			yield return Toils_General.Do(delegate
			{
				SanguophageUtility.DoBite(this.pawn, this.Prisoner, 0.2f, 0.1f, 0.4499f, 1f, IntRange.One, ThoughtDefOf.FedOn, ThoughtDefOf.FedOn_Social);
			});
			yield return Toils_Interpersonal.SetLastInteractTime(TargetIndex.A);
			yield break;
		}

		// Token: 0x040054F4 RID: 21748
		public const float BloodLoss = 0.4499f;

		// Token: 0x040054F5 RID: 21749
		public const int WaitTicks = 120;

		// Token: 0x040054F6 RID: 21750
		private const float HemogenGain = 0.2f;

		// Token: 0x040054F7 RID: 21751
		private const float NutritionGain = 0.1f;
	}
}
