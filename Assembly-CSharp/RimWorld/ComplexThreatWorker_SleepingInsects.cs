using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EB2 RID: 7858
	public class ComplexThreatWorker_SleepingInsects : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x0600AE3D RID: 44605 RVA: 0x0031B617 File Offset: 0x00319817
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfInsects);
		}

		// Token: 0x0600AE3E RID: 44606 RVA: 0x0031B63B File Offset: 0x0031983B
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints((PawnKindDef k) => k.RaceProps.Insect, points, null);
		}
	}
}
