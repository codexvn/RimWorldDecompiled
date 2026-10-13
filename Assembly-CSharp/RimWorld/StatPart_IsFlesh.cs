using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003709 RID: 14089
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x06013898 RID: 80024 RVA: 0x005CBBFC File Offset: 0x005C9DFC
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06013899 RID: 80025 RVA: 0x005CBC1C File Offset: 0x005C9E1C
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0601389A RID: 80026 RVA: 0x005CBC64 File Offset: 0x005C9E64
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, delegate(Pawn x)
			{
				if (!x.RaceProps.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, delegate(ThingDef x)
			{
				if (!x.race.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, out bodySize);
		}
	}
}
