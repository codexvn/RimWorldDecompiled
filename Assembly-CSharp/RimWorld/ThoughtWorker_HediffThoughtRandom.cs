using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BE RID: 6334
	public class ThoughtWorker_HediffThoughtRandom : ThoughtWorker
	{
		// Token: 0x0600909C RID: 37020 RVA: 0x002A9978 File Offset: 0x002A7B78
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				HediffWithComps hediffWithComps = hediff as HediffWithComps;
				if (hediffWithComps != null)
				{
					foreach (HediffComp hediffComp in hediffWithComps.comps)
					{
						HediffComp_GiveRandomSituationalThought hediffComp_GiveRandomSituationalThought = hediffComp as HediffComp_GiveRandomSituationalThought;
						if (hediffComp_GiveRandomSituationalThought != null && hediffComp_GiveRandomSituationalThought.selectedThought == this.def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
