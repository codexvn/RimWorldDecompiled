using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C1E RID: 11294
	public class CompProperties_BiosculpterPod_HealingCycle : CompProperties_BiosculpterPod_BaseCycle
	{
		// Token: 0x0400A60C RID: 42508
		public List<BodyPartDef> bodyPartsToRestore;

		// Token: 0x0400A60D RID: 42509
		public List<HediffDef> conditionsToPossiblyCure;
	}
}
