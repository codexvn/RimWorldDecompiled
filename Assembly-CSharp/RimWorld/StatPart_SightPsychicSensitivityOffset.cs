using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003723 RID: 14115
	public class StatPart_SightPsychicSensitivityOffset : StatPart
	{
		// Token: 0x06013909 RID: 80137 RVA: 0x005CD290 File Offset: 0x005CB490
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetPsychicOffset(req.Thing, out num))
			{
				val += num;
			}
		}

		// Token: 0x0601390A RID: 80138 RVA: 0x005CD2B4 File Offset: 0x005CB4B4
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (req.HasThing && this.TryGetPsychicOffset(req.Thing, out f))
			{
				return "StatsReport_SightPsychicSensitivityOffset".Translate() + (": +" + f.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0601390B RID: 80139 RVA: 0x005CD304 File Offset: 0x005CB504
		private bool TryGetPsychicOffset(Thing t, out float offset)
		{
			if (t != null)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					HediffSet hediffSet = pawn.health.hediffSet;
					BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
					float maxValue = float.MaxValue;
					float partEfficiencySpecialWeight = PawnCapacityWorker_Sight.PartEfficiencySpecialWeight;
					float num = PawnCapacityUtility.CalculateTagEfficiency(hediffSet, sightSource, maxValue, default(FloatRange), null, partEfficiencySpecialWeight);
					if (num <= this.startsAt)
					{
						offset = GenMath.LerpDoubleClamped(this.startsAt, this.endsAt, this.minBonus, this.maxBonus, num);
						return offset >= 0.01f;
					}
				}
			}
			offset = 0f;
			return false;
		}

		// Token: 0x0400D902 RID: 55554
		public float startsAt = 0.5f;

		// Token: 0x0400D903 RID: 55555
		public float minBonus;

		// Token: 0x0400D904 RID: 55556
		public float maxBonus = 0.5f;

		// Token: 0x0400D905 RID: 55557
		public float endsAt;
	}
}
