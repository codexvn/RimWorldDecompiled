using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020027D4 RID: 10196
	public class RitualTargetFilter_UsableThrone : RitualTargetFilter
	{
		// Token: 0x0600DF2A RID: 57130 RVA: 0x00419458 File Offset: 0x00417658
		public RitualTargetFilter_UsableThrone()
		{
		}

		// Token: 0x0600DF2B RID: 57131 RVA: 0x00419460 File Offset: 0x00417660
		public RitualTargetFilter_UsableThrone(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DF2C RID: 57132 RVA: 0x0041946C File Offset: 0x0041766C
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			Pawn pawn = initiator.Thing as Pawn;
			rejectionReason = "";
			if (pawn == null)
			{
				return false;
			}
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(pawn) ?? pawn.ownership.AssignedThrone;
			if (building_Throne == null)
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneAssigned".Translate();
				return false;
			}
			if (!pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneIsNotAccessible".Translate();
				return false;
			}
			if (pawn.royalty.AnyUnmetThroneroomRequirements())
			{
				rejectionReason = "AbilitySpeechDisabledNoThroneUndignified".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600DF2D RID: 57133 RVA: 0x0041950C File Offset: 0x0041770C
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone((Pawn)initiator.Thing);
			if (building_Throne == null)
			{
				return TargetInfo.Invalid;
			}
			return new TargetInfo(building_Throne.InteractionCell, building_Throne.Map, false);
		}

		// Token: 0x0600DF2E RID: 57134 RVA: 0x00419546 File Offset: 0x00417746
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			yield return "AbilitySpeechTargetDescThrone".Translate();
			yield break;
		}
	}
}
