using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CD RID: 14029
	public class SpecialThingFilterWorker_ChildOnlyApparel : SpecialThingFilterWorker
	{
		// Token: 0x06013795 RID: 79765 RVA: 0x005C903E File Offset: 0x005C723E
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.IsApparel && def.apparel.developmentalStageFilter == DevelopmentalStage.Child;
		}

		// Token: 0x06013796 RID: 79766 RVA: 0x005C8D9B File Offset: 0x005C6F9B
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x06013797 RID: 79767 RVA: 0x005C8DC6 File Offset: 0x005C6FC6
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
