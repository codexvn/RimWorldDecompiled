using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D88 RID: 7560
	public class IncidentWorker_CreepJoiner_Metalhorror : IncidentWorker_GiveQuest
	{
		// Token: 0x0600A93F RID: 43327 RVA: 0x00307021 File Offset: 0x00305221
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Find.Anomaly.CanNewMetalhorrorBiosignatureImplantOccur;
		}
	}
}
