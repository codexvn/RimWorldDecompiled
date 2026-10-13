using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001A64 RID: 6756
	public class ComplexLayoutDef : StructureLayoutDef
	{
		// Token: 0x04006526 RID: 25894
		public List<ComplexThreat> threats;

		// Token: 0x04006527 RID: 25895
		public float roomRewardCrateFactor = 0.5f;

		// Token: 0x04006528 RID: 25896
		public float fixedHostileFactionChance = 0.25f;

		// Token: 0x04006529 RID: 25897
		public ThingSetMakerDef rewardThingSetMakerDef;
	}
}
