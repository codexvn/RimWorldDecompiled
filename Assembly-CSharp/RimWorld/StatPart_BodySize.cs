using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E9 RID: 14057
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x06013803 RID: 79875 RVA: 0x005C9D84 File Offset: 0x005C7F84
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06013804 RID: 79876 RVA: 0x005C9DA4 File Offset: 0x005C7FA4
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06013805 RID: 79877 RVA: 0x005C9DF4 File Offset: 0x005C7FF4
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}
