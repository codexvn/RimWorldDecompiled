using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010E6 RID: 4326
	public class Trigger_GameCondition : Trigger
	{
		// Token: 0x0600694E RID: 26958 RVA: 0x002130F4 File Offset: 0x002112F4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 199 == 0)
			{
				foreach (GameConditionDef def in this.gameConditions)
				{
					if (lord.Map.GameConditionManager.ConditionIsActive(def))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x04004A80 RID: 19072
		public List<GameConditionDef> gameConditions;
	}
}
