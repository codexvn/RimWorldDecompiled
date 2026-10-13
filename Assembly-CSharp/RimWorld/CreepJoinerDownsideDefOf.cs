using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003615 RID: 13845
	[DefOf]
	public static class CreepJoinerDownsideDefOf
	{
		// Token: 0x06013400 RID: 78848 RVA: 0x005B628B File Offset: 0x005B448B
		static CreepJoinerDownsideDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(CreepJoinerDownsideDefOf));
		}

		// Token: 0x0400D541 RID: 54593
		[MayRequireAnomaly]
		public static CreepJoinerDownsideDef OrganDecay;

		// Token: 0x0400D542 RID: 54594
		[MayRequireAnomaly]
		public static CreepJoinerDownsideDef CrumblingMind;
	}
}
