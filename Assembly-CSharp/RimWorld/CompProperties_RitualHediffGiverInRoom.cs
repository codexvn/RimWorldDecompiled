using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003865 RID: 14437
	public class CompProperties_RitualHediffGiverInRoom : CompProperties
	{
		// Token: 0x06013F58 RID: 81752 RVA: 0x005EBFBE File Offset: 0x005EA1BE
		public CompProperties_RitualHediffGiverInRoom()
		{
			this.compClass = typeof(CompRitualHediffGiverInRoom);
		}

		// Token: 0x0400DC63 RID: 56419
		public HediffDef hediff;

		// Token: 0x0400DC64 RID: 56420
		public float minRadius = 999f;

		// Token: 0x0400DC65 RID: 56421
		public float severity = -1f;

		// Token: 0x0400DC66 RID: 56422
		public bool resetLastRecreationalDrugTick;
	}
}
