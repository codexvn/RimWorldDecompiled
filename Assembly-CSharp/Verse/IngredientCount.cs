using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C8A RID: 3210
	public sealed class IngredientCount : IExposable
	{
		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x001AA5DB File Offset: 0x001A87DB
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x0600526F RID: 21103 RVA: 0x001AA5EB File Offset: 0x001A87EB
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + ((this != null) ? this.ToString() : null));
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06005270 RID: 21104 RVA: 0x001AA61C File Offset: 0x001A881C
		public string Summary
		{
			get
			{
				return this.count.ToString() + "x " + this.filter.Summary;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06005271 RID: 21105 RVA: 0x001AA63E File Offset: 0x001A883E
		public string SummaryFilterFirst
		{
			get
			{
				return this.filter.Summary.CapitalizeFirst() + " x" + this.count.ToString();
			}
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x001AA668 File Offset: 0x001A8868
		public string SummaryFor(RecipeDef recipe)
		{
			return this.CountFor(recipe).ToString() + "x " + this.filter.Summary;
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x001AA69C File Offset: 0x001A889C
		public float CountFor(RecipeDef recipe)
		{
			float num = this.GetBaseCount();
			ThingDef thingDef = this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x) && !x.smallVolume) ?? this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x));
			if (thingDef != null)
			{
				float num2 = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
				if (Math.Abs(num2) > 1E-45f)
				{
					num /= num2;
				}
			}
			return num;
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x001AA724 File Offset: 0x001A8924
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe, Bill bill = null)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(((bill == null) ? this.count : recipe.Worker.GetIngredientCount(this, bill)) / num);
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001AA75D File Offset: 0x001A895D
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001AA765 File Offset: 0x001A8965
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001AA76E File Offset: 0x001A896E
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001AA77B File Offset: 0x001A897B
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x06005279 RID: 21113 RVA: 0x001AA792 File Offset: 0x001A8992
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.count, "count", 0f, false);
		}

		// Token: 0x04003B07 RID: 15111
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04003B08 RID: 15112
		private float count = 1f;
	}
}
