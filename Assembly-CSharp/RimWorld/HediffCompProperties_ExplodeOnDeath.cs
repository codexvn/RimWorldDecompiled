using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003855 RID: 14421
	public class HediffCompProperties_ExplodeOnDeath : HediffCompProperties
	{
		// Token: 0x06013F12 RID: 81682 RVA: 0x005EAB96 File Offset: 0x005E8D96
		public HediffCompProperties_ExplodeOnDeath()
		{
			this.compClass = typeof(HediffComp_ExplodeOnDeath);
		}

		// Token: 0x0400DC3C RID: 56380
		public bool destroyGear;

		// Token: 0x0400DC3D RID: 56381
		public bool destroyBody;

		// Token: 0x0400DC3E RID: 56382
		public float explosionRadius;

		// Token: 0x0400DC3F RID: 56383
		public DamageDef damageDef;

		// Token: 0x0400DC40 RID: 56384
		public int damageAmount = -1;
	}
}
