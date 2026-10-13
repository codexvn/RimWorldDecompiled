using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D97 RID: 11671
	public class CompProperties_OxygenPusher : CompProperties
	{
		// Token: 0x0601069D RID: 67229 RVA: 0x004BC80B File Offset: 0x004BAA0B
		public CompProperties_OxygenPusher()
		{
			this.compClass = typeof(CompOxygenPusher);
		}

		// Token: 0x0400AB3C RID: 43836
		public bool requiresPower = true;

		// Token: 0x0400AB3D RID: 43837
		public float airPerSecondPerHundredCells = 0.1f;
	}
}
