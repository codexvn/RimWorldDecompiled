using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003934 RID: 14644
	public class SymbolResolver_Roof : SymbolResolver
	{
		// Token: 0x060142AF RID: 82607 RVA: 0x006023D8 File Offset: 0x006005D8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.noRoof != null && rp.noRoof.Value)
			{
				return;
			}
			RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
			RoofDef def = rp.roofDef ?? RoofDefOf.RoofConstructed;
			foreach (IntVec3 c in rp.rect)
			{
				if (!roofGrid.Roofed(c))
				{
					roofGrid.SetRoof(c, def);
				}
			}
		}
	}
}
