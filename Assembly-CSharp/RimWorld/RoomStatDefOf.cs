using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035B5 RID: 13749
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x060133A3 RID: 78755 RVA: 0x005B5C5E File Offset: 0x005B3E5E
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}

		// Token: 0x0400D121 RID: 53537
		public static RoomStatDef Cleanliness;

		// Token: 0x0400D122 RID: 53538
		public static RoomStatDef Wealth;

		// Token: 0x0400D123 RID: 53539
		public static RoomStatDef Space;

		// Token: 0x0400D124 RID: 53540
		public static RoomStatDef Beauty;

		// Token: 0x0400D125 RID: 53541
		public static RoomStatDef Impressiveness;

		// Token: 0x0400D126 RID: 53542
		public static RoomStatDef ReadingBonus;

		// Token: 0x0400D127 RID: 53543
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x0400D128 RID: 53544
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x0400D129 RID: 53545
		public static RoomStatDef FoodPoisonChance;
	}
}
