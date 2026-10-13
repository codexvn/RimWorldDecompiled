using System;

namespace RimWorld.BaseGen
{
	// Token: 0x020038EC RID: 14572
	public class SymbolResolver_AncientComplex_Mechanitor : SymbolResolver_AncientComplex_Base
	{
		// Token: 0x1700332F RID: 13103
		// (get) Token: 0x060141BF RID: 82367 RVA: 0x005FCD37 File Offset: 0x005FAF37
		protected override LayoutDef DefaultLayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Mechanitor_Loot;
			}
		}

		// Token: 0x060141C0 RID: 82368 RVA: 0x005FCD40 File Offset: 0x005FAF40
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.floorDef = TerrainDefOf.PackedDirt;
			BaseGen.symbolStack.Push("outdoorsPath", resolveParams, null);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
			base.ResolveComplex(rp);
		}
	}
}
