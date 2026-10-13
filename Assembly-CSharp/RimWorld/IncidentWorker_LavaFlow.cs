using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB1 RID: 7601
	public class IncidentWorker_LavaFlow : IncidentWorker
	{
		// Token: 0x0600A9D9 RID: 43481 RVA: 0x00309A19 File Offset: 0x00307C19
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			((Map)parms.target).GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(GameConditionDefOf.LavaFlow));
			return true;
		}
	}
}
