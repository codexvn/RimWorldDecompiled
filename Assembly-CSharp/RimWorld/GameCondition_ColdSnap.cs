using System;

namespace RimWorld
{
	// Token: 0x02001D32 RID: 7474
	public class GameCondition_ColdSnap : GameCondition
	{
		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x0600A766 RID: 42854 RVA: 0x002FFB34 File Offset: 0x002FDD34
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x0600A767 RID: 42855 RVA: 0x002FFB4F File Offset: 0x002FDD4F
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -20f);
		}

		// Token: 0x040073E9 RID: 29673
		private const float MaxTempOffset = -20f;
	}
}
