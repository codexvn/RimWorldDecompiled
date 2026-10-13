using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305D RID: 12381
	public class Alert_Analyzable_GrayFlesh : Alert_Analyzable
	{
		// Token: 0x17002CFA RID: 11514
		// (get) Token: 0x0601162F RID: 71215 RVA: 0x004FCE4F File Offset: 0x004FB04F
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.GrayFleshSample;
			}
		}

		// Token: 0x06011630 RID: 71216 RVA: 0x004FCE56 File Offset: 0x004FB056
		public Alert_Analyzable_GrayFlesh()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertGrayFleshSample".Translate();
			this.defaultExplanation = "AlertGrayFleshSampleDesc".Translate();
		}
	}
}
