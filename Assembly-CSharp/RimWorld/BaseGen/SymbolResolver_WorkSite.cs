using System;

namespace RimWorld.BaseGen
{
	// Token: 0x0200398F RID: 14735
	public class SymbolResolver_WorkSite : SymbolResolver
	{
		// Token: 0x060143C7 RID: 82887 RVA: 0x0060A4F9 File Offset: 0x006086F9
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("storage", rp, null);
		}
	}
}
