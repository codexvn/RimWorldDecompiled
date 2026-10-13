using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001763 RID: 5987
	public class Trigger_HighValueThingsAround : Trigger
	{
		// Token: 0x06008C81 RID: 35969 RVA: 0x0029C918 File Offset: 0x0029AB18
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 120 == 0)
			{
				if (TutorSystem.TutorialMode)
				{
					return false;
				}
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick > 300)
				{
					float num = StealAIUtility.TotalMarketValueAround(lord.ownedPawns);
					float num2 = StealAIUtility.StartStealingMarketValueThreshold(lord);
					return num > num2;
				}
			}
			return false;
		}

		// Token: 0x04005B52 RID: 23378
		private const int CheckInterval = 120;

		// Token: 0x04005B53 RID: 23379
		private const int MinTicksSinceDamage = 300;
	}
}
