using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003732 RID: 14130
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x0601394A RID: 80202 RVA: 0x005CE04E File Offset: 0x005CC24E
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x0601394B RID: 80203 RVA: 0x005CE084 File Offset: 0x005CC284
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				return "NoPower".Translate().CapitalizeFirst() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0601394C RID: 80204 RVA: 0x005CE0F0 File Offset: 0x005CC2F0
		public static bool Applies(Thing th)
		{
			if (th.def.building.unpoweredWorkTableWorkSpeedFactor == 0f)
			{
				return false;
			}
			CompPowerTrader compPowerTrader = th.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && !compPowerTrader.PowerOn;
		}
	}
}
