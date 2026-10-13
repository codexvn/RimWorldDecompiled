using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CF RID: 14031
	public class SpecialThingFilterWorker_NonDeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x0601379D RID: 79773 RVA: 0x005C9074 File Offset: 0x005C7274
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && !apparel.WornByCorpse;
		}

		// Token: 0x0601379E RID: 79774 RVA: 0x005C9096 File Offset: 0x005C7296
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
