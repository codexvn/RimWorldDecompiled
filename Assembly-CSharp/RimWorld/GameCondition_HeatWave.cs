using System;

namespace RimWorld
{
	// Token: 0x02001D31 RID: 7473
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x0600A763 RID: 42851 RVA: 0x002FFB34 File Offset: 0x002FDD34
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x0600A764 RID: 42852 RVA: 0x002FFB3B File Offset: 0x002FDD3B
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		// Token: 0x040073E8 RID: 29672
		private const float MaxTempOffset = 17f;
	}
}
