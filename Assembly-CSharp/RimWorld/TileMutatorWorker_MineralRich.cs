using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02002103 RID: 8451
	public class TileMutatorWorker_MineralRich : TileMutatorWorker
	{
		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x0600B874 RID: 47220 RVA: 0x00353414 File Offset: 0x00351614
		private List<ThingDef> Mineables
		{
			get
			{
				if (this.cachedMineables == null)
				{
					this.cachedMineables = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
					{
						BuildingProperties building = thingDef.building;
						if (((building != null) ? building.mineableThing : null) != null && thingDef.building.isResourceRock && !this.def.resourceBlacklist.Contains(thingDef))
						{
							this.cachedMineables.Add(thingDef);
						}
					}
				}
				return this.cachedMineables;
			}
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x0600B875 RID: 47221 RVA: 0x003534B4 File Offset: 0x003516B4
		private Perlin Perlin
		{
			get
			{
				if (this.perlin == null)
				{
					this.perlin = new Perlin(0.4000000059604645, 0.5, 0.5, 4, true, false, Gen.HashCombineInt(Find.World.info.Seed, 525214866), QualityMode.Medium);
				}
				return this.perlin;
			}
		}

		// Token: 0x0600B876 RID: 47222 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_MineralRich(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B877 RID: 47223 RVA: 0x00353512 File Offset: 0x00351712
		public override string GetLabel(PlanetTile tile)
		{
			return "MineralRich".Translate(this.GetMineableThingDefForTile(tile).Named("MINERAL"));
		}

		// Token: 0x0600B878 RID: 47224 RVA: 0x00353534 File Offset: 0x00351734
		public override string GetDescription(PlanetTile tile)
		{
			return "MineralRichDescription".Translate(this.GetMineableThingDefForTile(tile).Named("MINERAL"));
		}

		// Token: 0x0600B879 RID: 47225 RVA: 0x00353558 File Offset: 0x00351758
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float num = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map);
			ThingDef mineableThingDefForTile = this.GetMineableThingDefForTile(map.Tile);
			float num2 = this.Mineables.Sum((ThingDef d) => d.building.mineableScatterCommonality);
			float num3 = mineableThingDefForTile.building.mineableScatterCommonality / num2;
			num *= num3;
			int num4 = Mathf.RoundToInt(10000f / num);
			int x = map.Size.x;
			if (Mathf.RoundToInt((float)(x * x) / (float)num4) < 2)
			{
				num = 10000f / ((float)(x * x) * 0.5f);
			}
			GenStep_ScatterLumpsMineable genStep_ScatterLumpsMineable = new GenStep_ScatterLumpsMineable
			{
				maxValue = float.MaxValue,
				countPer10kCellsRange = new FloatRange(num, num),
				forcedDefToScatter = mineableThingDefForTile
			};
			for (int i = 0; i < 2; i++)
			{
				genStep_ScatterLumpsMineable.Generate(map, default(GenStepParams));
			}
		}

		// Token: 0x0600B87A RID: 47226 RVA: 0x00353648 File Offset: 0x00351848
		private ThingDef GetMineableThingDefForTile(PlanetTile tile)
		{
			List<ThingDef> mineables = this.Mineables;
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(tile);
			float value = this.Perlin.GetValue(tileCenter);
			int count = mineables.Count;
			int num = (int)(value * (float)count);
			if (num >= count)
			{
				num = count - 1;
			}
			return mineables[num];
		}

		// Token: 0x04007F24 RID: 32548
		private Perlin perlin;

		// Token: 0x04007F25 RID: 32549
		private const int SeedPart = 525214866;

		// Token: 0x04007F26 RID: 32550
		private List<ThingDef> cachedMineables;
	}
}
