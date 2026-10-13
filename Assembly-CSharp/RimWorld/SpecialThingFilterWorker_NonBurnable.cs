using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C7 RID: 14023
	public class SpecialThingFilterWorker_NonBurnable : SpecialThingFilterWorker
	{
		// Token: 0x06013781 RID: 79745 RVA: 0x005C8ED2 File Offset: 0x005C70D2
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && !t.BurnableByRecipe;
		}

		// Token: 0x06013782 RID: 79746 RVA: 0x005C8EED File Offset: 0x005C70ED
		public override bool CanEverMatch(ThingDef def)
		{
			return !def.burnableByRecipe || def.MadeFromStuff;
		}

		// Token: 0x06013783 RID: 79747 RVA: 0x005C8EFF File Offset: 0x005C70FF
		public override bool AlwaysMatches(ThingDef def)
		{
			return !def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
