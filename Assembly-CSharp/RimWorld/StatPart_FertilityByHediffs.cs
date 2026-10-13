using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F8 RID: 14072
	public class StatPart_FertilityByHediffs : StatPart
	{
		// Token: 0x06013838 RID: 79928 RVA: 0x005CA6A0 File Offset: 0x005C88A0
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(32);
			StatPart_FertilityByHediffs.Factor(pawn, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06013839 RID: 79929 RVA: 0x005CA6D8 File Offset: 0x005C88D8
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val *= StatPart_FertilityByHediffs.Factor(pawn, null);
		}

		// Token: 0x0601383A RID: 79930 RVA: 0x005CA704 File Offset: 0x005C8904
		public static float Factor(Pawn pawn, StringBuilder explanation = null)
		{
			float num = 1f;
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				HediffStage curStage = hediff.CurStage;
				if (curStage != null && curStage.fertilityFactor != 1f)
				{
					num *= curStage.fertilityFactor;
					if (explanation != null)
					{
						explanation.AppendLine(hediff.LabelBaseCap + ": x" + curStage.fertilityFactor.ToStringPercent());
					}
				}
			}
			return Mathf.Max(num, 0f);
		}
	}
}
