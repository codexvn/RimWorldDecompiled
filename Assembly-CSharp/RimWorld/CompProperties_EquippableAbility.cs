using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C73 RID: 11379
	public class CompProperties_EquippableAbility : CompProperties
	{
		// Token: 0x0600FF2F RID: 65327 RVA: 0x0049F5D2 File Offset: 0x0049D7D2
		public CompProperties_EquippableAbility()
		{
			this.compClass = typeof(CompEquippableAbility);
		}

		// Token: 0x0400A70C RID: 42764
		public AbilityDef abilityDef;
	}
}
