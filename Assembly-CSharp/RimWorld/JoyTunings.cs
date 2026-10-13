using System;

namespace RimWorld
{
	// Token: 0x0200247F RID: 9343
	public static class JoyTunings
	{
		// Token: 0x04008A15 RID: 35349
		public const float BaseJoyGainPerHour = 0.36f;

		// Token: 0x04008A16 RID: 35350
		public const float ThreshLow = 0.15f;

		// Token: 0x04008A17 RID: 35351
		public const float ThreshSatisfied = 0.3f;

		// Token: 0x04008A18 RID: 35352
		public const float ThreshHigh = 0.7f;

		// Token: 0x04008A19 RID: 35353
		public const float ThreshVeryHigh = 0.85f;

		// Token: 0x04008A1A RID: 35354
		public const float BaseFallPerInterval = 0.0015f;

		// Token: 0x04008A1B RID: 35355
		public const float FallRateFactorWhenLow = 0.7f;

		// Token: 0x04008A1C RID: 35356
		public const float FallRateFactorWhenVeryLow = 0.4f;

		// Token: 0x04008A1D RID: 35357
		public const float FallRateFactorWhenFormingCaravan = 0.5f;

		// Token: 0x04008A1E RID: 35358
		public const float ToleranceGainPerJoy = 0.65f;

		// Token: 0x04008A1F RID: 35359
		public const float BoredStartToleranceThreshold = 0.5f;

		// Token: 0x04008A20 RID: 35360
		public const float BoredEndToleranceThreshold = 0.3f;
	}
}
