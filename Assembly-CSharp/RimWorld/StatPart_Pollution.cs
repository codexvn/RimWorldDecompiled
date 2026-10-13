using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003718 RID: 14104
	public class StatPart_Pollution : StatPart
	{
		// Token: 0x060138DB RID: 80091 RVA: 0x005CC65A File Offset: 0x005CA85A
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x060138DC RID: 80092 RVA: 0x005CC678 File Offset: 0x005CA878
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_Pollution".Translate() + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x060138DD RID: 80093 RVA: 0x005CC6C8 File Offset: 0x005CA8C8
		private bool ActiveFor(Thing t)
		{
			return ModLister.CheckBiotech("Pollution stat part") && (t != null && t.Spawned && t.def.deteriorateFromEnvironmentalEffects) && t.Position.IsPolluted(t.Map);
		}

		// Token: 0x0400D8E0 RID: 55520
		private float multiplier = 1f;
	}
}
