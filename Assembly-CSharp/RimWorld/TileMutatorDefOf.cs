using System;

namespace RimWorld
{
	// Token: 0x0200361B RID: 13851
	[DefOf]
	public static class TileMutatorDefOf
	{
		// Token: 0x06013406 RID: 78854 RVA: 0x005B62F1 File Offset: 0x005B44F1
		static TileMutatorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TileMutatorDefOf));
		}

		// Token: 0x0400D561 RID: 54625
		public static TileMutatorDef Mountain;

		// Token: 0x0400D562 RID: 54626
		public static TileMutatorDef Coast;

		// Token: 0x0400D563 RID: 54627
		public static TileMutatorDef River;

		// Token: 0x0400D564 RID: 54628
		public static TileMutatorDef Caves;

		// Token: 0x0400D565 RID: 54629
		[MayRequireOdyssey]
		public static TileMutatorDef Lakeshore;

		// Token: 0x0400D566 RID: 54630
		[MayRequireOdyssey]
		public static TileMutatorDef RiverConfluence;

		// Token: 0x0400D567 RID: 54631
		[MayRequireOdyssey]
		public static TileMutatorDef Headwater;

		// Token: 0x0400D568 RID: 54632
		[MayRequireOdyssey]
		public static TileMutatorDef RiverIsland;

		// Token: 0x0400D569 RID: 54633
		[MayRequireOdyssey]
		public static TileMutatorDef RiverDelta;

		// Token: 0x0400D56A RID: 54634
		[MayRequireOdyssey]
		public static TileMutatorDef Fjord;

		// Token: 0x0400D56B RID: 54635
		[MayRequireOdyssey]
		public static TileMutatorDef AnimalHabitat;

		// Token: 0x0400D56C RID: 54636
		[MayRequireOdyssey]
		public static TileMutatorDef Crevasse;

		// Token: 0x0400D56D RID: 54637
		[MayRequireOdyssey]
		public static TileMutatorDef MixedBiome;
	}
}
