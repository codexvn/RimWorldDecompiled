using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D22 RID: 11554
	public class CompProperties_LootSpawn : CompProperties
	{
		// Token: 0x060103BC RID: 66492 RVA: 0x004B19DD File Offset: 0x004AFBDD
		public CompProperties_LootSpawn()
		{
			this.compClass = typeof(CompLootSpawn);
		}

		// Token: 0x0400A98E RID: 43406
		public ThingSetMakerDef contents;
	}
}
