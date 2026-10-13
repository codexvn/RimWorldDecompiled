using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003729 RID: 14121
	public class StatPart_ToxicFallout : StatPart
	{
		// Token: 0x06013924 RID: 80164 RVA: 0x005CD849 File Offset: 0x005CBA49
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x06013925 RID: 80165 RVA: 0x005CD868 File Offset: 0x005CBA68
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(GameConditionDefOf.ToxicFallout.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x06013926 RID: 80166 RVA: 0x005CD8C8 File Offset: 0x005CBAC8
		private bool ActiveFor(Thing t)
		{
			return t != null && t.def.deteriorateFromEnvironmentalEffects && t.MapHeld != null && t.MapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && t.PositionHeld.IsValid && !t.PositionHeld.Roofed(t.MapHeld);
		}

		// Token: 0x0400D90E RID: 55566
		private float multiplier = 1f;
	}
}
