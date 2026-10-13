using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371C RID: 14108
	public class StatPart_Rest : StatPart
	{
		// Token: 0x060138ED RID: 80109 RVA: 0x005CCC8C File Offset: 0x005CAE8C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					val *= this.RestMultiplier(pawn.needs.rest.CurCategory);
				}
			}
		}

		// Token: 0x060138EE RID: 80110 RVA: 0x005CCCDC File Offset: 0x005CAEDC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					return pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060138EF RID: 80111 RVA: 0x005CCD4B File Offset: 0x005CAF4B
		private float RestMultiplier(RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return this.factorRested;
			case RestCategory.Tired:
				return this.factorTired;
			case RestCategory.VeryTired:
				return this.factorVeryTired;
			case RestCategory.Exhausted:
				return this.factorExhausted;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0400D8F8 RID: 55544
		private float factorExhausted = 1f;

		// Token: 0x0400D8F9 RID: 55545
		private float factorVeryTired = 1f;

		// Token: 0x0400D8FA RID: 55546
		private float factorTired = 1f;

		// Token: 0x0400D8FB RID: 55547
		private float factorRested = 1f;
	}
}
