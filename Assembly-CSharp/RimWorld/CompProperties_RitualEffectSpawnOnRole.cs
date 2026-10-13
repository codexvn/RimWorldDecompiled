using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002806 RID: 10246
	public class CompProperties_RitualEffectSpawnOnRole : CompProperties_RitualEffectSpawnOnPawn
	{
		// Token: 0x0600E00E RID: 57358 RVA: 0x0041C95C File Offset: 0x0041AB5C
		public CompProperties_RitualEffectSpawnOnRole()
		{
			this.compClass = typeof(CompRitualEffect_SpawnOnRole);
		}

		// Token: 0x040096C3 RID: 38595
		[NoTranslate]
		public string roleId;
	}
}
