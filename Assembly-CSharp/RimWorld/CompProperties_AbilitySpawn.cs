using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200227E RID: 8830
	public class CompProperties_AbilitySpawn : CompProperties_AbilityEffect
	{
		// Token: 0x0600C063 RID: 49251 RVA: 0x0037B7C1 File Offset: 0x003799C1
		public CompProperties_AbilitySpawn()
		{
			this.compClass = typeof(CompAbilityEffect_Spawn);
		}

		// Token: 0x040083CB RID: 33739
		public ThingDef thingDef;

		// Token: 0x040083CC RID: 33740
		public bool allowOnBuildings = true;

		// Token: 0x040083CD RID: 33741
		public bool sendSkipSignal = true;
	}
}
