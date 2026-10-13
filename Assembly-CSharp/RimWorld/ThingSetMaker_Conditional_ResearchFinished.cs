using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B5F RID: 11103
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		// Token: 0x0600F844 RID: 63556 RVA: 0x004834BB File Offset: 0x004816BB
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		// Token: 0x0400A340 RID: 41792
		public ResearchProjectDef researchProject;
	}
}
