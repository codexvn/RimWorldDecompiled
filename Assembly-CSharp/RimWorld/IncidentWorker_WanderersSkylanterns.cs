using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E1E RID: 7710
	public class IncidentWorker_WanderersSkylanterns : IncidentWorker_VisitorGroup
	{
		// Token: 0x0600AB87 RID: 43911 RVA: 0x00310E01 File Offset: 0x0030F001
		protected override LordJob_VisitColony CreateLordJob(IncidentParms parms, List<Pawn> pawns)
		{
			LordJob_VisitColony lordJob_VisitColony = base.CreateLordJob(parms, pawns);
			lordJob_VisitColony.gifts = parms.gifts;
			parms.gifts = null;
			return lordJob_VisitColony;
		}

		// Token: 0x0600AB88 RID: 43912 RVA: 0x003080E0 File Offset: 0x003062E0
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, Pawn leader, bool traderExists)
		{
			base.SendStandardLetter(parms, pawns, Array.Empty<NamedArgument>());
		}
	}
}
