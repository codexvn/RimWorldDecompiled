using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036EC RID: 14060
	public class StatPart_NearHarbingerTree : StatPart
	{
		// Token: 0x0601380C RID: 79884 RVA: 0x005C9EEE File Offset: 0x005C80EE
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0601380D RID: 79885 RVA: 0x005C9F0C File Offset: 0x005C810C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(ThingDefOf.Plant_TreeHarbinger.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0601380E RID: 79886 RVA: 0x005C9F6C File Offset: 0x005C816C
		private bool ActiveFor(Thing t)
		{
			if (t == null)
			{
				return false;
			}
			CompHarbingerTreeConsumable compHarbingerTreeConsumable = t.TryGetComp<CompHarbingerTreeConsumable>();
			return compHarbingerTreeConsumable != null && compHarbingerTreeConsumable.BeingConsumed;
		}

		// Token: 0x0400D895 RID: 55445
		private float multiplier = 1f;
	}
}
