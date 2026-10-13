using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200361C RID: 13852
	[DefOf]
	public static class MechWeightClassDefOf
	{
		// Token: 0x06013407 RID: 78855 RVA: 0x005B6302 File Offset: 0x005B4502
		static MechWeightClassDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MechWeightClassDefOf));
		}

		// Token: 0x0400D56E RID: 54638
		public static MechWeightClassDef Light;

		// Token: 0x0400D56F RID: 54639
		public static MechWeightClassDef Medium;

		// Token: 0x0400D570 RID: 54640
		public static MechWeightClassDef Heavy;

		// Token: 0x0400D571 RID: 54641
		public static MechWeightClassDef UltraHeavy;
	}
}
