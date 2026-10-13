using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CE RID: 14030
	public class SpecialThingFilterWorker_AdultOnlyApparel : SpecialThingFilterWorker
	{
		// Token: 0x06013799 RID: 79769 RVA: 0x005C9058 File Offset: 0x005C7258
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.IsApparel && def.apparel.developmentalStageFilter == DevelopmentalStage.Adult;
		}

		// Token: 0x0601379A RID: 79770 RVA: 0x005C8D9B File Offset: 0x005C6F9B
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0601379B RID: 79771 RVA: 0x005C8DC6 File Offset: 0x005C6FC6
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
