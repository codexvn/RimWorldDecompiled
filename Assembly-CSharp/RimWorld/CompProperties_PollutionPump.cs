using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB2 RID: 11698
	public class CompProperties_PollutionPump : CompProperties, IPlantEffectRadius
	{
		// Token: 0x06010725 RID: 67365 RVA: 0x004BE82F File Offset: 0x004BCA2F
		public CompProperties_PollutionPump()
		{
			this.compClass = typeof(CompPollutionPump);
		}

		// Token: 0x170029EF RID: 10735
		// (get) Token: 0x06010726 RID: 67366 RVA: 0x004BE864 File Offset: 0x004BCA64
		public float PlantEffectRadius
		{
			get
			{
				return this.radius;
			}
		}

		// Token: 0x0400AB87 RID: 43911
		public float radius = 9.9f;

		// Token: 0x0400AB88 RID: 43912
		public int intervalTicks = 60000;

		// Token: 0x0400AB89 RID: 43913
		public EffecterDef pumpEffecterDef;

		// Token: 0x0400AB8A RID: 43914
		public int pumpsPerWastepack = 3;

		// Token: 0x0400AB8B RID: 43915
		public bool disabledByArtificialBuildings;
	}
}
