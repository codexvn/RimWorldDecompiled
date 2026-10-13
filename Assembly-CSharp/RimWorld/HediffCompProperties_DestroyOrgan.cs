using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003850 RID: 14416
	public class HediffCompProperties_DestroyOrgan : HediffCompProperties
	{
		// Token: 0x06013F04 RID: 81668 RVA: 0x005EA71F File Offset: 0x005E891F
		public HediffCompProperties_DestroyOrgan()
		{
			this.compClass = typeof(HediffComp_DestroyOrgan);
		}

		// Token: 0x0400DC2F RID: 56367
		public string messageText;

		// Token: 0x0400DC30 RID: 56368
		public DamageDef damageType;
	}
}
