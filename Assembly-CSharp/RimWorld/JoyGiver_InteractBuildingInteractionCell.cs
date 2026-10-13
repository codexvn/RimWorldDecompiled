using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014DB RID: 5339
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x06007F8F RID: 32655 RVA: 0x00266340 File Offset: 0x00264540
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false))
			{
				return JobMaker.MakeJob(this.def.jobDef, t, t.InteractionCell);
			}
			return null;
		}
	}
}
