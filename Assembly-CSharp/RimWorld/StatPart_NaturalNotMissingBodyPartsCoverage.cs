using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200370F RID: 14095
	public class StatPart_NaturalNotMissingBodyPartsCoverage : StatPart
	{
		// Token: 0x060138B6 RID: 80054 RVA: 0x005CBFD0 File Offset: 0x005CA1D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060138B7 RID: 80055 RVA: 0x005CBFF0 File Offset: 0x005CA1F0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetValue(req, out f))
			{
				return "StatsReport_MissingBodyParts".Translate() + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138B8 RID: 80056 RVA: 0x005CC030 File Offset: 0x005CA230
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.health.hediffSet.GetCoverageOfNotMissingNaturalParts(x.RaceProps.body.corePart), (ThingDef x) => 1f, out value);
		}
	}
}
