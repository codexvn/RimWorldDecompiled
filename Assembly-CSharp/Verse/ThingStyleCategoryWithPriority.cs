using System;

namespace Verse
{
	// Token: 0x020002E7 RID: 743
	public class ThingStyleCategoryWithPriority : IExposable
	{
		// Token: 0x06001242 RID: 4674 RVA: 0x000024DF File Offset: 0x000006DF
		public ThingStyleCategoryWithPriority()
		{
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000551B1 File Offset: 0x000533B1
		public ThingStyleCategoryWithPriority(StyleCategoryDef category, float priority)
		{
			this.category = category;
			this.priority = priority;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000551C7 File Offset: 0x000533C7
		public void ExposeData()
		{
			Scribe_Defs.Look<StyleCategoryDef>(ref this.category, "category");
			Scribe_Values.Look<float>(ref this.priority, "priority", 0f, false);
		}

		// Token: 0x04001085 RID: 4229
		public StyleCategoryDef category;

		// Token: 0x04001086 RID: 4230
		public float priority;
	}
}
