using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200375E RID: 14174
	public class StatWorker_ForagedNutritionPerDay : StatWorker
	{
		// Token: 0x06013A52 RID: 80466 RVA: 0x005D5A1C File Offset: 0x005D3C1C
		public override float GetBaseValueFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			float result = base.GetBaseValueFor(req);
			if (ModsConfig.OdysseyActive && pawn != null)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					result = pawn.BodySize * 0.6f;
				}
			}
			return result;
		}

		// Token: 0x06013A53 RID: 80467 RVA: 0x005D5A7C File Offset: 0x005D3C7C
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			StringBuilder stringBuilder = new StringBuilder();
			float num = this.GetBaseValueFor(req);
			if (ModsConfig.OdysseyActive && pawn != null && pawn.IsAnimal)
			{
				Pawn_TrainingTracker training = pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.Forage))
				{
					num = pawn.BodySize * 0.6f;
					stringBuilder.AppendLine(string.Format("{0}: {1}", "ForageTrained".Translate(), num.ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense)));
					stringBuilder.AppendLine();
					goto IL_E8;
				}
			}
			if (num != 0f || this.stat.showZeroBaseValue)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + this.stat.ValueToString(num, numberSense, true));
				stringBuilder.AppendLine();
			}
			IL_E8:
			base.GetOffsetsAndFactorsExplanation(req, stringBuilder, num, "");
			return stringBuilder.ToString();
		}

		// Token: 0x06013A54 RID: 80468 RVA: 0x005D5B88 File Offset: 0x005D3D88
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null && pawn.IsAnimal && pawn.training != null)
			{
				return ModsConfig.OdysseyActive && pawn.training.HasLearned(TrainableDefOf.Forage);
			}
			return base.ShouldShowFor(req);
		}

		// Token: 0x0400D97A RID: 55674
		private const float ForgeBodySizeFactor = 0.6f;
	}
}
