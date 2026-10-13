using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003434 RID: 13364
	public class PawnColumnWorker_Tame : PawnColumnWorker_Designator
	{
		// Token: 0x1700313A RID: 12602
		// (get) Token: 0x06012CDA RID: 77018 RVA: 0x00508D5F File Offset: 0x00506F5F
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Tame;
			}
		}

		// Token: 0x06012CDB RID: 77019 RVA: 0x00594B90 File Offset: 0x00592D90
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorTameDesc".Translate();
		}

		// Token: 0x06012CDC RID: 77020 RVA: 0x00594BA1 File Offset: 0x00592DA1
		protected override bool HasCheckbox(Pawn pawn)
		{
			return TameUtility.CanTame(pawn) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06012CDD RID: 77021 RVA: 0x00594BB3 File Offset: 0x00592DB3
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Hunt);
			TameUtility.ShowDesignationWarnings(pawn, true);
		}
	}
}
