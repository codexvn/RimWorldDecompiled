using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CEB RID: 11499
	public class CompIngredients : ThingComp
	{
		// Token: 0x170028ED RID: 10477
		// (get) Token: 0x0601027D RID: 66173 RVA: 0x004ACAD6 File Offset: 0x004AACD6
		public CompProperties_Ingredients Props
		{
			get
			{
				return (CompProperties_Ingredients)this.props;
			}
		}

		// Token: 0x170028EE RID: 10478
		// (get) Token: 0x0601027E RID: 66174 RVA: 0x004ACAE3 File Offset: 0x004AACE3
		public List<string> MergeCompatibilityTags
		{
			get
			{
				if (this.cachedMergeCompatibilityTags == null)
				{
					this.cachedMergeCompatibilityTags = new List<string>();
					if (this.Props.performMergeCompatibilityChecks)
					{
						CompIngredients.ComputeTags(this.cachedMergeCompatibilityTags, this.ingredients);
					}
				}
				return this.cachedMergeCompatibilityTags;
			}
		}

		// Token: 0x0601027F RID: 66175 RVA: 0x004ACB1C File Offset: 0x004AAD1C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.ingredients, "ingredients", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ingredients == null)
				{
					this.ingredients = new List<ThingDef>();
				}
				if (this.ingredients.RemoveAll((ThingDef i) => i == null) != 0)
				{
					Log.Error("Some ingredients were null after loading.");
				}
			}
		}

		// Token: 0x06010280 RID: 66176 RVA: 0x004ACB96 File Offset: 0x004AAD96
		public void RegisterIngredient(ThingDef def)
		{
			if (!this.ingredients.Contains(def))
			{
				this.ingredients.Add(def);
				this.cachedMergeCompatibilityTags = null;
			}
		}

		// Token: 0x06010281 RID: 66177 RVA: 0x004ACBBC File Offset: 0x004AADBC
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (piece != this.parent)
			{
				CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					compIngredients.ingredients.Add(this.ingredients[i]);
				}
			}
		}

		// Token: 0x06010282 RID: 66178 RVA: 0x004ACC10 File Offset: 0x004AAE10
		public override bool AllowStackWith(Thing otherStack)
		{
			CompIngredients compIngredients;
			if (!otherStack.TryGetComp(out compIngredients))
			{
				return false;
			}
			if (!this.Props.performMergeCompatibilityChecks || !compIngredients.Props.performMergeCompatibilityChecks)
			{
				return true;
			}
			if (ModsConfig.BiotechActive && FoodUtility.GetFoodKind(this.parent) != FoodUtility.GetFoodKind(otherStack))
			{
				return false;
			}
			int count = this.MergeCompatibilityTags.Count;
			int count2 = compIngredients.MergeCompatibilityTags.Count;
			bool result;
			if (count == 0 && count2 == 0)
			{
				result = true;
			}
			else if (count != count2 && (count == 0 || count2 == 0))
			{
				result = false;
			}
			else
			{
				CompIngredients.tmpMergedIngredients.Clear();
				CompIngredients.tmpMergedIngredientTags.Clear();
				CompIngredients.tmpMergedIngredients.AddRange(this.ingredients);
				bool flag;
				CompIngredients.MergeIngredients(CompIngredients.tmpMergedIngredients, compIngredients.ingredients, out flag, this.parent.def);
				if (flag)
				{
					result = false;
				}
				else
				{
					CompIngredients.ComputeTags(CompIngredients.tmpMergedIngredientTags, CompIngredients.tmpMergedIngredients);
					result = (CompIngredients.tmpMergedIngredientTags.SetsEqual(this.MergeCompatibilityTags) && CompIngredients.tmpMergedIngredientTags.SetsEqual(compIngredients.MergeCompatibilityTags));
				}
			}
			return result;
		}

		// Token: 0x06010283 RID: 66179 RVA: 0x004ACD14 File Offset: 0x004AAF14
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
			bool flag;
			CompIngredients.MergeIngredients(this.ingredients, compIngredients.ingredients, out flag, null);
			this.cachedMergeCompatibilityTags = null;
		}

		// Token: 0x06010284 RID: 66180 RVA: 0x004ACD4C File Offset: 0x004AAF4C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ingredients.Count > 0)
			{
				stringBuilder.Append("Ingredients".Translate() + ": ");
				bool flag;
				stringBuilder.Append(this.GetIngredientsString(true, out flag));
				if (flag)
				{
					stringBuilder.Append(" (* " + "OnlyStacksWithCompatibleMeals".Translate().Resolve() + ")");
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				stringBuilder.AppendLineIfNotEmpty().Append(this.GetFoodKindInspectString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010285 RID: 66181 RVA: 0x004ACDE8 File Offset: 0x004AAFE8
		public string GetIngredientsString(bool includeMergeCompatibility, out bool hasMergeCompatibilityIngredients)
		{
			StringBuilder stringBuilder = new StringBuilder();
			hasMergeCompatibilityIngredients = false;
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				ThingDef thingDef = this.ingredients[i];
				stringBuilder.Append((i == 0) ? thingDef.LabelCap.Resolve() : thingDef.label);
				if (includeMergeCompatibility && this.Props.performMergeCompatibilityChecks)
				{
					IngredientProperties ingredient = thingDef.ingredient;
					if (ingredient != null && ingredient.mergeCompatibilityTags.Count > 0)
					{
						stringBuilder.Append("*");
						hasMergeCompatibilityIngredients = true;
					}
				}
				if (i < this.ingredients.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010286 RID: 66182 RVA: 0x004ACEA4 File Offset: 0x004AB0A4
		private string GetFoodKindInspectString()
		{
			if (FoodUtility.GetFoodKind(this.parent) == FoodKind.NonMeat)
			{
				return "MealKindVegetarian".Translate().Colorize(Color.green);
			}
			if (FoodUtility.GetFoodKind(this.parent) == FoodKind.Meat)
			{
				return "MealKindMeat".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return "MealKindAny".Translate().Colorize(Color.white);
		}

		// Token: 0x06010287 RID: 66183 RVA: 0x004ACF0A File Offset: 0x004AB10A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.parent.def.ingestible != null && this.parent.def.ingestible.IsMeal)
			{
				if (this.ingredients.Count > 0)
				{
					bool flag;
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Ingredients".Translate(), this.GetIngredientsString(false, out flag), "IngredientsDesc".Translate(), 1000, null, Dialog_InfoCard.DefsToHyperlinks(this.ingredients), false, false);
				}
				if (ModsConfig.IdeologyActive)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "DietaryType".Translate(), this.GetFoodKindInspectString().StripTags(), "DietaryTypeDesc".Translate(), 995, null, null, false, false);
				}
			}
			yield break;
		}

		// Token: 0x06010288 RID: 66184 RVA: 0x004ACF1C File Offset: 0x004AB11C
		private static void ComputeTags(List<string> result, List<ThingDef> ingredients)
		{
			foreach (ThingDef thingDef in ingredients)
			{
				if (thingDef.ingredient != null)
				{
					foreach (string obj in thingDef.ingredient.mergeCompatibilityTags)
					{
						result.AddUnique(obj);
					}
				}
			}
		}

		// Token: 0x06010289 RID: 66185 RVA: 0x004ACFB4 File Offset: 0x004AB1B4
		private static void MergeIngredients(List<ThingDef> destIngredients, List<ThingDef> srcIngredients, out bool lostImportantIngredients, ThingDef defToMake = null)
		{
			lostImportantIngredients = false;
			foreach (ThingDef obj in srcIngredients)
			{
				destIngredients.AddUnique(obj);
			}
			if (destIngredients.Count > 3)
			{
				destIngredients.SortStable(CompIngredients.MostTagsCmp);
				while (destIngredients.Count > 3)
				{
					IngredientProperties ingredient = destIngredients.Last<ThingDef>().ingredient;
					List<string> list = (ingredient != null) ? ingredient.mergeCompatibilityTags : null;
					if (list != null && list.Any<string>())
					{
						lostImportantIngredients = true;
					}
					destIngredients.RemoveLast<ThingDef>();
				}
			}
			if (defToMake != null && !lostImportantIngredients)
			{
				RecipeDef recipeDef = DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef x) => x.ProducedThingDef == defToMake);
				if (recipeDef == null)
				{
					return;
				}
				CompIngredients.missingIngredients.AddRange(recipeDef.ingredients);
				using (List<ThingDef>.Enumerator enumerator = destIngredients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef ing = enumerator.Current;
						IngredientCount ingredientCount = CompIngredients.missingIngredients.FirstOrDefault((IngredientCount x) => x.filter.Allows(ing));
						if (ingredientCount != null)
						{
							CompIngredients.missingIngredients.Remove(ingredientCount);
							if (!CompIngredients.missingIngredients.Any<IngredientCount>())
							{
								break;
							}
						}
					}
				}
				if (CompIngredients.missingIngredients.Any<IngredientCount>())
				{
					lostImportantIngredients = true;
				}
			}
			CompIngredients.missingIngredients.Clear();
		}

		// Token: 0x0400A8C4 RID: 43204
		public List<ThingDef> ingredients = new List<ThingDef>();

		// Token: 0x0400A8C5 RID: 43205
		private List<string> cachedMergeCompatibilityTags;

		// Token: 0x0400A8C6 RID: 43206
		public const int MaxNumIngredients = 3;

		// Token: 0x0400A8C7 RID: 43207
		private static readonly List<ThingDef> tmpMergedIngredients = new List<ThingDef>();

		// Token: 0x0400A8C8 RID: 43208
		private static readonly List<string> tmpMergedIngredientTags = new List<string>();

		// Token: 0x0400A8C9 RID: 43209
		private static readonly List<IngredientCount> missingIngredients = new List<IngredientCount>();

		// Token: 0x0400A8CA RID: 43210
		private static readonly Func<ThingDef, ThingDef, int> MostTagsCmp = new Func<ThingDef, ThingDef, int>(GenCollection.CompareBy<ThingDef, int>(new Func<ThingDef, int>(CompIngredients.<>c.<>9, ldftn(<.cctor>b__23_0))).Descending<ThingDef>().Compare);
	}
}
