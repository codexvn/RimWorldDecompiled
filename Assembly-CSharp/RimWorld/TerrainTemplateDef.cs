using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190E RID: 6414
	public class TerrainTemplateDef : Def
	{
		// Token: 0x04005CE0 RID: 23776
		[NoTranslate]
		public string texturePath;

		// Token: 0x04005CE1 RID: 23777
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04005CE2 RID: 23778
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04005CE3 RID: 23779
		public TerrainDef burnedDef;

		// Token: 0x04005CE4 RID: 23780
		public List<ThingDefCountClass> costList;

		// Token: 0x04005CE5 RID: 23781
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x04005CE6 RID: 23782
		public List<StatModifier> statBases;

		// Token: 0x04005CE7 RID: 23783
		public int uiOrder;

		// Token: 0x04005CE8 RID: 23784
		public int renderPrecedenceStart;

		// Token: 0x04005CE9 RID: 23785
		public int constructionSkillPrerequisite;

		// Token: 0x04005CEA RID: 23786
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x04005CEB RID: 23787
		public StyleCategoryDef dominantStyleCategory;
	}
}
