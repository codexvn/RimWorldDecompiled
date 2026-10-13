using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F94 RID: 8084
	public class GenStep_ArchonexusResearchBuildings_SecondCycle : GenStep_ArchonexusResearchBuildings
	{
		// Token: 0x0600B296 RID: 45718 RVA: 0x003321B8 File Offset: 0x003303B8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = CellRect.CenteredOn(loc, this.Size.x, this.Size.z);
			resolveParams.minorBuildingCount = new int?(4);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexusResearchBuildings", resolveParams, null);
			BaseGen.Generate();
		}
	}
}
