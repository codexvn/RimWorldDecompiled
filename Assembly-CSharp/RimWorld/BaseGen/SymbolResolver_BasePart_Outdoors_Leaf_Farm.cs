using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003901 RID: 14593
	public class SymbolResolver_BasePart_Outdoors_Leaf_Farm : SymbolResolver
	{
		// Token: 0x06014208 RID: 82440 RVA: 0x005FE748 File Offset: 0x005FC948
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9) && BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes && BaseGen.globalSettings.basePart_farmsCoverage + (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area < 0.55f && (BaseGen.globalSettings.maxFarms <= -1 || BaseGen.globalSettings.basePart_farmsCount < BaseGen.globalSettings.maxFarms) && (rp.rect.Width <= 15 && rp.rect.Height <= 15) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x06014209 RID: 82441 RVA: 0x005FE858 File Offset: 0x005FCA58
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("farm", rp, null);
			BaseGen.globalSettings.basePart_farmsCoverage += (float)rp.rect.Area / (float)BaseGen.globalSettings.mainRect.Area;
			BaseGen.globalSettings.basePart_farmsCount++;
		}

		// Token: 0x0400DEB5 RID: 57013
		private const float MaxCoverage = 0.55f;
	}
}
