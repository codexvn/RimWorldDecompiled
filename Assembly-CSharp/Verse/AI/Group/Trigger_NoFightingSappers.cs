using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010CC RID: 4300
	public class Trigger_NoFightingSappers : Trigger
	{
		// Token: 0x06006915 RID: 26901 RVA: 0x002123F8 File Offset: 0x002105F8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn p = lord.ownedPawns[i];
					if (this.IsFightingSapper(p))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x0021243F File Offset: 0x0021063F
		private bool IsFightingSapper(Pawn p)
		{
			return !p.Downed && !p.InMentalState && (SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p));
		}
	}
}
