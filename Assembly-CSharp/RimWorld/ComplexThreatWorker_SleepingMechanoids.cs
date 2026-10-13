using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EB4 RID: 7860
	public class ComplexThreatWorker_SleepingMechanoids : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x0600AE43 RID: 44611 RVA: 0x0031B684 File Offset: 0x00319884
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfMechanoids);
		}

		// Token: 0x0600AE44 RID: 44612 RVA: 0x0031B6A8 File Offset: 0x003198A8
		private static bool MechKindSuitableForComplex(PawnKindDef def)
		{
			return def.RaceProps.IsMechanoid && !def.isGoodBreacher && def.isFighter && def.allowInMechClusters;
		}

		// Token: 0x0600AE45 RID: 44613 RVA: 0x0031B6CF File Offset: 0x003198CF
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(ComplexThreatWorker_SleepingMechanoids.MechKindSuitableForComplex), points, null);
		}
	}
}
