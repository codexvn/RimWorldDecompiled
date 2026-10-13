using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200370C RID: 14092
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x060138A4 RID: 80036 RVA: 0x005CBDCE File Offset: 0x005C9FCE
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x060138A5 RID: 80037 RVA: 0x005CBDE0 File Offset: 0x005C9FE0
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsRotting(req))
			{
				return "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138A6 RID: 80038 RVA: 0x005CBE15 File Offset: 0x005CA015
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() > RotStage.Fresh;
		}
	}
}
