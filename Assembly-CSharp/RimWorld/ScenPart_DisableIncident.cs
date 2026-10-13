using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020028AF RID: 10415
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170022AB RID: 8875
		// (get) Token: 0x0600E3F1 RID: 58353 RVA: 0x00429DD7 File Offset: 0x00427FD7
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x0600E3F2 RID: 58354 RVA: 0x00429DDE File Offset: 0x00427FDE
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.TraderCaravanArrival;
			yield return IncidentDefOf.OrbitalTraderArrival;
			yield return IncidentDefOf.WandererJoin;
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}
	}
}
