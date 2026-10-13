using System;

namespace RimWorld
{
	// Token: 0x0200361D RID: 13853
	[DefOf]
	public static class PlanetLayerSettingsDefOf
	{
		// Token: 0x06013408 RID: 78856 RVA: 0x005B6313 File Offset: 0x005B4513
		static PlanetLayerSettingsDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PlanetLayerSettingsDefOf));
		}

		// Token: 0x0400D572 RID: 54642
		public static PlanetLayerSettingsDef Surface;

		// Token: 0x0400D573 RID: 54643
		[MayRequireOdyssey]
		public static PlanetLayerSettingsDef Orbit;
	}
}
