using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001906 RID: 6406
	public static class RecipeDefGenerator
	{
		// Token: 0x06009191 RID: 37265 RVA: 0x002AD773 File Offset: 0x002AB973
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs(bool hotReload = false)
		{
			foreach (RecipeDef recipeDef in RecipeDefGenerator.DefsFromRecipeMakers(hotReload).Concat(RecipeDefGenerator.DrugAdministerDefs(hotReload)))
			{
				yield return recipeDef;
			}
			IEnumerator<RecipeDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009192 RID: 37266 RVA: 0x002AD783 File Offset: 0x002AB983
		private static IEnumerable<RecipeDef> DefsFromRecipeMakers(bool hotReload = false)
		{
			foreach (ThingDef def in from d in DefDatabase<ThingDef>.AllDefs
			where d.recipeMaker != null
			select d)
			{
				yield return RecipeDefGenerator.CreateRecipeDefFromMaker(def, 1, hotReload);
				if (def.recipeMaker.bulkRecipeCount > 0)
				{
					yield return RecipeDefGenerator.CreateRecipeDefFromMaker(def, def.recipeMaker.bulkRecipeCount, hotReload);
				}
				def = null;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009193 RID: 37267 RVA: 0x002AD794 File Offset: 0x002AB994
		private static RecipeDef CreateRecipeDefFromMaker(ThingDef def, int adjustedCount = 1, bool hotReload = false)
		{
			RecipeMakerProperties recipeMaker = def.recipeMaker;
			string text = "Make_" + def.defName;
			if (adjustedCount != 1)
			{
				text += "Bulk";
			}
			RecipeDef recipeDef = hotReload ? (DefDatabase<RecipeDef>.GetNamed(text, false) ?? new RecipeDef()) : new RecipeDef();
			recipeDef.defName = text;
			string text2 = def.label;
			if (adjustedCount != 1)
			{
				text2 = text2 + " x" + adjustedCount.ToString();
			}
			if (string.IsNullOrEmpty(recipeMaker.label))
			{
				recipeDef.label = "RecipeMake".Translate(text2);
			}
			else
			{
				recipeDef.label = recipeMaker.label;
			}
			recipeDef.jobString = "RecipeMakeJobString".Translate(text2);
			recipeDef.modContentPack = def.modContentPack;
			recipeDef.displayPriority = recipeMaker.displayPriority + adjustedCount - 1;
			recipeDef.workAmount = (float)(recipeMaker.workAmount * adjustedCount);
			recipeDef.workSpeedStat = recipeMaker.workSpeedStat;
			recipeDef.efficiencyStat = recipeMaker.efficiencyStat;
			RecipeDefGenerator.SetIngredients(recipeDef, def, adjustedCount);
			recipeDef.useIngredientsForColor = recipeMaker.useIngredientsForColor;
			if (def.costListForDifficulty != null)
			{
				recipeDef.regenerateOnDifficultyChange = true;
			}
			recipeDef.defaultIngredientFilter = recipeMaker.defaultIngredientFilter;
			recipeDef.products.Add(new ThingDefCountClass(def, recipeMaker.productCount * adjustedCount));
			recipeDef.targetCountAdjustment = recipeMaker.targetCountAdjustment * adjustedCount;
			recipeDef.skillRequirements = recipeMaker.skillRequirements.ListFullCopyOrNull<SkillRequirement>();
			recipeDef.workSkill = recipeMaker.workSkill;
			recipeDef.workSkillLearnFactor = recipeMaker.workSkillLearnPerTick;
			recipeDef.requiredGiverWorkType = recipeMaker.requiredGiverWorkType;
			recipeDef.unfinishedThingDef = recipeMaker.unfinishedThingDef;
			recipeDef.recipeUsers = recipeMaker.recipeUsers.ListFullCopyOrNull<ThingDef>();
			recipeDef.mechanitorOnlyRecipe = recipeMaker.mechanitorOnlyRecipe;
			recipeDef.effectWorking = recipeMaker.effectWorking;
			recipeDef.soundWorking = recipeMaker.soundWorking;
			recipeDef.researchPrerequisite = recipeMaker.researchPrerequisite;
			recipeDef.memePrerequisitesAny = recipeMaker.memePrerequisitesAny;
			recipeDef.researchPrerequisites = recipeMaker.researchPrerequisites;
			recipeDef.factionPrerequisiteTags = recipeMaker.factionPrerequisiteTags;
			recipeDef.fromIdeoBuildingPreceptOnly = recipeMaker.fromIdeoBuildingPreceptOnly;
			string[] items = recipeDef.products.Select(delegate(ThingDefCountClass p)
			{
				if (p.count != 1)
				{
					return p.Label;
				}
				return Find.ActiveLanguageWorker.WithIndefiniteArticle(p.thingDef.label, false, false);
			}).ToArray<string>();
			recipeDef.description = "RecipeMakeDescription".Translate(items.ToCommaList(true, false));
			recipeDef.descriptionHyperlinks = (from p in recipeDef.products
			select new DefHyperlink(p.thingDef)).ToList<DefHyperlink>();
			if (adjustedCount != 1 && recipeDef.workAmount < 0f)
			{
				recipeDef.workAmount = recipeDef.WorkAmountTotal(null) * (float)adjustedCount;
			}
			return recipeDef;
		}

		// Token: 0x06009194 RID: 37268 RVA: 0x002ADA50 File Offset: 0x002ABC50
		public static void SetIngredients(RecipeDef r, ThingDef def, int adjustedCount = 1)
		{
			r.ingredients.Clear();
			r.adjustedCount = adjustedCount;
			if (def.MadeFromStuff)
			{
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount((float)(def.CostStuffCount * adjustedCount));
				ingredientCount.filter.SetAllowAllWhoCanMake(def);
				ThingFilter filter = ingredientCount.filter;
				string customSummary;
				if ((customSummary = def.stuffCategorySummary) == null)
				{
					customSummary = (from category in def.stuffCategories
					select category.noun).ToCommaListOr(false);
				}
				filter.customSummary = customSummary;
				r.ingredients.Add(ingredientCount);
				r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
				r.productHasIngredientStuff = true;
			}
			if (def.CostList != null)
			{
				foreach (ThingDefCountClass thingDefCountClass in def.CostList)
				{
					IngredientCount ingredientCount2 = new IngredientCount();
					ingredientCount2.SetBaseCount((float)(thingDefCountClass.count * adjustedCount));
					ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
					r.ingredients.Add(ingredientCount2);
				}
			}
		}

		// Token: 0x06009195 RID: 37269 RVA: 0x002ADB7C File Offset: 0x002ABD7C
		public static void ResetRecipeIngredientsForDifficulty()
		{
			foreach (RecipeDef recipeDef in from x in DefDatabase<RecipeDef>.AllDefs
			where x.regenerateOnDifficultyChange
			select x)
			{
				RecipeDefGenerator.SetIngredients(recipeDef, recipeDef.products[0].thingDef, recipeDef.adjustedCount);
			}
		}

		// Token: 0x06009196 RID: 37270 RVA: 0x002ADC04 File Offset: 0x002ABE04
		private static IEnumerable<RecipeDef> DrugAdministerDefs(bool hotReload = false)
		{
			foreach (ThingDef thingDef in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d)
			{
				string defName = "Administer_" + thingDef.defName;
				RecipeDef recipeDef = hotReload ? (DefDatabase<RecipeDef>.GetNamed(defName, false) ?? new RecipeDef()) : new RecipeDef();
				recipeDef.defName = defName;
				recipeDef.label = "RecipeAdminister".Translate(thingDef.label);
				recipeDef.jobString = "RecipeAdministerJobString".Translate(thingDef.label);
				recipeDef.workerClass = typeof(Recipe_AdministerIngestible);
				recipeDef.targetsBodyPart = false;
				recipeDef.anesthetize = false;
				recipeDef.surgerySuccessChanceFactor = 99999f;
				recipeDef.modContentPack = thingDef.modContentPack;
				recipeDef.workAmount = (float)thingDef.ingestible.baseIngestTicks;
				recipeDef.humanlikeOnly = thingDef.ingestible.humanlikeOnly;
				IngredientCount ingredientCount = new IngredientCount();
				ingredientCount.SetBaseCount(1f);
				ingredientCount.filter.SetAllow(thingDef, true);
				recipeDef.ingredients.Add(ingredientCount);
				recipeDef.fixedIngredientFilter.SetAllow(thingDef, true);
				recipeDef.recipeUsers = new List<ThingDef>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Pawn && d.race.IsFlesh))
				{
					recipeDef.recipeUsers.Add(item);
				}
				yield return recipeDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
