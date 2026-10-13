using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F95 RID: 8085
	public class GenStep_ArchonexusResearchBuildings_ThirdCycle : GenStep_ArchonexusResearchBuildings
	{
		// Token: 0x0600B298 RID: 45720 RVA: 0x00332228 File Offset: 0x00330428
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = CellRect.CenteredOn(loc, this.Size.x, this.Size.z);
			resolveParams.minorBuildingCount = new int?(8);
			resolveParams.minorBuildingRadialDistance = new int?(12);
			resolveParams.centralBuilding = ThingDefOf.GrandArchotechStructure;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexusResearchBuildings", resolveParams, null);
			BaseGen.Generate();
		}
	}
}
