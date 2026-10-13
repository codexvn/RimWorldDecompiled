using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200303C RID: 12348
	public class Alert_DateRitualComing : Alert
	{
		// Token: 0x060115AF RID: 71087 RVA: 0x004FA95B File Offset: 0x004F8B5B
		public Alert_DateRitualComing()
		{
			this.defaultLabel = "AlertRitualComing".Translate();
			this.requireIdeology = true;
		}

		// Token: 0x060115B0 RID: 71088 RVA: 0x004FA98C File Offset: 0x004F8B8C
		private void UpcomingRituals()
		{
			this.ritualEntries.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Ritual precept_Ritual in ideo.PreceptsListForReading.OfType<Precept_Ritual>())
				{
					if (!precept_Ritual.isAnytime)
					{
						RitualObligationTrigger ritualObligationTrigger = precept_Ritual.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
						if (ritualObligationTrigger != null)
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date = (RitualObligationTrigger_Date)ritualObligationTrigger;
							int num = ritualObligationTrigger_Date.OccursOnTick();
							int num2 = ritualObligationTrigger_Date.CurrentTickRelative();
							if ((float)(num - num2) < 180000f && num2 < num)
							{
								this.ritualEntries.Add("AlertRitualComingEntry".Translate(precept_Ritual.LabelCap, ritualObligationTrigger_Date.DateString, (num - num2).ToStringTicksToPeriodVerbose(true, true)));
							}
						}
					}
				}
			}
		}

		// Token: 0x060115B1 RID: 71089 RVA: 0x004FAAD4 File Offset: 0x004F8CD4
		public override TaggedString GetExplanation()
		{
			return "AlertRitualComingDesc".Translate() + ":\n" + this.ritualEntries.ToLineList("  - ") + "\n\n" + "AlertRitualComingExtra".Translate(RitualObligation.DaysToExpire).Resolve();
		}

		// Token: 0x060115B2 RID: 71090 RVA: 0x004FAB35 File Offset: 0x004F8D35
		public override AlertReport GetReport()
		{
			this.UpcomingRituals();
			return this.ritualEntries.Count > 0;
		}

		// Token: 0x0400B3BB RID: 46011
		private const float TicksBeforeWarning = 180000f;

		// Token: 0x0400B3BC RID: 46012
		private List<string> ritualEntries = new List<string>();
	}
}
