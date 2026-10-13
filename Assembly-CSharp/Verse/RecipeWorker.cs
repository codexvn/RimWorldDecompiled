using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public class RecipeWorker
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return true;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x000489C0 File Offset: 0x00046BC0
		public virtual AcceptanceReport AvailableReport(Thing thing, BodyPartRecord part = null)
		{
			return this.AvailableOnNow(thing, part);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x000489CF File Offset: 0x00046BCF
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return Enumerable.Empty<BodyPartRecord>();
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x000489D6 File Offset: 0x00046BD6
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x000489F6 File Offset: 0x00046BF6
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00048A03 File Offset: 0x00046C03
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00048A0C File Offset: 0x00046C0C
		public virtual float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			return ing.GetBaseCount();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00048A14 File Offset: 0x00046C14
		public virtual TaggedString GetConfirmation(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00048A1C File Offset: 0x00046C1C
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact, HistoryEventDef overrideEventDef = null)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction == Faction.OfPlayer)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(factionToInform, goodwillImpact, true, !factionToInform.temporary, overrideEventDef ?? HistoryEventDefOf.PerformedHarmfulSurgery, null);
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string LabelFromUniqueIngredients(Bill bill)
		{
			return null;
		}

		// Token: 0x04000B37 RID: 2871
		public RecipeDef recipe;
	}
}
