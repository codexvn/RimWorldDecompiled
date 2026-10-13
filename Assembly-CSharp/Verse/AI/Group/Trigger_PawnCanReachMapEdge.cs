using System;

namespace Verse.AI.Group
{
	// Token: 0x020010D4 RID: 4308
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x06006928 RID: 26920 RVA: 0x0021286C File Offset: 0x00210A6C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 193 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
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
