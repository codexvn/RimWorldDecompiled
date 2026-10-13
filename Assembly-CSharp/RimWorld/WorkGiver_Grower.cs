using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015CC RID: 5580
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x060083DD RID: 33757 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060083DE RID: 33758 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		// Token: 0x060083DF RID: 33759 RVA: 0x00278C97 File Offset: 0x00276E97
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			int num;
			for (int i = 0; i < bList.Count; i = num + 1)
			{
				Building_PlantGrower building_PlantGrower = bList[i] as Building_PlantGrower;
				if (building_PlantGrower != null && this.ExtraRequirements(building_PlantGrower, pawn) && !building_PlantGrower.IsForbidden(pawn) && pawn.CanReach(building_PlantGrower, PathEndMode.OnCell, maxDanger, false, false, TraverseMode.ByPawn) && !building_PlantGrower.IsBurning())
				{
					foreach (IntVec3 intVec in building_PlantGrower.OccupiedRect())
					{
						yield return intVec;
					}
					WorkGiver_Grower.wantedPlantDef = null;
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int i = 0; i < zonesList.Count; i = num + 1)
			{
				Zone_Growing growZone = zonesList[i] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						string str = "Grow zone has 0 cells: ";
						Zone_Growing zone_Growing = growZone;
						Log.ErrorOnce(str + ((zone_Growing != null) ? zone_Growing.ToString() : null), -563487);
					}
					else if (this.ExtraRequirements(growZone, pawn) && !growZone.ContainsStaticFire && pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, false, TraverseMode.ByPawn))
					{
						for (int j = 0; j < growZone.cells.Count; j = num + 1)
						{
							yield return growZone.cells[j];
							num = j;
						}
						WorkGiver_Grower.wantedPlantDef = null;
						growZone = null;
					}
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			yield break;
			yield break;
		}

		// Token: 0x060083E0 RID: 33760 RVA: 0x00278CB0 File Offset: 0x00276EB0
		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			if (plantToGrowSettable == null)
			{
				return null;
			}
			return plantToGrowSettable.GetPlantDefToGrow();
		}

		// Token: 0x040056FA RID: 22266
		protected static ThingDef wantedPlantDef;
	}
}
