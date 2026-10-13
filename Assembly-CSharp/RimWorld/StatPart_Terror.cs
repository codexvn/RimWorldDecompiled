using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003728 RID: 14120
	public class StatPart_Terror : StatPart
	{
		// Token: 0x06013921 RID: 80161 RVA: 0x005CD788 File Offset: 0x005CB988
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val += TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1));
		}

		// Token: 0x06013922 RID: 80162 RVA: 0x005CD7C4 File Offset: 0x005CB9C4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && !Mathf.Approximately(TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1)), 0f))
				{
					return "StatsReport_Terror".Translate() + (": " + TerrorUtility.SuppressionFallRateOverTerror.Evaluate(pawn.GetStatValue(StatDefOf.Terror, true, -1)).ToStringPercent());
				}
			}
			return null;
		}
	}
}
