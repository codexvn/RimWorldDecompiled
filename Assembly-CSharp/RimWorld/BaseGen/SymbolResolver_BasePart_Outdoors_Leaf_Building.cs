using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020038FF RID: 14591
	public class SymbolResolver_BasePart_Outdoors_Leaf_Building : SymbolResolver
	{
		// Token: 0x06014202 RID: 82434 RVA: 0x005FE5C4 File Offset: 0x005FC7C4
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings || rp.rect.Width < 9 || rp.rect.Height < 9) && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_buildingsResolved < BaseGen.globalSettings.minBuildings);
		}

		// Token: 0x06014203 RID: 82435 RVA: 0x005FE65C File Offset: 0x005FC85C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = (rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
			resolveParams.floorDef = (rp.floorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, true));
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams, null);
			BaseGen.globalSettings.basePart_buildingsResolved++;
		}
	}
}
