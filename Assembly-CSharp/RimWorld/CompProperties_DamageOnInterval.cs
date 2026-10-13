using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C4B RID: 11339
	public class CompProperties_DamageOnInterval : CompProperties
	{
		// Token: 0x0600FE45 RID: 65093 RVA: 0x0049BB1D File Offset: 0x00499D1D
		public CompProperties_DamageOnInterval()
		{
			this.compClass = typeof(CompDamageOnInterval);
		}

		// Token: 0x0400A6A5 RID: 42661
		public DamageDef damageDef;

		// Token: 0x0400A6A6 RID: 42662
		public int ticksBetweenDamage;

		// Token: 0x0400A6A7 RID: 42663
		public float damage;

		// Token: 0x0400A6A8 RID: 42664
		public List<DamageEffectStage> effectStages = new List<DamageEffectStage>();

		// Token: 0x0400A6A9 RID: 42665
		public float startHitPointsPercent = -1f;
	}
}
