using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DB2 RID: 15794
	public static class MapParentTuning
	{
		// Token: 0x0400EF6F RID: 61295
		public const float ShortDetectionCountdownDays = 4f;

		// Token: 0x0400EF70 RID: 61296
		public const float DefaultDetectionCountdownDays = 4f;

		// Token: 0x0400EF71 RID: 61297
		public static readonly FloatRange SiteDetectionCountdownMultiplier = new FloatRange(0.85f, 1.15f);
	}
}
