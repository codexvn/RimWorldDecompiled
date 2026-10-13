using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372A RID: 14122
	public class StatPart_Trainable : StatPart
	{
		// Token: 0x06013928 RID: 80168 RVA: 0x005CD93D File Offset: 0x005CBB3D
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.Applies(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x06013929 RID: 80169 RVA: 0x005CD954 File Offset: 0x005CBB54
		public override string ExplanationPart(StatRequest req)
		{
			if (this.Applies(req))
			{
				return "StatsReport_Trainable".Translate(this.trainableDef.LabelCap) + (": " + this.factor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
			}
			return null;
		}

		// Token: 0x0601392A RID: 80170 RVA: 0x005CD9A8 File Offset: 0x005CBBA8
		private bool Applies(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null || !pawn.IsAnimal)
			{
				return false;
			}
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.Dig);
		}

		// Token: 0x0601392B RID: 80171 RVA: 0x005CD9E5 File Offset: 0x005CBBE5
		public override bool ForceShow(StatRequest req)
		{
			return this.Applies(req);
		}

		// Token: 0x0400D90F RID: 55567
		public TrainableDef trainableDef;

		// Token: 0x0400D910 RID: 55568
		public float factor = 1f;
	}
}
