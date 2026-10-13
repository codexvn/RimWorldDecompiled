using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001955 RID: 6485
	public class CompProperties_CausesGameCondition : CompProperties
	{
		// Token: 0x0600936D RID: 37741 RVA: 0x002B5B59 File Offset: 0x002B3D59
		public CompProperties_CausesGameCondition()
		{
			this.compClass = typeof(CompCauseGameCondition);
		}

		// Token: 0x04005EDA RID: 24282
		public GameConditionDef conditionDef;

		// Token: 0x04005EDB RID: 24283
		public int worldRange;

		// Token: 0x04005EDC RID: 24284
		public bool hideSource;

		// Token: 0x04005EDD RID: 24285
		public bool preventConditionStacking = true;
	}
}
