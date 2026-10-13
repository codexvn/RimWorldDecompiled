using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200218F RID: 8591
	public abstract class SketchBuildable : SketchEntity
	{
		// Token: 0x17001C71 RID: 7281
		// (get) Token: 0x0600BB7B RID: 47995
		public abstract BuildableDef Buildable { get; }

		// Token: 0x17001C72 RID: 7282
		// (get) Token: 0x0600BB7C RID: 47996
		public abstract ThingDef Stuff { get; }

		// Token: 0x17001C73 RID: 7283
		// (get) Token: 0x0600BB7D RID: 47997 RVA: 0x0036416F File Offset: 0x0036236F
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x0600BB7E RID: 47998 RVA: 0x00364183 File Offset: 0x00362383
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		// Token: 0x0600BB7F RID: 47999
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x0600BB80 RID: 48000 RVA: 0x0036418E File Offset: 0x0036238E
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}

		// Token: 0x0600BB81 RID: 48001 RVA: 0x003641B4 File Offset: 0x003623B4
		public Thing FirstPermanentBlockerAt(IntVec3 at, Map map)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(at, Rot4.North, this.Buildable.Size))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (!thingList[i].def.destroyable && !GenConstruct.CanPlaceBlueprintOver(this.Buildable, thingList[i].def, null, null))
						{
							return thingList[i];
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600BB82 RID: 48002 RVA: 0x0036427C File Offset: 0x0036247C
		protected void ForceTerrainAffordance(IntVec3 at, Rot4 rot, Map map, TerrainDef defaultTerrain)
		{
			TerrainDef newTerr;
			if (defaultTerrain != null && defaultTerrain.affordances.Contains(this.Buildable.terrainAffordanceNeeded))
			{
				newTerr = defaultTerrain;
			}
			else
			{
				newTerr = map.Biome.TerrainForAffordance(this.Buildable.terrainAffordanceNeeded);
			}
			CellRect cellRect = GenAdj.OccupiedRect(at, rot, this.Buildable.Size);
			cellRect.ClipInsideMap(map);
			foreach (IntVec3 c in cellRect)
			{
				if (map.terrainGrid.TerrainAt(c).IsWater)
				{
					map.terrainGrid.SetFoundation(c, ModsConfig.OdysseyActive ? TerrainDefOf.HeavyBridge : TerrainDefOf.Bridge);
				}
				else
				{
					map.terrainGrid.SetTerrain(c, newTerr);
				}
			}
		}
	}
}
