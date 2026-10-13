using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C2 RID: 14018
	public class SpecialThingFilterWorker_Smeltable : SpecialThingFilterWorker
	{
		// Token: 0x0601376E RID: 79726 RVA: 0x005C8DCF File Offset: 0x005C6FCF
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.Smeltable;
		}

		// Token: 0x0601376F RID: 79727 RVA: 0x005C8DE7 File Offset: 0x005C6FE7
		public override bool CanEverMatch(ThingDef def)
		{
			return def.smeltable;
		}

		// Token: 0x06013770 RID: 79728 RVA: 0x005C8DEF File Offset: 0x005C6FEF
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.smeltable && !def.MadeFromStuff;
		}
	}
}
