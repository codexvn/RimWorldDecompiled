using System;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200395E RID: 14686
	public class SymbolResolver_GenericRoom : SymbolResolver
	{
		// Token: 0x0601432D RID: 82733 RVA: 0x006067C4 File Offset: 0x006049C4
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("doors", rp, null);
			if (!this.interior.NullOrEmpty())
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ContractedBy(1);
				BaseGen.symbolStack.Push(this.interior, resolveParams, null);
			}
			ResolveParams resolveParams2 = rp;
			if (this.useRandomCarpet)
			{
				resolveParams2.floorDef = (from x in DefDatabase<TerrainDef>.AllDefsListForReading
				where x.IsCarpet
				select x).RandomElement<TerrainDef>();
			}
			resolveParams2.noRoof = new bool?(!this.allowRoof);
			BaseGen.symbolStack.Push("emptyRoom", resolveParams2, null);
		}

		// Token: 0x0400DF40 RID: 57152
		public string interior;

		// Token: 0x0400DF41 RID: 57153
		public bool useRandomCarpet;

		// Token: 0x0400DF42 RID: 57154
		public bool allowRoof = true;
	}
}
