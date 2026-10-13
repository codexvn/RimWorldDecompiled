using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200385E RID: 14430
	public class Hediff_HeartAttack : HediffWithComps
	{
		// Token: 0x06013F31 RID: 81713 RVA: 0x005EB365 File Offset: 0x005E9565
		public override void PostMake()
		{
			base.PostMake();
			this.intervalFactor = Rand.Range(0.1f, 2f);
		}

		// Token: 0x06013F32 RID: 81714 RVA: 0x005EB382 File Offset: 0x005E9582
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalFactor, "intervalFactor", 0f, false);
		}

		// Token: 0x06013F33 RID: 81715 RVA: 0x005EB3A0 File Offset: 0x005E95A0
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick((int)(5000f * this.intervalFactor), delta))
			{
				this.Severity += Rand.Range(-0.4f, 0.6f);
			}
		}

		// Token: 0x06013F34 RID: 81716 RVA: 0x005EB3E0 File Offset: 0x005E95E0
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, 0);
			float num = 0.65f * quality;
			if (Rand.Value < num)
			{
				if (batchPosition == 0 && this.pawn.Spawned)
				{
					MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatSuccess".Translate(num.ToStringPercent()), 6.5f);
				}
				this.Severity -= 0.3f;
				return;
			}
			if (batchPosition == 0 && this.pawn.Spawned)
			{
				MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "TextMote_TreatFailed".Translate(num.ToStringPercent()), 6.5f);
			}
		}

		// Token: 0x0400DC45 RID: 56389
		private float intervalFactor;

		// Token: 0x0400DC46 RID: 56390
		private const int SeverityChangeInterval = 5000;

		// Token: 0x0400DC47 RID: 56391
		private const float TendSuccessChanceFactor = 0.65f;

		// Token: 0x0400DC48 RID: 56392
		private const float TendSeverityReduction = 0.3f;
	}
}
