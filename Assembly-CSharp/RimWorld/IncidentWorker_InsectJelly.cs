using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAF RID: 7599
	public class IncidentWorker_InsectJelly : IncidentWorker
	{
		// Token: 0x0600A9D4 RID: 43476 RVA: 0x00309974 File Offset: 0x00307B74
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600A9D5 RID: 43477 RVA: 0x003099A8 File Offset: 0x00307BA8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnJellyTunnels(Rand.Range(2, 3), (int)parms.points / 8, map);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0400755A RID: 30042
		public const int JellyPointsCost = 8;
	}
}
