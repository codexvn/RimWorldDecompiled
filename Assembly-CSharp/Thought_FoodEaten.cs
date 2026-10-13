using System;
using RimWorld;
using Verse;

// Token: 0x0200003E RID: 62
public class Thought_FoodEaten : Thought_Memory
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600014C RID: 332 RVA: 0x00009243 File Offset: 0x00007443
	public override string Description
	{
		get
		{
			return base.Description + "\n\n" + this.foodThoughtDescription;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000925C File Offset: 0x0000745C
	public void SetFood(Thing food)
	{
		CompIngredients compIngredients = food.TryGetComp<CompIngredients>();
		this.foodThoughtDescription = "ThoughtFoodEatenFood".Translate() + ": " + food.def.LabelCap;
		if (compIngredients != null && compIngredients.ingredients.Count > 0)
		{
			bool flag;
			this.foodThoughtDescription += " (" + "ThoughtFoodEatenIngredients".Translate() + ": " + compIngredients.GetIngredientsString(false, out flag) + ")";
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000092FC File Offset: 0x000074FC
	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look<string>(ref this.foodThoughtDescription, "foodThoughtDescription", null, false);
	}

	// Token: 0x0400009B RID: 155
	private string foodThoughtDescription;
}
