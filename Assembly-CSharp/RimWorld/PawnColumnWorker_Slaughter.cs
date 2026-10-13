using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003433 RID: 13363
	public class PawnColumnWorker_Slaughter : PawnColumnWorker_Designator
	{
		// Token: 0x17003139 RID: 12601
		// (get) Token: 0x06012CD4 RID: 77012 RVA: 0x00507F98 File Offset: 0x00506198
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Slaughter;
			}
		}

		// Token: 0x06012CD5 RID: 77013 RVA: 0x00594B1D File Offset: 0x00592D1D
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorSlaughterDesc".Translate();
		}

		// Token: 0x06012CD6 RID: 77014 RVA: 0x005949FB File Offset: 0x00592BFB
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06012CD7 RID: 77015 RVA: 0x00594B2E File Offset: 0x00592D2E
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			SlaughterDesignatorUtility.CheckWarnAboutBondedAnimal(pawn);
		}

		// Token: 0x06012CD8 RID: 77016 RVA: 0x00594B38 File Offset: 0x00592D38
		protected override bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			if (pawn.HomeFaction == Faction.OfPlayer || pawn.HomeFaction == null)
			{
				return base.ShouldConfirmDesignation(pawn, out title);
			}
			title = "AnimalSlaughterConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
			return true;
		}
	}
}
