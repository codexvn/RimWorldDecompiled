using System;

namespace RimWorld
{
	// Token: 0x0200361E RID: 13854
	[DefOf]
	public static class PlanetLayerDefOf
	{
		// Token: 0x06013409 RID: 78857 RVA: 0x005B6324 File Offset: 0x005B4524
		static PlanetLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PlanetLayerDefOf));
		}

		// Token: 0x0400D574 RID: 54644
		public static PlanetLayerDef Surface;

		// Token: 0x0400D575 RID: 54645
		[MayRequireOdyssey]
		public static PlanetLayerDef Orbit;
	}
}
