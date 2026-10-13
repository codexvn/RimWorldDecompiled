using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F62 RID: 12130
	public class CompProperties_UseEffectInstallImplant : CompProperties_UseEffect
	{
		// Token: 0x06011183 RID: 70019 RVA: 0x004E892E File Offset: 0x004E6B2E
		public CompProperties_UseEffectInstallImplant()
		{
			this.compClass = typeof(CompUseEffect_InstallImplant);
		}

		// Token: 0x0400B174 RID: 45428
		public HediffDef hediffDef;

		// Token: 0x0400B175 RID: 45429
		public BodyPartDef bodyPart;

		// Token: 0x0400B176 RID: 45430
		public bool canUpgrade;

		// Token: 0x0400B177 RID: 45431
		public bool allowNonColonists;

		// Token: 0x0400B178 RID: 45432
		public bool requiresExistingHediff;

		// Token: 0x0400B179 RID: 45433
		public float maxSeverity = float.MaxValue;

		// Token: 0x0400B17A RID: 45434
		public float minSeverity;

		// Token: 0x0400B17B RID: 45435
		public bool requiresPsychicallySensitive;
	}
}
