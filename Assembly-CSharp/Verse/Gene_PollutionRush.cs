using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000660 RID: 1632
	public class Gene_PollutionRush : Gene
	{
		// Token: 0x0600292B RID: 10539 RVA: 0x000D8B38 File Offset: 0x000D6D38
		public override void PostRemove()
		{
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PollutionStimulus, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			base.PostRemove();
		}
	}
}
