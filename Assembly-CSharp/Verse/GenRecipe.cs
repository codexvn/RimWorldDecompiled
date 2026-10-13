using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D05 RID: 3333
	public static class GenRecipe
	{
		// Token: 0x0600554B RID: 21835 RVA: 0x001B7E44 File Offset: 0x001B6044
		public static IEnumerable<Thing> MakeRecipeProducts(RecipeDef recipeDef, Pawn worker, List<Thing> ingredients, Thing dominantIngredient, IBillGiver billGiver, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			float efficiency;
			if (recipeDef.efficiencyStat == null)
			{
				efficiency = 1f;
			}
			else
			{
				efficiency = worker.GetStatValue(recipeDef.efficiencyStat, true, -1);
			}
			if (recipeDef.workTableEfficiencyStat != null)
			{
				Building_WorkTable building_WorkTable = billGiver as Building_WorkTable;
				if (building_WorkTable != null)
				{
					efficiency *= building_WorkTable.GetStatValue(recipeDef.workTableEfficiencyStat, true, -1);
				}
			}
			if (recipeDef.products != null)
			{
				int num;
				for (int i = 0; i < recipeDef.products.Count; i = num + 1)
				{
					ThingDefCountClass thingDefCountClass = recipeDef.products[i];
					ThingDef stuff = thingDefCountClass.thingDef.MadeFromStuff ? dominantIngredient.def : null;
					Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, stuff);
					thing.stackCount = Mathf.CeilToInt((float)thingDefCountClass.count * efficiency);
					if (dominantIngredient != null && recipeDef.useIngredientsForColor)
					{
						thing.SetColor(dominantIngredient.DrawColor, false);
					}
					CompIngredients compIngredients = thing.TryGetComp<CompIngredients>();
					if (compIngredients != null)
					{
						for (int k = 0; k < ingredients.Count; k++)
						{
							compIngredients.RegisterIngredient(ingredients[k].def);
						}
					}
					thing.Notify_RecipeProduced(worker);
					yield return GenRecipe.PostProcessProduct(thing, recipeDef, worker, precept, style, overrideGraphicIndex);
					num = i;
				}
			}
			if (recipeDef.specialProducts != null)
			{
				int num;
				for (int i = 0; i < recipeDef.specialProducts.Count; i = num + 1)
				{
					for (int j = 0; j < ingredients.Count; j = num + 1)
					{
						Thing thing2 = ingredients[j];
						SpecialProductType specialProductType = recipeDef.specialProducts[i];
						if (specialProductType != SpecialProductType.Butchery)
						{
							if (specialProductType == SpecialProductType.Smelted)
							{
								foreach (Thing product in thing2.SmeltProducts(efficiency))
								{
									yield return GenRecipe.PostProcessProduct(product, recipeDef, worker, precept, style, overrideGraphicIndex);
								}
								IEnumerator<Thing> enumerator = null;
							}
						}
						else
						{
							foreach (Thing product2 in thing2.ButcherProducts(worker, efficiency))
							{
								yield return GenRecipe.PostProcessProduct(product2, recipeDef, worker, precept, style, overrideGraphicIndex);
							}
							IEnumerator<Thing> enumerator = null;
						}
						num = j;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x001B7E94 File Offset: 0x001B6094
		public static IEnumerable<Thing> FinalizeGestatedPawns(Bill_Mech bill, Pawn worker, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			yield return GenRecipe.PostProcessProduct(bill.Gestator.GestatingMech, bill.recipe, worker, bill.precept, style, overrideGraphicIndex);
			yield break;
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x001B7EBC File Offset: 0x001B60BC
		private static Thing PostProcessProduct(Thing product, RecipeDef recipeDef, Pawn worker, Precept_ThingStyle precept = null, ThingStyleDef style = null, int? overrideGraphicIndex = null)
		{
			CompQuality compQuality = product.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				if (recipeDef.workSkill == null)
				{
					Log.Error(((recipeDef != null) ? recipeDef.ToString() : null) + " needs workSkill because it creates a product with a quality.");
				}
				QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, recipeDef.workSkill, true);
				compQuality.SetQuality(q, new ArtGenerationContext?(ArtGenerationContext.Colony));
			}
			CompArt compArt = product.TryGetComp<CompArt>();
			if (compArt != null)
			{
				compArt.JustCreatedBy(worker);
				if (compQuality != null && compQuality.Quality >= QualityCategory.Excellent)
				{
					TaleRecorder.RecordTale(TaleDefOf.CraftedArt, new object[]
					{
						worker,
						product
					});
				}
			}
			if (compQuality != null)
			{
				QualityUtility.SendCraftNotification(product, worker);
			}
			if (worker.Ideo != null)
			{
				product.StyleDef = worker.Ideo.GetStyleFor(product.def);
			}
			if (precept != null)
			{
				product.StyleSourcePrecept = precept;
			}
			else if (style != null)
			{
				product.StyleDef = style;
			}
			else if (!product.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(product.def.randomStyleChance))
			{
				product.SetStyleDef(product.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef);
			}
			product.overrideGraphicIndex = overrideGraphicIndex;
			if (product.def.Minifiable)
			{
				product = product.MakeMinified(DestroyMode.Vanish);
			}
			return product;
		}
	}
}
