using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003796 RID: 14230
	public struct RoyalTitleFoodRequirement
	{
		// Token: 0x17003261 RID: 12897
		// (get) Token: 0x06013B4B RID: 80715 RVA: 0x005DADCA File Offset: 0x005D8FCA
		public bool Defined
		{
			get
			{
				return this.minQuality > FoodPreferability.Undefined;
			}
		}

		// Token: 0x06013B4C RID: 80716 RVA: 0x005DADD8 File Offset: 0x005D8FD8
		public bool Acceptable(ThingDef food)
		{
			return food.ingestible != null && (this.allowedDefs.Contains(food) || (this.allowedTypes != FoodTypeFlags.None && (this.allowedTypes & food.ingestible.foodType) != FoodTypeFlags.None) || food.ingestible.preferability >= this.minQuality);
		}

		// Token: 0x0400DA46 RID: 55878
		public FoodPreferability minQuality;

		// Token: 0x0400DA47 RID: 55879
		public FoodTypeFlags allowedTypes;

		// Token: 0x0400DA48 RID: 55880
		public List<ThingDef> allowedDefs;
	}
}
