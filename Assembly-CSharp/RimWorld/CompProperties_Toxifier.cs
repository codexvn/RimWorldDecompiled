using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E9B RID: 11931
	public class CompProperties_Toxifier : CompProperties
	{
		// Token: 0x06010CE0 RID: 68832 RVA: 0x004D64D3 File Offset: 0x004D46D3
		public CompProperties_Toxifier()
		{
			this.compClass = typeof(CompToxifier);
		}

		// Token: 0x06010CE1 RID: 68833 RVA: 0x004D6508 File Offset: 0x004D4708
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.pollutionIntervalTicks <= 0)
			{
				yield return "Pollution interval ticks must be greater than zero.";
			}
			if (this.cellsToPollute <= 0)
			{
				yield return "Cells to pollute must be greater than zero.";
			}
			if (this.radius <= 0f)
			{
				yield return "Radius to pollute must be greater than zero.";
			}
			yield break;
		}

		// Token: 0x0400AEF3 RID: 44787
		public float radius = 5.9f;

		// Token: 0x0400AEF4 RID: 44788
		public int pollutionIntervalTicks = 60000;

		// Token: 0x0400AEF5 RID: 44789
		public int cellsToPollute = 6;
	}
}
