using System;

namespace Verse.AI.Group
{
	// Token: 0x020010D3 RID: 4307
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x06006926 RID: 26918 RVA: 0x002127FC File Offset: 0x002109FC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
