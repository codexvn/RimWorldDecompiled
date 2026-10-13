using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022F6 RID: 8950
	public class Recipe_BloodTransfusion : Recipe_Surgery
	{
		// Token: 0x0600C295 RID: 49813 RVA: 0x00386FA8 File Offset: 0x003851A8
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return surgeryTarget.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false) && base.CompletableEver(surgeryTarget);
		}

		// Token: 0x0600C296 RID: 49814 RVA: 0x00386FCC File Offset: 0x003851CC
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (thing.MapHeld == null)
			{
				return false;
			}
			if (thing.MapHeld.listerThings.ThingsOfDef(ThingDefOf.HemogenPack).Count == 0)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return (pawn == null || pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false)) && base.AvailableOnNow(thing, part);
		}

		// Token: 0x0600C297 RID: 49815 RVA: 0x000026EA File Offset: 0x000008EA
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x0600C298 RID: 49816 RVA: 0x00387030 File Offset: 0x00385230
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (!ingredients[i].def.IsMedicine)
				{
					num += 0.35f * (float)ingredients[i].stackCount;
					num2 += JobGiver_GetHemogen.HemogenPackHemogenGain * (float)ingredients[i].stackCount;
				}
			}
			if (num > 0f)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity -= num;
				}
			}
			if (num2 > 0f)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) != null)
				{
					GeneUtility.OffsetHemogen(pawn, num2, true);
				}
			}
			for (int j = 0; j < ingredients.Count; j++)
			{
				ingredients[j].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600C299 RID: 49817 RVA: 0x00387120 File Offset: 0x00385320
		public override float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			BillStack billStack = bill.billStack;
			Pawn pawn = ((billStack != null) ? billStack.billGiver : null) as Pawn;
			if (pawn == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
			return Mathf.Min((float)bill.Map.listerThings.ThingsOfDef(ThingDefOf.HemogenPack).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / 0.35f);
		}

		// Token: 0x040084D6 RID: 34006
		public const float BloodlossHealedPerPack = 0.35f;
	}
}
