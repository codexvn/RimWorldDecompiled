using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002249 RID: 8777
	public class CompProperties_AbilityLaunchProjectile : CompProperties_AbilityEffect
	{
		// Token: 0x0600BF9A RID: 49050 RVA: 0x00378B38 File Offset: 0x00376D38
		public CompProperties_AbilityLaunchProjectile()
		{
			this.compClass = typeof(CompAbilityEffect_LaunchProjectile);
		}

		// Token: 0x0400836E RID: 33646
		public ThingDef projectileDef;
	}
}
