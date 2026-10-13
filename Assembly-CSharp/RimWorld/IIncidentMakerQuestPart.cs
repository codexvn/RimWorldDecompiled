using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001CFB RID: 7419
	public interface IIncidentMakerQuestPart
	{
		// Token: 0x0600A5B3 RID: 42419
		IEnumerable<FiringIncident> MakeIntervalIncidents();
	}
}
