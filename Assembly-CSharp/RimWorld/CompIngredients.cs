using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompIngredients : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__17 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompIngredients _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompIngredients compIngredients = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compIngredients.parent.def.ingestible == null || !compIngredients.parent.def.ingestible.IsMeal)
				{
					break;
				}
				if (compIngredients.ingredients.Count > 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Ingredients".Translate(), compIngredients.GetIngredientsString(includeMergeCompatibility: false, out var _), "IngredientsDesc".Translate(), 1000, null, Dialog_InfoCard.DefsToHyperlinks(compIngredients.ingredients));
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00c0;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00c0;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00c0:
				if (ModsConfig.IdeologyActive)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "DietaryType".Translate(), compIngredients.GetFoodKindInspectString().StripTags(), "DietaryTypeDesc".Translate(), 995);
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public List<ThingDef> ingredients = new List<ThingDef>();

	private List<string> cachedMergeCompatibilityTags;

	public const int MaxNumIngredients = 3;

	private static readonly List<ThingDef> tmpMergedIngredients = new List<ThingDef>();

	private static readonly List<string> tmpMergedIngredientTags = new List<string>();

	private static readonly List<IngredientCount> missingIngredients = new List<IngredientCount>();

	private static readonly Func<ThingDef, ThingDef, int> MostTagsCmp = GenCollection.CompareBy((ThingDef a) => a.ingredient?.mergeCompatibilityTags.Count ?? 0).Descending().Compare;

	public CompProperties_Ingredients Props => (CompProperties_Ingredients)props;

	public List<string> MergeCompatibilityTags
	{
		get
		{
			if (cachedMergeCompatibilityTags == null)
			{
				cachedMergeCompatibilityTags = new List<string>();
				if (Props.performMergeCompatibilityChecks)
				{
					ComputeTags(cachedMergeCompatibilityTags, ingredients);
				}
			}
			return cachedMergeCompatibilityTags;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Collections.Look(ref ingredients, "ingredients", LookMode.Def);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (ingredients == null)
			{
				ingredients = new List<ThingDef>();
			}
			if (ingredients.RemoveAll((ThingDef i) => i == null) != 0)
			{
				Log.Error("Some ingredients were null after loading.");
			}
		}
	}

	public void RegisterIngredient(ThingDef def)
	{
		if (!ingredients.Contains(def))
		{
			ingredients.Add(def);
			cachedMergeCompatibilityTags = null;
		}
	}

	public override void PostSplitOff(Thing piece)
	{
		base.PostSplitOff(piece);
		if (piece != parent)
		{
			CompIngredients compIngredients = piece.TryGetComp<CompIngredients>();
			for (int i = 0; i < ingredients.Count; i++)
			{
				compIngredients.ingredients.Add(ingredients[i]);
			}
		}
	}

	public override bool AllowStackWith(Thing otherStack)
	{
		if (!otherStack.TryGetComp(out CompIngredients comp))
		{
			return false;
		}
		if (!Props.performMergeCompatibilityChecks || !comp.Props.performMergeCompatibilityChecks)
		{
			return true;
		}
		if (ModsConfig.BiotechActive && FoodUtility.GetFoodKind(parent) != FoodUtility.GetFoodKind(otherStack))
		{
			return false;
		}
		int count = MergeCompatibilityTags.Count;
		int count2 = comp.MergeCompatibilityTags.Count;
		if (count == 0 && count2 == 0)
		{
			return true;
		}
		if (count != count2 && (count == 0 || count2 == 0))
		{
			return false;
		}
		tmpMergedIngredients.Clear();
		tmpMergedIngredientTags.Clear();
		tmpMergedIngredients.AddRange(ingredients);
		MergeIngredients(tmpMergedIngredients, comp.ingredients, out var lostImportantIngredients, parent.def);
		if (lostImportantIngredients)
		{
			return false;
		}
		ComputeTags(tmpMergedIngredientTags, tmpMergedIngredients);
		return tmpMergedIngredientTags.SetsEqual(MergeCompatibilityTags) && tmpMergedIngredientTags.SetsEqual(comp.MergeCompatibilityTags);
	}

	public override void PreAbsorbStack(Thing otherStack, int count)
	{
		base.PreAbsorbStack(otherStack, count);
		CompIngredients compIngredients = otherStack.TryGetComp<CompIngredients>();
		MergeIngredients(ingredients, compIngredients.ingredients, out var _);
		cachedMergeCompatibilityTags = null;
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (ingredients.Count > 0)
		{
			stringBuilder.Append("Ingredients".Translate() + ": ");
			stringBuilder.Append(GetIngredientsString(includeMergeCompatibility: true, out var hasMergeCompatibilityIngredients));
			if (hasMergeCompatibilityIngredients)
			{
				stringBuilder.Append(" (* " + "OnlyStacksWithCompatibleMeals".Translate().Resolve() + ")");
			}
		}
		if (ModsConfig.IdeologyActive)
		{
			stringBuilder.AppendLineIfNotEmpty().Append(GetFoodKindInspectString());
		}
		return stringBuilder.ToString();
	}

	public string GetIngredientsString(bool includeMergeCompatibility, out bool hasMergeCompatibilityIngredients)
	{
		StringBuilder stringBuilder = new StringBuilder();
		hasMergeCompatibilityIngredients = false;
		for (int i = 0; i < ingredients.Count; i++)
		{
			ThingDef thingDef = ingredients[i];
			stringBuilder.Append((i == 0) ? thingDef.LabelCap.Resolve() : thingDef.label);
			if (includeMergeCompatibility && Props.performMergeCompatibilityChecks)
			{
				IngredientProperties ingredient = thingDef.ingredient;
				if (ingredient != null && ingredient.mergeCompatibilityTags.Count > 0)
				{
					stringBuilder.Append("*");
					hasMergeCompatibilityIngredients = true;
				}
			}
			if (i < ingredients.Count - 1)
			{
				stringBuilder.Append(", ");
			}
		}
		return stringBuilder.ToString();
	}

	private string GetFoodKindInspectString()
	{
		if (FoodUtility.GetFoodKind(parent) == FoodKind.NonMeat)
		{
			return "MealKindVegetarian".Translate().Colorize(Color.green);
		}
		if (FoodUtility.GetFoodKind(parent) == FoodKind.Meat)
		{
			return "MealKindMeat".Translate().Colorize(ColorLibrary.RedReadable);
		}
		return "MealKindAny".Translate().Colorize(Color.white);
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__17))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	private static void ComputeTags(List<string> result, List<ThingDef> ingredients)
	{
		foreach (ThingDef ingredient in ingredients)
		{
			if (ingredient.ingredient == null)
			{
				continue;
			}
			foreach (string mergeCompatibilityTag in ingredient.ingredient.mergeCompatibilityTags)
			{
				result.AddUnique(mergeCompatibilityTag);
			}
		}
	}

	private static void MergeIngredients(List<ThingDef> destIngredients, List<ThingDef> srcIngredients, out bool lostImportantIngredients, ThingDef defToMake = null)
	{
		lostImportantIngredients = false;
		foreach (ThingDef srcIngredient in srcIngredients)
		{
			destIngredients.AddUnique(srcIngredient);
		}
		if (destIngredients.Count > 3)
		{
			destIngredients.SortStable(MostTagsCmp);
			while (destIngredients.Count > 3)
			{
				List<string> list = destIngredients.Last().ingredient?.mergeCompatibilityTags;
				if (list != null && list.Any())
				{
					lostImportantIngredients = true;
				}
				destIngredients.RemoveLast();
			}
		}
		if (defToMake != null && !lostImportantIngredients)
		{
			RecipeDef recipeDef = DefDatabase<RecipeDef>.AllDefs.FirstOrDefault((RecipeDef x) => x.ProducedThingDef == defToMake);
			if (recipeDef == null)
			{
				return;
			}
			missingIngredients.AddRange(recipeDef.ingredients);
			foreach (ThingDef ing in destIngredients)
			{
				IngredientCount ingredientCount = missingIngredients.FirstOrDefault((IngredientCount x) => x.filter.Allows(ing));
				if (ingredientCount != null)
				{
					missingIngredients.Remove(ingredientCount);
					if (!missingIngredients.Any())
					{
						break;
					}
				}
			}
			if (missingIngredients.Any())
			{
				lostImportantIngredients = true;
			}
		}
		missingIngredients.Clear();
	}
}
