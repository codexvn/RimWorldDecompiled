using System;

namespace Verse
{
	// Token: 0x020002E8 RID: 744
	public class StyleCategoryPair : IExposable
	{
		// Token: 0x06001245 RID: 4677 RVA: 0x000551EF File Offset: 0x000533EF
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
		}

		// Token: 0x04001087 RID: 4231
		public StyleCategoryDef category;

		// Token: 0x04001088 RID: 4232
		public ThingStyleDef styleDef;
	}
}
