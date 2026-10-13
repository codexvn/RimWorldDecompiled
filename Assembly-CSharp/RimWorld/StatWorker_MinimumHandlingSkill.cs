using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003752 RID: 14162
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x06013A1F RID: 80415 RVA: 0x005D4B9D File Offset: 0x005D2D9D
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x06013A20 RID: 80416 RVA: 0x005D4BA8 File Offset: 0x005D2DA8
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef.race.Humanlike)
			{
				return "StatsReport_BaseValue".Translate() + ": " + this.ValueFromReq(req).ToString("F0");
			}
			Thing thing = req.Thing;
			float f = (thing != null) ? thing.GetStatValue(StatDefOf.Wildness, true, -1) : thingDef.GetStatValueAbstract(StatDefOf.Wildness, null);
			return "Wildness".Translate() + " " + f.ToStringPercent() + ": " + this.ValueFromReq(req).ToString("F0");
		}

		// Token: 0x06013A21 RID: 80417 RVA: 0x005D4C70 File Offset: 0x005D2E70
		private float ValueFromReq(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef.race.Humanlike)
			{
				return 0f;
			}
			Thing thing = req.Thing;
			float x = (thing != null) ? thing.GetStatValue(StatDefOf.Wildness, true, -1) : thingDef.GetStatValueAbstract(StatDefOf.Wildness, null);
			return Mathf.Clamp(StatWorker_MinimumHandlingSkill.HandlingSkillFromWildness.Evaluate(x), 0f, 20f);
		}

		// Token: 0x0400D96B RID: 55659
		private static readonly SimpleCurve HandlingSkillFromWildness = new SimpleCurve
		{
			{
				new CurvePoint(0.15f, 1f),
				true
			},
			{
				new CurvePoint(0.98f, 10f),
				true
			},
			{
				new CurvePoint(0.99f, 14f),
				true
			}
		};
	}
}
