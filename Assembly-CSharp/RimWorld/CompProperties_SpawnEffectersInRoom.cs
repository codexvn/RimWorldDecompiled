using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003867 RID: 14439
	public class CompProperties_SpawnEffectersInRoom : CompProperties
	{
		// Token: 0x06013F5C RID: 81756 RVA: 0x005EC160 File Offset: 0x005EA360
		public CompProperties_SpawnEffectersInRoom()
		{
			this.compClass = typeof(CompSpawnEffectersInRoom);
		}

		// Token: 0x0400DC68 RID: 56424
		public EffecterDef effecter;

		// Token: 0x0400DC69 RID: 56425
		public float radius = 10f;
	}
}
