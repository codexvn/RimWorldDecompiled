using System;

namespace RimWorld
{
	// Token: 0x02002F65 RID: 12133
	public class CompProperties_UseEffect_LearnSkill : CompProperties_UseEffect
	{
		// Token: 0x0601118C RID: 70028 RVA: 0x004E8CAB File Offset: 0x004E6EAB
		public CompProperties_UseEffect_LearnSkill()
		{
			this.compClass = typeof(CompUseEffect_LearnSkill);
		}

		// Token: 0x0400B17C RID: 45436
		public SkillDef skill;

		// Token: 0x0400B17D RID: 45437
		public float xpGainAmount = 50000f;
	}
}
