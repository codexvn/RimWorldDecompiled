using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200144F RID: 5199
	public class JobGiver_SpectateDutySpectateRect : ThinkNode_JobGiver
	{
		// Token: 0x06007DD5 RID: 32213 RVA: 0x0025E578 File Offset: 0x0025C778
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			IntVec3 intVec;
			if (!this.TryFindSpot(pawn, duty, out intVec))
			{
				return null;
			}
			IntVec3 centerCell = duty.spectateRect.CenterCell;
			Building edifice = intVec.GetEdifice(pawn.Map);
			if (edifice != null && pawn.CanReserveSittableOrSpot(intVec, false))
			{
				return JobMaker.MakeJob(JobDefOf.SpectateCeremony, intVec, centerCell, edifice);
			}
			return JobMaker.MakeJob(JobDefOf.SpectateCeremony, intVec, centerCell);
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x0025E600 File Offset: 0x0025C800
		protected virtual bool TryFindSpot(Pawn pawn, PawnDuty duty, out IntVec3 spot)
		{
			Precept_Ritual ritual = null;
			if (pawn.GetLord() != null)
			{
				LordJob_Ritual lordJob_Ritual = pawn.GetLord().LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null)
				{
					ritual = lordJob_Ritual.Ritual;
				}
			}
			if ((duty.spectateRectPreferredSide != SpectateRectSide.None && SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out spot, duty.spectateRectPreferredSide, 1, null, ritual, new Func<IntVec3, Pawn, Map, bool>(RitualUtility.GoodSpectateCellForRitual))) || SpectatorCellFinder.TryFindSpectatorCellFor(pawn, duty.spectateRect, pawn.Map, out spot, duty.spectateRectAllowedSides, 1, null, ritual, new Func<IntVec3, Pawn, Map, bool>(RitualUtility.GoodSpectateCellForRitual)))
			{
				return true;
			}
			IntVec3 target = duty.spectateRect.CenterCell;
			if (CellFinder.TryFindRandomReachableNearbyCell(target, pawn.MapHeld, 5f, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (IntVec3 c) => c.GetRoom(pawn.MapHeld) == target.GetRoom(pawn.MapHeld) && pawn.CanReserveSittableOrSpot(c, false) && !duty.spectateRect.Contains(c), null, out spot, 999999))
			{
				return true;
			}
			string str = "Failed to find a spectator spot for ";
			Pawn pawn2 = pawn;
			Log.Warning(str + ((pawn2 != null) ? pawn2.ToString() : null));
			return false;
		}
	}
}
