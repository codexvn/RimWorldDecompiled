using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003705 RID: 14085
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06013883 RID: 80003 RVA: 0x005CB902 File Offset: 0x005C9B02
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints && req.Thing.def.healthAffectsPrice;
		}

		// Token: 0x06013884 RID: 80004 RVA: 0x005CB933 File Offset: 0x005C9B33
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06013885 RID: 80005 RVA: 0x005CB950 File Offset: 0x005C9B50
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(req.Thing.HitPoints.ToString() + " / " + req.Thing.MaxHitPoints.ToString());
		}
	}
}
