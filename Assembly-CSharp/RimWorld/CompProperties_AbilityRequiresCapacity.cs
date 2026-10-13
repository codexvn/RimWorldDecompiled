using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022B0 RID: 8880
	public class CompProperties_AbilityRequiresCapacity : AbilityCompProperties
	{
		// Token: 0x0600C131 RID: 49457 RVA: 0x0037F17E File Offset: 0x0037D37E
		public CompProperties_AbilityRequiresCapacity()
		{
			this.compClass = typeof(CompAbility_RequiresCapacity);
		}

		// Token: 0x04008420 RID: 33824
		public PawnCapacityDef capacity;
	}
}
