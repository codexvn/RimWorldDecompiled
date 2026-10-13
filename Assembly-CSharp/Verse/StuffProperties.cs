using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000263 RID: 611
	public class StuffProperties
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0004948C File Offset: 0x0004768C
		public ThingDef SourceNaturalRock
		{
			get
			{
				if (!this.sourceNaturalRockCached)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					List<RecipeDef> allDefsListForReading2 = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].category == ThingCategory.Building && allDefsListForReading[i].building.isNaturalRock && allDefsListForReading[i].building.mineableThing != null && !allDefsListForReading[i].IsSmoothed)
						{
							if (allDefsListForReading[i].building.mineableThing == this.parent)
							{
								this.sourceNaturalRockCachedValue = allDefsListForReading[i];
								break;
							}
							for (int j = 0; j < allDefsListForReading2.Count; j++)
							{
								if (allDefsListForReading2[j].IsIngredient(allDefsListForReading[i].building.mineableThing))
								{
									bool flag = false;
									for (int k = 0; k < allDefsListForReading2[j].products.Count; k++)
									{
										if (allDefsListForReading2[j].products[k].thingDef == this.parent)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										this.sourceNaturalRockCachedValue = allDefsListForReading[i];
										break;
									}
								}
							}
						}
					}
					this.sourceNaturalRockCached = true;
				}
				return this.sourceNaturalRockCachedValue;
			}
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000495DC File Offset: 0x000477DC
		public bool CanMake(BuildableDef t)
		{
			if (!t.MadeFromStuff)
			{
				return false;
			}
			for (int i = 0; i < t.stuffCategories.Count; i++)
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					if (t.stuffCategories[i] == this.categories[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0004963C File Offset: 0x0004783C
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x04000B65 RID: 2917
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04000B66 RID: 2918
		[MustTranslate]
		public string stuffAdjective;

		// Token: 0x04000B67 RID: 2919
		public float commonality = 1f;

		// Token: 0x04000B68 RID: 2920
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x04000B69 RID: 2921
		public bool allowedInStuffGeneration = true;

		// Token: 0x04000B6A RID: 2922
		public List<StatModifier> statOffsets;

		// Token: 0x04000B6B RID: 2923
		public List<StatModifier> statFactors;

		// Token: 0x04000B6C RID: 2924
		public List<StatModifierQuality> statOffsetsQuality;

		// Token: 0x04000B6D RID: 2925
		public List<StatModifierQuality> statFactorsQuality;

		// Token: 0x04000B6E RID: 2926
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x04000B6F RID: 2927
		public EffecterDef constructEffect;

		// Token: 0x04000B70 RID: 2928
		public StuffAppearanceDef appearance;

		// Token: 0x04000B71 RID: 2929
		public bool allowColorGenerators;

		// Token: 0x04000B72 RID: 2930
		public bool canSuggestUseDefaultStuff;

		// Token: 0x04000B73 RID: 2931
		public bool isAirtight;

		// Token: 0x04000B74 RID: 2932
		public SoundDef soundImpactBullet;

		// Token: 0x04000B75 RID: 2933
		public SoundDef soundImpactMelee;

		// Token: 0x04000B76 RID: 2934
		public SoundDef soundMeleeHitSharp;

		// Token: 0x04000B77 RID: 2935
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x04000B78 RID: 2936
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x04000B79 RID: 2937
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
