using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372D RID: 14125
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x06013934 RID: 80180 RVA: 0x005CDBD5 File Offset: 0x005CBDD5
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x06013935 RID: 80181 RVA: 0x005CDBEB File Offset: 0x005CBDEB
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsWildMan(req))
			{
				return "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x06013936 RID: 80182 RVA: 0x005CDC28 File Offset: 0x005CBE28
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x0400D911 RID: 55569
		public float offset;
	}
}
