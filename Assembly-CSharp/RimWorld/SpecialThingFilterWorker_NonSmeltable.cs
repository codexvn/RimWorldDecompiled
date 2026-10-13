using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C3 RID: 14019
	public class SpecialThingFilterWorker_NonSmeltable : SpecialThingFilterWorker
	{
		// Token: 0x06013772 RID: 79730 RVA: 0x005C8E04 File Offset: 0x005C7004
		public override bool Matches(Thing t)
		{
			return !t.Smeltable;
		}

		// Token: 0x06013773 RID: 79731 RVA: 0x005C8E0F File Offset: 0x005C700F
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.PotentiallySmeltable;
		}
	}
}
