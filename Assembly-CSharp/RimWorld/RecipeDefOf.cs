using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003599 RID: 13721
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x06013387 RID: 78727 RVA: 0x005B5A82 File Offset: 0x005B3C82
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x0400CE5E RID: 52830
		public static RecipeDef RemoveBodyPart;

		// Token: 0x0400CE5F RID: 52831
		public static RecipeDef InstallPegLeg;

		// Token: 0x0400CE60 RID: 52832
		public static RecipeDef SmeltOrDestroyThing;

		// Token: 0x0400CE61 RID: 52833
		public static RecipeDef Sterilize;

		// Token: 0x0400CE62 RID: 52834
		[MayRequireBiotech]
		public static RecipeDef ImplantXenogerm;

		// Token: 0x0400CE63 RID: 52835
		[MayRequireBiotech]
		public static RecipeDef ImplantEmbryo;

		// Token: 0x0400CE64 RID: 52836
		[MayRequireBiotech]
		public static RecipeDef ExtractHemogenPack;
	}
}
