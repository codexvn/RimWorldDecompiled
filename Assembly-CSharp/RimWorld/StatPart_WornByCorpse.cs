using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372E RID: 14126
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x06013938 RID: 80184 RVA: 0x005CDC50 File Offset: 0x005CBE50
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		// Token: 0x06013939 RID: 80185 RVA: 0x005CDC88 File Offset: 0x005CBE88
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					return "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0400D912 RID: 55570
		private const float Factor = 0.1f;
	}
}
