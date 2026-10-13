using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203A RID: 8250
	public class RoadDefGenStep_Place : RoadDefGenStep_Bulldoze
	{
		// Token: 0x0600B549 RID: 46409 RVA: 0x00341E6C File Offset: 0x0034006C
		public override void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance)
		{
			if (this.onlyIfOriginAllows)
			{
				bool flag = false;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = position + GenAdj.CardinalDirections[i];
					if (intVec.InBounds(map) && this.chancePerPositionCurve.Evaluate(distance[intVec.x, intVec.z].fromRoad) > 0f && (GenConstruct.CanBuildOnTerrain(this.place, intVec, map, Rot4.North, null, null) || intVec.GetTerrain(map) == this.place) && (GenConstruct.CanBuildOnTerrain(this.place, distance[intVec.x, intVec.z].origin, map, Rot4.North, null, null) || distance[intVec.x, intVec.z].origin.GetTerrain(map) == this.place))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			if (!this.suppressOnTerrainTag.NullOrEmpty() && map.terrainGrid.TerrainAt(position).HasTag(this.suppressOnTerrainTag))
			{
				return;
			}
			base.Place(map, position, rockDef, origin, distance);
			TerrainDef terrainDef = this.place as TerrainDef;
			if (terrainDef != null)
			{
				if (this.proximitySpacing != 0)
				{
					Log.ErrorOnce("Proximity spacing used for road terrain placement; not yet supported", 60936625);
				}
				TerrainDef terrainDef2 = map.terrainGrid.TerrainAt(position);
				if (terrainDef == TerrainDefOf.FlagstoneSandstone)
				{
					terrainDef = rockDef;
				}
				if (terrainDef.bridge)
				{
					if (terrainDef2 == TerrainDefOf.WaterDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterShallow);
					}
					if (terrainDef2 == TerrainDefOf.WaterOceanDeep)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.WaterOceanShallow);
					}
				}
				if (GenConstruct.CanBuildOnTerrain(terrainDef, position, map, Rot4.North, null, null) && (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Bridge, position, map, Rot4.North, null, null) || terrainDef.bridge) && !terrainDef2.bridge)
				{
					if (terrainDef2.HasTag("Road") && !terrainDef2.Removable)
					{
						map.terrainGrid.SetTerrain(position, TerrainDefOf.Gravel);
					}
					if (terrainDef.isFoundation)
					{
						map.terrainGrid.SetFoundation(position, terrainDef);
					}
					else
					{
						map.terrainGrid.SetTerrain(position, terrainDef);
					}
				}
				if (position.OnEdge(map) && !map.roadInfo.roadEdgeTiles.Contains(position))
				{
					map.roadInfo.roadEdgeTiles.Add(position);
					return;
				}
			}
			else if (this.place is ThingDef)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.place, position, map, Rot4.North, null, null))
				{
					return;
				}
				if (this.proximitySpacing > 0 && GenClosest.ClosestThing_Global(position, map.listerThings.ThingsOfDef((ThingDef)this.place), (float)this.proximitySpacing, null, null, false) != null)
				{
					return;
				}
				List<Thing> thingList = position.GetThingList(map);
				for (int j = thingList.Count - 1; j >= 0; j--)
				{
					if (!thingList[j].def.destroyable)
					{
						return;
					}
					thingList[j].Destroy(DestroyMode.Vanish);
				}
				RoadDefGenStep_DryWithFallback.PlaceWorker(map, position, TerrainDefOf.Gravel);
				GenSpawn.Spawn(ThingMaker.MakeThing((ThingDef)this.place, null), position, map, WipeMode.Vanish);
				return;
			}
			else
			{
				Log.ErrorOnce(string.Format("Can't figure out how to place object {0} while building road", this.place), 10785584);
			}
		}

		// Token: 0x04007C7F RID: 31871
		public BuildableDef place;

		// Token: 0x04007C80 RID: 31872
		public int proximitySpacing;

		// Token: 0x04007C81 RID: 31873
		public bool onlyIfOriginAllows;

		// Token: 0x04007C82 RID: 31874
		public string suppressOnTerrainTag;
	}
}
