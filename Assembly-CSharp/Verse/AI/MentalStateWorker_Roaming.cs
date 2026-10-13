using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FAD RID: 4013
	public class MentalStateWorker_Roaming : MentalStateWorker
	{
		// Token: 0x060062A4 RID: 25252 RVA: 0x001F1642 File Offset: 0x001EF842
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && MentalStateWorker_Roaming.CanRoamNow(pawn);
		}

		// Token: 0x060062A5 RID: 25253 RVA: 0x001F1658 File Offset: 0x001EF858
		public static bool CanRoamNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.IsPlayerHome && GenTicks.TicksGame >= 300000 && pawn.RaceProps.Roamer && pawn.Faction == Faction.OfPlayer && !pawn.roping.IsRoped && !pawn.mindState.InRoamingCooldown && pawn.CanReachMapEdge();
		}

		// Token: 0x04004685 RID: 18053
		public const int GracePeriodSinceGameStartedDays = 5;
	}
}
