using System;

namespace RimWorld.BaseGen
{
	// Token: 0x02003900 RID: 14592
	public class SymbolResolver_BasePart_Outdoors_Leaf_Empty : SymbolResolver
	{
		// Token: 0x06014205 RID: 82437 RVA: 0x005FE6C8 File Offset: 0x005FC8C8
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_landingPadsResolved >= BaseGen.globalSettings.minLandingPads || rp.rect.Width < 9 || rp.rect.Height < 9);
		}

		// Token: 0x06014206 RID: 82438 RVA: 0x005FE731 File Offset: 0x005FC931
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.globalSettings.basePart_emptyNodesResolved++;
		}
	}
}
