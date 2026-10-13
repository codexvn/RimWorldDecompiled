using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001766 RID: 5990
	public class Trigger_WoundedGuestPresent : Trigger
	{
		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x06008C88 RID: 35976 RVA: 0x0029C975 File Offset: 0x0029AB75
		private TriggerData_PawnCycleInd Data
		{
			get
			{
				return (TriggerData_PawnCycleInd)this.data;
			}
		}

		// Token: 0x06008C89 RID: 35977 RVA: 0x0029C982 File Offset: 0x0029AB82
		public Trigger_WoundedGuestPresent()
		{
			this.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x06008C8A RID: 35978 RVA: 0x0029CA90 File Offset: 0x0029AC90
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 800 == 0)
			{
				TriggerData_PawnCycleInd data = this.Data;
				data.pawnCycleInd++;
				if (data.pawnCycleInd >= lord.ownedPawns.Count)
				{
					data.pawnCycleInd = 0;
				}
				if (lord.ownedPawns.Any<Pawn>())
				{
					Pawn pawn = lord.ownedPawns[data.pawnCycleInd];
					if (pawn.Spawned && !pawn.Downed && !pawn.InMentalState && KidnapAIUtility.ReachableWoundedGuest(pawn) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04005B57 RID: 23383
		private const int CheckInterval = 800;
	}
}
