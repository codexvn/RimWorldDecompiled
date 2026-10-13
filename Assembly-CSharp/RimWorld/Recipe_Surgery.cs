using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200231A RID: 8986
	public class Recipe_Surgery : RecipeWorker
	{
		// Token: 0x0600C321 RID: 49953 RVA: 0x00389038 File Offset: 0x00387238
		protected bool CheckSurgeryFail(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
		{
			if (this.recipe.surgeryOutcomeEffect == null)
			{
				return false;
			}
			SurgeryOutcome outcome = this.recipe.surgeryOutcomeEffect.GetOutcome(this.recipe, surgeon, patient, ingredients, part, bill);
			if (outcome != null && outcome.failure)
			{
				if (this.recipe.addsHediffOnFailure != null)
				{
					patient.health.AddHediff(this.recipe.addsHediffOnFailure, part, null, null);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C322 RID: 49954 RVA: 0x003890B4 File Offset: 0x003872B4
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if ((this.recipe.genderPrerequisite ?? pawn.gender) != pawn.gender)
			{
				return false;
			}
			if (this.recipe.mustBeFertile && pawn.Sterile())
			{
				return false;
			}
			if (!this.recipe.allowedForQuestLodgers && pawn.IsQuestLodger())
			{
				return false;
			}
			if (this.recipe.minAllowedAge > 0 && pawn.ageTracker.AgeBiologicalYears < this.recipe.minAllowedAge)
			{
				return false;
			}
			if (this.recipe.developmentalStageFilter != null && !this.recipe.developmentalStageFilter.Value.Has(pawn.DevelopmentalStage))
			{
				return false;
			}
			if (this.recipe.humanlikeOnly && !pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive)
			{
				if (this.recipe.mutantBlacklist != null && pawn.IsMutant && this.recipe.mutantBlacklist.Contains(pawn.mutant.Def))
				{
					return false;
				}
				if (this.recipe.mutantPrerequisite != null && (!pawn.IsMutant || !this.recipe.mutantPrerequisite.Contains(pawn.mutant.Def)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C323 RID: 49955 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CompletableEver(Pawn surgeryTarget)
		{
			return true;
		}

		// Token: 0x0600C324 RID: 49956 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void OnSurgerySuccess(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}
	}
}
