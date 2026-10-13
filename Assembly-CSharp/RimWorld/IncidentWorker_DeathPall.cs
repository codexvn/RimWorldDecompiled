using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D8B RID: 7563
	public class IncidentWorker_DeathPall : IncidentWorker_MakeGameCondition
	{
		// Token: 0x0600A94A RID: 43338 RVA: 0x003072B6 File Offset: 0x003054B6
		public override GameConditionDef GetGameConditionDef(IncidentParms parms)
		{
			return GameConditionDefOf.DeathPall;
		}
	}
}
