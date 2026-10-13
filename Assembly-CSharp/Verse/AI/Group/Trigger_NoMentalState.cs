using System;

namespace Verse.AI.Group
{
	// Token: 0x020010D9 RID: 4313
	public class Trigger_NoMentalState : Trigger
	{
		// Token: 0x06006934 RID: 26932 RVA: 0x00212A88 File Offset: 0x00210C88
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					if (lord.ownedPawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
