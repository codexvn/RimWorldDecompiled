using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3B RID: 7483
	public class GameCondition_DroughtInitial : GameCondition_Drought
	{
		// Token: 0x0600A7A8 RID: 42920 RVA: 0x00300800 File Offset: 0x002FEA00
		public override void GameConditionTick()
		{
			if (base.HandleEndDueToRain())
			{
				return;
			}
			if (Find.TickManager.TicksGame > this.startTick + 240000)
			{
				IncidentDef drought = IncidentDefOf.Drought;
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(drought.category, base.SingleMap);
				this.End();
				drought.Worker.TryExecute(parms);
			}
		}

		// Token: 0x0400740E RID: 29710
		private const int DurationUntilFullDrought = 240000;
	}
}
