using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F67 RID: 3943
	public class MentalBreakWorker_HumanityBreak : MentalBreakWorker
	{
		// Token: 0x0600617F RID: 24959 RVA: 0x001EDC48 File Offset: 0x001EBE48
		public override bool BreakCanOccur(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && base.BreakCanOccur(pawn) && !pawn.Inhumanized();
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x001EDC68 File Offset: 0x001EBE68
		public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			if (base.TryStart(pawn, reason, causedByMood))
			{
				if (!pawn.Inhumanized())
				{
					pawn.health.AddHediff(HediffDefOf.Inhumanized, null, null, null);
				}
				return true;
			}
			return false;
		}
	}
}
