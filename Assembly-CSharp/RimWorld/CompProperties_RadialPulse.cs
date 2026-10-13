using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DD4 RID: 11732
	public class CompProperties_RadialPulse : CompProperties
	{
		// Token: 0x060107FB RID: 67579 RVA: 0x004C2303 File Offset: 0x004C0503
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		// Token: 0x0400AC10 RID: 44048
		public int ticksBetweenPulses = 300;

		// Token: 0x0400AC11 RID: 44049
		public int ticksPerPulse = 60;

		// Token: 0x0400AC12 RID: 44050
		public Color color;

		// Token: 0x0400AC13 RID: 44051
		public float radius;
	}
}
