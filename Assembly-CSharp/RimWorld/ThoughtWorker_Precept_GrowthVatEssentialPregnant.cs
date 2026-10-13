using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001830 RID: 6192
	public class ThoughtWorker_Precept_GrowthVatEssentialPregnant : ThoughtWorker_Precept
	{
		// Token: 0x06008F29 RID: 36649 RVA: 0x002A50DE File Offset: 0x002A32DE
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			return p.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false);
		}
	}
}
