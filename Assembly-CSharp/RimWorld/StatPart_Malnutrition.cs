using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200370B RID: 14091
	public class StatPart_Malnutrition : StatPart
	{
		// Token: 0x060138A0 RID: 80032 RVA: 0x005CBCF8 File Offset: 0x005C9EF8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			float num2;
			if (this.TryGetMalnutritionFactor(req, out num, out num2))
			{
				val *= num2;
			}
		}

		// Token: 0x060138A1 RID: 80033 RVA: 0x005CBD18 File Offset: 0x005C9F18
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			float f2;
			if (this.TryGetMalnutritionFactor(req, out f, out f2))
			{
				return "StatsReport_Malnutrition".Translate(f.ToStringPercent()) + ": x" + f2.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060138A2 RID: 80034 RVA: 0x005CBD64 File Offset: 0x005C9F64
		private bool TryGetMalnutritionFactor(StatRequest req, out float malnutritionSeverity, out float factor)
		{
			factor = 0f;
			malnutritionSeverity = 0f;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef == null)
					{
						return false;
					}
					malnutritionSeverity = firstHediffOfDef.Severity;
					factor = this.curve.Evaluate(malnutritionSeverity);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400D8CF RID: 55503
		private SimpleCurve curve;
	}
}
