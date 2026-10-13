using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000259 RID: 601
	public class RecipeMakerProperties
	{
		// Token: 0x04000B1F RID: 2847
		[MustTranslate]
		public string label;

		// Token: 0x04000B20 RID: 2848
		public int productCount = 1;

		// Token: 0x04000B21 RID: 2849
		public int targetCountAdjustment = 1;

		// Token: 0x04000B22 RID: 2850
		public int bulkRecipeCount = -1;

		// Token: 0x04000B23 RID: 2851
		public bool useIngredientsForColor = true;

		// Token: 0x04000B24 RID: 2852
		public int workAmount = -1;

		// Token: 0x04000B25 RID: 2853
		public StatDef workSpeedStat;

		// Token: 0x04000B26 RID: 2854
		public StatDef efficiencyStat;

		// Token: 0x04000B27 RID: 2855
		public ThingDef unfinishedThingDef;

		// Token: 0x04000B28 RID: 2856
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000B29 RID: 2857
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000B2A RID: 2858
		public SkillDef workSkill;

		// Token: 0x04000B2B RID: 2859
		public float workSkillLearnPerTick = 1f;

		// Token: 0x04000B2C RID: 2860
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x04000B2D RID: 2861
		public EffecterDef effectWorking;

		// Token: 0x04000B2E RID: 2862
		public SoundDef soundWorking;

		// Token: 0x04000B2F RID: 2863
		public List<ThingDef> recipeUsers;

		// Token: 0x04000B30 RID: 2864
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x04000B31 RID: 2865
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x04000B32 RID: 2866
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000B33 RID: 2867
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000B34 RID: 2868
		public bool mechanitorOnlyRecipe;

		// Token: 0x04000B35 RID: 2869
		public bool fromIdeoBuildingPreceptOnly;

		// Token: 0x04000B36 RID: 2870
		public int displayPriority = 99999;
	}
}
