using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003430 RID: 13360
	public class PawnColumnWorker_Hunt : PawnColumnWorker_Designator
	{
		// Token: 0x17003137 RID: 12599
		// (get) Token: 0x06012CC7 RID: 76999 RVA: 0x00504B9F File Offset: 0x00502D9F
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.Hunt;
			}
		}

		// Token: 0x06012CC8 RID: 77000 RVA: 0x00594973 File Offset: 0x00592B73
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorHuntDesc".Translate();
		}

		// Token: 0x06012CC9 RID: 77001 RVA: 0x00594984 File Offset: 0x00592B84
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.RaceProps.IsFlesh && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06012CCA RID: 77002 RVA: 0x005949BD File Offset: 0x00592BBD
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			pawn.MapHeld.designationManager.TryRemoveDesignationOn(pawn, DesignationDefOf.Tame);
			Designator_Hunt.ShowDesignationWarnings(pawn);
		}
	}
}
