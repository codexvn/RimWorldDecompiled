using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACF RID: 6863
	public class FactionCurve
	{
		// Token: 0x060099A1 RID: 39329 RVA: 0x002CBD99 File Offset: 0x002C9F99
		public float Evaluate(float x)
		{
			return this.selectionWeightPerPointsCurve.Evaluate(x);
		}

		// Token: 0x0400691D RID: 26909
		public FactionDef faction;

		// Token: 0x0400691E RID: 26910
		private SimpleCurve selectionWeightPerPointsCurve;
	}
}
