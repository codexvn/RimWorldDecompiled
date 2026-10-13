using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C6 RID: 14022
	public class SpecialThingFilterWorker_Burnable : SpecialThingFilterWorker
	{
		// Token: 0x0601377D RID: 79741 RVA: 0x005C8E9D File Offset: 0x005C709D
		public override bool Matches(Thing t)
		{
			return this.CanEverMatch(t.def) && t.BurnableByRecipe;
		}

		// Token: 0x0601377E RID: 79742 RVA: 0x005C8EB5 File Offset: 0x005C70B5
		public override bool CanEverMatch(ThingDef def)
		{
			return def.burnableByRecipe;
		}

		// Token: 0x0601377F RID: 79743 RVA: 0x005C8EBD File Offset: 0x005C70BD
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.burnableByRecipe && !def.MadeFromStuff;
		}
	}
}
