using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038FE RID: 14590
	public class SymbolResolver_BasePart_Outdoors_LeafDecorated_RandomInnerRect : SymbolResolver
	{
		// Token: 0x060141FF RID: 82431 RVA: 0x005FE4CC File Offset: 0x005FC6CC
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.rect.Width <= 15 && rp.rect.Height <= 15 && rp.rect.Width > 5 && rp.rect.Height > 5;
		}

		// Token: 0x06014200 RID: 82432 RVA: 0x005FE524 File Offset: 0x005FC724
		public override void Resolve(ResolveParams rp)
		{
			int num = Rand.RangeInclusive(5, rp.rect.Width - 1);
			int num2 = Rand.RangeInclusive(5, rp.rect.Height - 1);
			int num3 = Rand.RangeInclusive(0, rp.rect.Width - num);
			int num4 = Rand.RangeInclusive(0, rp.rect.Height - num2);
			ResolveParams resolveParams = rp;
			resolveParams.rect = new CellRect(rp.rect.minX + num3, rp.rect.minZ + num4, num, num2);
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", resolveParams, null);
		}

		// Token: 0x0400DEB3 RID: 57011
		private const int MinLength = 5;

		// Token: 0x0400DEB4 RID: 57012
		private const int MaxRectSize = 15;
	}
}
