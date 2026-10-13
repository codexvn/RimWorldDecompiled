using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F87 RID: 3975
	public static class MentalStateUtility
	{
		// Token: 0x0600621C RID: 25116 RVA: 0x001EFCA9 File Offset: 0x001EDEA9
		public static MentalStateDef GetWanderToOwnRoomStateOrFallback(Pawn pawn)
		{
			if (MentalStateDefOf.Wander_OwnRoom.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_OwnRoom;
			}
			if (MentalStateDefOf.Wander_Sad.Worker.StateCanOccur(pawn))
			{
				return MentalStateDefOf.Wander_Sad;
			}
			return null;
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x001EFCDC File Offset: 0x001EDEDC
		public static void TryTransitionToWanderOwnRoom(MentalState mentalState)
		{
			MentalStateDef wanderToOwnRoomStateOrFallback = MentalStateUtility.GetWanderToOwnRoomStateOrFallback(mentalState.pawn);
			if (wanderToOwnRoomStateOrFallback != null)
			{
				mentalState.pawn.mindState.mentalStateHandler.TryStartMentalState(wanderToOwnRoomStateOrFallback, null, false, false, mentalState.causedByMood, null, true, false, false);
				return;
			}
			mentalState.RecoverFromState();
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x001EFD24 File Offset: 0x001EDF24
		public static void StartMentalState(Pawn pawn, MentalStateDef stateDef, MentalStateDef mechStateDef = null)
		{
			MentalStateDef stateDef2 = pawn.RaceProps.IsMechanoid ? (mechStateDef ?? stateDef) : stateDef;
			if (pawn.mindState.mentalStateHandler.TryStartMentalState(stateDef2, null, false, true, false, null, false, false, false))
			{
				RestUtility.WakeUp(pawn, true);
			}
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x001EFD6C File Offset: 0x001EDF6C
		public static bool IsHavingMentalBreak(Pawn pawn)
		{
			if (!pawn.mindState.mentalStateHandler.InMentalState)
			{
				return false;
			}
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			if (mentalStateDef == null)
			{
				return false;
			}
			List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].mentalState == mentalStateDef)
				{
					return true;
				}
			}
			return false;
		}
	}
}
