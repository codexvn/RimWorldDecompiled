using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035C0 RID: 13760
	[DefOf]
	public static class WeatherDefOf
	{
		// Token: 0x060133AE RID: 78766 RVA: 0x005B5D19 File Offset: 0x005B3F19
		static WeatherDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WeatherDefOf));
		}

		// Token: 0x0400D284 RID: 53892
		public static WeatherDef Clear;

		// Token: 0x0400D285 RID: 53893
		public static WeatherDef Fog;

		// Token: 0x0400D286 RID: 53894
		public static WeatherDef FoggyRain;

		// Token: 0x0400D287 RID: 53895
		[MayRequireAnomaly]
		public static WeatherDef GrayPall;

		// Token: 0x0400D288 RID: 53896
		[MayRequireAnomaly]
		public static WeatherDef UnnaturalDarkness_Stage1;

		// Token: 0x0400D289 RID: 53897
		[MayRequireAnomaly]
		public static WeatherDef UnnaturalDarkness_Stage2;
	}
}
