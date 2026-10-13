using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C5 RID: 14021
	public class SpecialThingFilterWorker_Unfinished : SpecialThingFilterWorker
	{
		// Token: 0x06013779 RID: 79737 RVA: 0x005C8E90 File Offset: 0x005C7090
		public override bool Matches(Thing t)
		{
			return t.def.isUnfinishedThing;
		}

		// Token: 0x0601377A RID: 79738 RVA: 0x00161F8E File Offset: 0x0016018E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.isUnfinishedThing;
		}

		// Token: 0x0601377B RID: 79739 RVA: 0x00161F8E File Offset: 0x0016018E
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.isUnfinishedThing;
		}
	}
}
