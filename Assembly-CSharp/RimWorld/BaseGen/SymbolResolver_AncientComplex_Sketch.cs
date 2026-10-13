using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020038ED RID: 14573
	public class SymbolResolver_AncientComplex_Sketch : SymbolResolver
	{
		// Token: 0x060141C2 RID: 82370 RVA: 0x005FCD84 File Offset: 0x005FAF84
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.ancientLayoutStructureSketch != null;
		}

		// Token: 0x060141C3 RID: 82371 RVA: 0x005FCD9C File Offset: 0x005FAF9C
		public override void Resolve(ResolveParams rp)
		{
			rp.ancientLayoutStructureSketch.layoutDef.Worker.Spawn(rp.ancientLayoutStructureSketch, BaseGen.globalSettings.map, rp.rect.Min, rp.threatPoints, null, true, false, null);
		}
	}
}
