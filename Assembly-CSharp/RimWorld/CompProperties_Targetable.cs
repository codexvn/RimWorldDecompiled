using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F1E RID: 12062
	public class CompProperties_Targetable : CompProperties_UseEffect
	{
		// Token: 0x06011073 RID: 69747 RVA: 0x004E5D5E File Offset: 0x004E3F5E
		public CompProperties_Targetable()
		{
			this.compClass = typeof(CompTargetable);
		}

		// Token: 0x0400B0FA RID: 45306
		public bool psychicSensitiveTargetsOnly;

		// Token: 0x0400B0FB RID: 45307
		public bool fleshCorpsesOnly;

		// Token: 0x0400B0FC RID: 45308
		public bool nonDessicatedCorpsesOnly;

		// Token: 0x0400B0FD RID: 45309
		public bool nonDownedPawnOnly;

		// Token: 0x0400B0FE RID: 45310
		public bool ignoreQuestLodgerPawns;

		// Token: 0x0400B0FF RID: 45311
		public bool ignorePlayerFactionPawns;

		// Token: 0x0400B100 RID: 45312
		public MutantDef mutantFilter;

		// Token: 0x0400B101 RID: 45313
		public ThingDef moteOnTarget;

		// Token: 0x0400B102 RID: 45314
		public ThingDef moteConnecting;

		// Token: 0x0400B103 RID: 45315
		public FleckDef fleckOnTarget;

		// Token: 0x0400B104 RID: 45316
		public FleckDef fleckConnecting;

		// Token: 0x0400B105 RID: 45317
		public HediffDef cannotHaveHediff;
	}
}
