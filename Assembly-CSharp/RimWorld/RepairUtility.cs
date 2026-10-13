using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160D RID: 5645
	public static class RepairUtility
	{
		// Token: 0x0600852F RID: 34095 RVA: 0x0027DA78 File Offset: 0x0027BC78
		public static bool PawnCanRepairEver(Pawn pawn, Thing t)
		{
			Building building = t as Building;
			return building != null && t.def.useHitPoints && building.def.building.repairable && t.Faction == pawn.Faction;
		}

		// Token: 0x06008530 RID: 34096 RVA: 0x0027DAC5 File Offset: 0x0027BCC5
		public static bool PawnCanRepairNow(Pawn pawn, Thing t)
		{
			return RepairUtility.PawnCanRepairEver(pawn, t) && pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, (Building)t) && t.HitPoints != t.MaxHitPoints;
		}
	}
}
