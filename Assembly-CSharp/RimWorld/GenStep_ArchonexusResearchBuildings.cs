using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F93 RID: 8083
	public abstract class GenStep_ArchonexusResearchBuildings : GenStep_ScattererBestFit
	{
		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600B291 RID: 45713 RVA: 0x003320F6 File Offset: 0x003302F6
		public override int SeedPart
		{
			get
			{
				return 746395733;
			}
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600B292 RID: 45714 RVA: 0x003320FD File Offset: 0x003302FD
		protected override IntVec2 Size
		{
			get
			{
				return new IntVec2(20, 20);
			}
		}

		// Token: 0x0600B293 RID: 45715 RVA: 0x00332108 File Offset: 0x00330308
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B294 RID: 45716 RVA: 0x00332120 File Offset: 0x00330320
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
			if (cell.Roofed(map))
			{
				return true;
			}
			TerrainDef terrain = cell.GetTerrain(map);
			if (terrain != null && (terrain.IsWater || terrain.IsRoad))
			{
				return true;
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial || (thingList[i].def.building != null && thingList[i].def.building.isNaturalRock))
				{
					return true;
				}
			}
			return false;
		}
	}
}
