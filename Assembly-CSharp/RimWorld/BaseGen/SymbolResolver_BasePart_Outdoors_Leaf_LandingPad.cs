using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003902 RID: 14594
	public class SymbolResolver_BasePart_Outdoors_Leaf_LandingPad : SymbolResolver
	{
		// Token: 0x0601420B RID: 82443 RVA: 0x005FE8B8 File Offset: 0x005FCAB8
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && BaseGen.globalSettings.basePart_buildingsResolved >= BaseGen.globalSettings.minBuildings && (BaseGen.globalSettings.basePart_emptyNodesResolved >= BaseGen.globalSettings.minEmptyNodes || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && (BaseGen.globalSettings.basePart_landingPadsResolved == 0 || BaseGen.globalSettings.basePart_landingPadsResolved < BaseGen.globalSettings.minLandingPads) && rp.faction != null && rp.faction == Faction.OfEmpire;
		}

		// Token: 0x0601420C RID: 82444 RVA: 0x005FE954 File Offset: 0x005FCB54
		public override void Resolve(ResolveParams rp)
		{
			CellRect rect;
			if (!rp.rect.TryFindRandomInnerRect(new IntVec2(9, 9), out rect, null))
			{
				return;
			}
			ResolveParams resolveParams = rp;
			resolveParams.rect = rect;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.globalSettings.basePart_landingPadsResolved++;
		}

		// Token: 0x0400DEB6 RID: 57014
		private static List<ThingDef> availablePowerPlants = new List<ThingDef>();

		// Token: 0x0400DEB7 RID: 57015
		public const int Size = 9;
	}
}
