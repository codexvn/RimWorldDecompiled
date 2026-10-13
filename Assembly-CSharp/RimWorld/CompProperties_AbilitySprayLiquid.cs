using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002280 RID: 8832
	public class CompProperties_AbilitySprayLiquid : CompProperties_AbilityEffect
	{
		// Token: 0x0600C068 RID: 49256 RVA: 0x0037B913 File Offset: 0x00379B13
		public CompProperties_AbilitySprayLiquid()
		{
			this.compClass = typeof(CompAbilityEffect_SprayLiquid);
		}

		// Token: 0x040083CE RID: 33742
		public ThingDef projectileDef;

		// Token: 0x040083CF RID: 33743
		public int numCellsToHit;

		// Token: 0x040083D0 RID: 33744
		public EffecterDef sprayEffecter;
	}
}
