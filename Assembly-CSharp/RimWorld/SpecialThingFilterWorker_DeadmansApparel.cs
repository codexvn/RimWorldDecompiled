using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D0 RID: 14032
	public class SpecialThingFilterWorker_DeadmansApparel : SpecialThingFilterWorker
	{
		// Token: 0x060137A0 RID: 79776 RVA: 0x005C90B0 File Offset: 0x005C72B0
		public override bool Matches(Thing t)
		{
			Apparel apparel = t as Apparel;
			return apparel != null && apparel.WornByCorpse;
		}

		// Token: 0x060137A1 RID: 79777 RVA: 0x005C9096 File Offset: 0x005C7296
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.apparel.careIfWornByCorpse;
		}
	}
}
