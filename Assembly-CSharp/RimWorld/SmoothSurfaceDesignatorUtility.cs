using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030C4 RID: 12484
	public static class SmoothSurfaceDesignatorUtility
	{
		// Token: 0x06011870 RID: 71792 RVA: 0x005088BF File Offset: 0x00506ABF
		public static bool CanSmoothFloorUnder(Building b)
		{
			return b.def.Fillage != FillCategory.Full || b.def.passability != Traversability.Impassable;
		}

		// Token: 0x06011871 RID: 71793 RVA: 0x005088E4 File Offset: 0x00506AE4
		public static void Notify_BuildingSpawned(Building b)
		{
			if (!SmoothSurfaceDesignatorUtility.CanSmoothFloorUnder(b))
			{
				foreach (IntVec3 c in b.OccupiedRect())
				{
					Designation designation = b.Map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
					if (designation != null)
					{
						b.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x06011872 RID: 71794 RVA: 0x00508968 File Offset: 0x00506B68
		public static void Notify_BuildingDespawned(Building b, Map map)
		{
			foreach (IntVec3 c in b.OccupiedRect())
			{
				Designation designation = map.designationManager.DesignationAt(c, DesignationDefOf.SmoothWall);
				if (designation != null)
				{
					map.designationManager.RemoveDesignation(designation);
				}
			}
		}

		// Token: 0x06011873 RID: 71795 RVA: 0x005089D8 File Offset: 0x00506BD8
		public static bool DesignateSmoothWall(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null && edifice.def.IsSmoothable)
			{
				if (DebugSettings.godMode)
				{
					SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(edifice, null), null);
				}
				else
				{
					map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothWall, null));
					map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06011874 RID: 71796 RVA: 0x00508A44 File Offset: 0x00506C44
		public static void DesignateSmoothFloor(Map map, IntVec3 c)
		{
			if (DebugSettings.godMode)
			{
				TerrainDef smoothedTerrain = c.GetTerrain(map).smoothedTerrain;
				map.terrainGrid.SetTerrain(c, smoothedTerrain);
				FilthMaker.RemoveAllFilth(c, map);
				return;
			}
			map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.SmoothFloor, null));
		}
	}
}
