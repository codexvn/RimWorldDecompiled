using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015B9 RID: 5561
	public abstract class WorkGiver_EnterBuilding : WorkGiver_Scanner
	{
		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x0600837E RID: 33662 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x0600837F RID: 33663 RVA: 0x00277A34 File Offset: 0x00275C34
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			Building_Enterable building_Enterable = t as Building_Enterable;
			if (building_Enterable == null)
			{
				return false;
			}
			if (building_Enterable.SelectedPawn != pawn)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = building_Enterable.CanAcceptPawn(pawn);
			if (!acceptanceReport.Accepted)
			{
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					JobFailReason.Is(acceptanceReport.Reason.CapitalizeFirst(), null);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06008380 RID: 33664 RVA: 0x00277AC4 File Offset: 0x00275CC4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Building_Enterable))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.EnterBuilding, t);
		}
	}
}
