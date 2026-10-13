using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003704 RID: 14084
	public class StatPart_HasRelic : StatPart
	{
		// Token: 0x0601387F RID: 79999 RVA: 0x005CB860 File Offset: 0x005C9A60
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val += this.offset;
			}
		}

		// Token: 0x06013880 RID: 80000 RVA: 0x005CB888 File Offset: 0x005C9A88
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReports_HasRelic".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x06013881 RID: 80001 RVA: 0x005CB8E0 File Offset: 0x005C9AE0
		private bool ActiveFor(Thing thing)
		{
			CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer != null && compRelicContainer.ContainedThing != null;
		}

		// Token: 0x0400D8C5 RID: 55493
		public float offset;
	}
}
