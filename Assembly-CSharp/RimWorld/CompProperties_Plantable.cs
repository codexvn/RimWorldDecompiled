using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DA0 RID: 11680
	public class CompProperties_Plantable : CompProperties
	{
		// Token: 0x060106BF RID: 67263 RVA: 0x004BD0CF File Offset: 0x004BB2CF
		public CompProperties_Plantable()
		{
			this.compClass = typeof(CompPlantable);
		}

		// Token: 0x0400AB5A RID: 43866
		public ThingDef plantDefToSpawn;
	}
}
