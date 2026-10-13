using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3A RID: 7482
	public class GameCondition_Drought : GameCondition
	{
		// Token: 0x0600A7A5 RID: 42917 RVA: 0x003007C6 File Offset: 0x002FE9C6
		public override void GameConditionTick()
		{
			this.HandleEndDueToRain();
		}

		// Token: 0x0600A7A6 RID: 42918 RVA: 0x003007CF File Offset: 0x002FE9CF
		protected bool HandleEndDueToRain()
		{
			if (GenTicks.IsTickInterval(60) && base.SingleMap.weatherManager.curWeather.rainRate > 0f)
			{
				this.End();
				return true;
			}
			return false;
		}

		// Token: 0x0400740D RID: 29709
		public const float PlantGrowthRateFactor = 0.3f;
	}
}
