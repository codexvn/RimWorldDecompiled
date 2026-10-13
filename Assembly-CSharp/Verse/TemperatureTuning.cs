using System;

namespace Verse
{
	// Token: 0x02000538 RID: 1336
	public static class TemperatureTuning
	{
		// Token: 0x04001D06 RID: 7430
		public const float MinimumTemperature = -273.15f;

		// Token: 0x04001D07 RID: 7431
		public const float MaximumTemperature = 1000f;

		// Token: 0x04001D08 RID: 7432
		public const float DefaultTemperature = 21f;

		// Token: 0x04001D09 RID: 7433
		public const float DeepUndergroundTemperature = 15f;

		// Token: 0x04001D0A RID: 7434
		public static readonly SimpleCurve SeasonalTempVariationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3f),
				true
			},
			{
				new CurvePoint(0.1f, 4f),
				true
			},
			{
				new CurvePoint(1f, 28f),
				true
			}
		};

		// Token: 0x04001D0B RID: 7435
		public const float DailyTempVariationAmplitude = 7f;

		// Token: 0x04001D0C RID: 7436
		public const float DailySunEffect = 14f;

		// Token: 0x04001D0D RID: 7437
		public const float FoodRefrigerationTemp = 10f;

		// Token: 0x04001D0E RID: 7438
		public const float FoodFreezingTemp = 0f;

		// Token: 0x04001D0F RID: 7439
		public const int RoomTempEqualizeInterval = 120;

		// Token: 0x04001D10 RID: 7440
		public const int Door_TempEqualizeIntervalOpen = 34;

		// Token: 0x04001D11 RID: 7441
		public const int Door_TempEqualizeIntervalClosed = 375;

		// Token: 0x04001D12 RID: 7442
		public const float Door_TempEqualizeRate = 1f;

		// Token: 0x04001D13 RID: 7443
		public const float Vent_TempEqualizeRate = 14f;

		// Token: 0x04001D14 RID: 7444
		public const float InventoryTemperature = 14f;

		// Token: 0x04001D15 RID: 7445
		public const float DropPodTemperature = 14f;

		// Token: 0x04001D16 RID: 7446
		public const float TradeShipTemperature = 14f;

		// Token: 0x04001D17 RID: 7447
		public const float TransporterTemperature = 14f;
	}
}
