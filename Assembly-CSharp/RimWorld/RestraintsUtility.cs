using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200244D RID: 9293
	public static class RestraintsUtility
	{
		// Token: 0x0600C946 RID: 51526 RVA: 0x003ADE44 File Offset: 0x003AC044
		public static bool InRestraints(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			if (pawn.HostFaction == null)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			return (lord == null || lord.LordJob == null || !lord.LordJob.NeverInRestraints) && (pawn.guest == null || !pawn.guest.Released) && !pawn.IsSlave;
		}

		// Token: 0x0600C947 RID: 51527 RVA: 0x003ADEA8 File Offset: 0x003AC0A8
		public static bool ShouldShowRestraintsInfo(Pawn pawn)
		{
			return pawn.IsPrisonerOfColony && RestraintsUtility.InRestraints(pawn);
		}
	}
}
