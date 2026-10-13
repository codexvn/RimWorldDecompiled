using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002689 RID: 9865
	public abstract class RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8CF RID: 55503
		public abstract void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets);

		// Token: 0x0600D8D0 RID: 55504 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			return true;
		}

		// Token: 0x040092FD RID: 37629
		public RitualAttachableOutcomeEffectDef def;
	}
}
