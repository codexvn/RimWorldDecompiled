using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C9A RID: 11418
	public class CompProperties_FleshmassNucleus : CompProperties
	{
		// Token: 0x0601005F RID: 65631 RVA: 0x004A3F68 File Offset: 0x004A2168
		public CompProperties_FleshmassNucleus()
		{
			this.compClass = typeof(CompFleshmassNucleus);
		}

		// Token: 0x0400A793 RID: 42899
		public float activityOnRoofCollapsed = 0.3f;

		// Token: 0x0400A794 RID: 42900
		public SimpleCurve activityMeatPerDayCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 20f),
				true
			},
			{
				new CurvePoint(0.9f, 120f),
				true
			}
		};
	}
}
