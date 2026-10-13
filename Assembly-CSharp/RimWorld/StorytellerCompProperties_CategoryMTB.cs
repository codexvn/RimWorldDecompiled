using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E32 RID: 7730
	public class StorytellerCompProperties_CategoryMTB : StorytellerCompProperties
	{
		// Token: 0x0600AC1C RID: 44060 RVA: 0x00313747 File Offset: 0x00311947
		public StorytellerCompProperties_CategoryMTB()
		{
			this.compClass = typeof(StorytellerComp_CategoryMTB);
		}

		// Token: 0x04007657 RID: 30295
		public float mtbDays = -1f;

		// Token: 0x04007658 RID: 30296
		public SimpleCurve mtbDaysFactorByDaysPassedCurve;

		// Token: 0x04007659 RID: 30297
		public IncidentCategoryDef category;
	}
}
