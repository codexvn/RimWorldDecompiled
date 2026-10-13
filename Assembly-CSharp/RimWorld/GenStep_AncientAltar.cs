using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204C RID: 8268
	public class GenStep_AncientAltar : GenStep_ScattererBestFit
	{
		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x0600B594 RID: 46484 RVA: 0x0034312C File Offset: 0x0034132C
		public override int SeedPart
		{
			get
			{
				return 572334943;
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x0600B595 RID: 46485 RVA: 0x00343133 File Offset: 0x00341333
		protected override IntVec2 Size
		{
			get
			{
				return SymbolResolver_AncientAltar.Size;
			}
		}

		// Token: 0x0600B596 RID: 46486 RVA: 0x0034313A File Offset: 0x0034133A
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.warnOnFail = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B597 RID: 46487 RVA: 0x00343154 File Offset: 0x00341354
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
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

		// Token: 0x0600B598 RID: 46488 RVA: 0x003431D8 File Offset: 0x003413D8
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (!base.TryFindScatterCell(map, out result))
			{
				result = map.Center;
			}
			return true;
		}

		// Token: 0x0600B599 RID: 46489 RVA: 0x003431F4 File Offset: 0x003413F4
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.triggerSecuritySignal = parms2.triggerSecuritySignal;
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.relicThing = parms2.relicThing;
			resolveParams.interiorThreatPoints = ((parms2.interiorThreatPoints > 0f) ? new float?(parms2.interiorThreatPoints) : null);
			resolveParams.exteriorThreatPoints = ((parms2.exteriorThreatPoints > 0f) ? new float?(parms2.exteriorThreatPoints) : null);
			resolveParams.rect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientAltar", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(resolveParams.rect);
			parms.sitePart.relicWasSpawned = true;
		}
	}
}
