using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C1 RID: 14017
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0601376A RID: 79722 RVA: 0x005C8D9B File Offset: 0x005C6F9B
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0601376B RID: 79723 RVA: 0x005C8DA9 File Offset: 0x005C6FA9
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x0601376C RID: 79724 RVA: 0x005C8DC6 File Offset: 0x005C6FC6
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
