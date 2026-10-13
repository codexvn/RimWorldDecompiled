using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003806 RID: 14342
	public class PlaceWorker_WatermillGenerator : PlaceWorker
	{
		// Token: 0x06013D99 RID: 81305 RVA: 0x005E49C8 File Offset: 0x005E2BC8
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			using (IEnumerator<IntVec3> enumerator = CompPowerPlantWater.GroundCells(loc, rot).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
					{
						return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(checkingDef, TerrainAffordanceDefOf.Heavy));
					}
				}
			}
			if (!this.WaterCellsPresent(loc, rot, map))
			{
				return new AcceptanceReport("MustBeOnMovingWater".Translate());
			}
			return true;
		}

		// Token: 0x06013D9A RID: 81306 RVA: 0x005E4A74 File Offset: 0x005E2C74
		private bool WaterCellsPresent(IntVec3 loc, Rot4 rot, Map map)
		{
			foreach (IntVec3 c in CompPowerPlantWater.WaterCells(loc, rot))
			{
				if (!c.InBounds(map) || !c.GetAffordances(map).Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013D9B RID: 81307 RVA: 0x005E4AE0 File Offset: 0x005E2CE0
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList<IntVec3>(), Color.white, null, null, 2900);
			Color color = this.WaterCellsPresent(loc, rot, Find.CurrentMap) ? Designator_Place.CanPlaceColor.ToOpaque() : Designator_Place.CannotPlaceColor.ToOpaque();
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList<IntVec3>(), color, null, null, 2900);
			bool flag = false;
			CellRect cellRect = CompPowerPlantWater.WaterUseRect(loc, rot);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator).Cast<Thing>());
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			PlaceWorker_WatermillGenerator.waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
			foreach (Thing thing2 in PlaceWorker_WatermillGenerator.waterMills)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(thing2.Position, thing2.Rotation).ToList<IntVec3>(), new Color(0.2f, 0.2f, 1f), null, null, 2900);
				if (cellRect.Overlaps(CompPowerPlantWater.WaterUseRect(thing2.Position, thing2.Rotation)))
				{
					flag = true;
				}
			}
			PlaceWorker_WatermillGenerator.waterMills.Clear();
			Color color2 = flag ? new Color(1f, 0.6f, 0f) : Designator_Place.CanPlaceColor.ToOpaque();
			if (!flag || Time.realtimeSinceStartup % 0.4f < 0.2f)
			{
				GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(loc, rot).ToList<IntVec3>(), color2, null, null, 2900);
			}
		}

		// Token: 0x06013D9C RID: 81308 RVA: 0x005E4D08 File Offset: 0x005E2F08
		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Heavy;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
		}

		// Token: 0x0400DBAC RID: 56236
		private static List<Thing> waterMills = new List<Thing>();
	}
}
