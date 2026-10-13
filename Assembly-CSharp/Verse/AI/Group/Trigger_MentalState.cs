using System;

namespace Verse.AI.Group
{
	// Token: 0x020010D8 RID: 4312
	public class Trigger_MentalState : Trigger
	{
		// Token: 0x06006932 RID: 26930 RVA: 0x00212A44 File Offset: 0x00210C44
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
