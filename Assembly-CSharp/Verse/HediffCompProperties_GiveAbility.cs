using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020006BA RID: 1722
	public class HediffCompProperties_GiveAbility : HediffCompProperties
	{
		// Token: 0x06002B0B RID: 11019 RVA: 0x000E174C File Offset: 0x000DF94C
		public HediffCompProperties_GiveAbility()
		{
			this.compClass = typeof(HediffComp_GiveAbility);
		}

		// Token: 0x040022BC RID: 8892
		public AbilityDef abilityDef;

		// Token: 0x040022BD RID: 8893
		public List<AbilityDef> abilityDefs;
	}
}
