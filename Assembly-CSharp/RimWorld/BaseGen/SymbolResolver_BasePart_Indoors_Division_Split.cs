using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038EE RID: 14574
	public class SymbolResolver_BasePart_Indoors_Division_Split : SymbolResolver
	{
		// Token: 0x060141C5 RID: 82373 RVA: 0x005FCDE4 File Offset: 0x005FAFE4
		private int ResolveMinLengthAfterSplit(ResolveParams rp)
		{
			int? minLengthAfterSplit = rp.minLengthAfterSplit;
			if (minLengthAfterSplit == null)
			{
				return 5;
			}
			return minLengthAfterSplit.GetValueOrDefault();
		}

		// Token: 0x060141C6 RID: 82374 RVA: 0x005FCE0A File Offset: 0x005FB00A
		private int ResolveMinWidthOrHeight(int minLengthAfterSplit)
		{
			return minLengthAfterSplit * 2 - 1;
		}

		// Token: 0x060141C7 RID: 82375 RVA: 0x005FCE14 File Offset: 0x005FB014
		public override bool CanResolve(ResolveParams rp)
		{
			int num = this.ResolveMinWidthOrHeight(this.ResolveMinLengthAfterSplit(rp));
			return base.CanResolve(rp) && (rp.rect.Width >= num || rp.rect.Height >= num);
		}

		// Token: 0x060141C8 RID: 82376 RVA: 0x005FCE60 File Offset: 0x005FB060
		public override void Resolve(ResolveParams rp)
		{
			int num = this.ResolveMinLengthAfterSplit(rp);
			int num2 = this.ResolveMinWidthOrHeight(num);
			if (rp.rect.Width < num2 && rp.rect.Height < num2)
			{
				string str = "Too small rect. params=";
				ResolveParams resolveParams = rp;
				Log.Warning(str + resolveParams.ToString());
				return;
			}
			if ((Rand.Bool && rp.rect.Height >= num2) || rp.rect.Width < num2)
			{
				int num3 = Rand.RangeInclusive(num - 1, rp.rect.Height - num);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = new CellRect(rp.rect.minX, rp.rect.minZ, rp.rect.Width, num3 + 1);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams2, null);
				ResolveParams resolveParams3 = rp;
				resolveParams3.rect = new CellRect(rp.rect.minX, rp.rect.minZ + num3, rp.rect.Width, rp.rect.Height - num3);
				BaseGen.symbolStack.Push("basePart_indoors", resolveParams3, null);
				return;
			}
			int num4 = Rand.RangeInclusive(num - 1, rp.rect.Width - num);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = new CellRect(rp.rect.minX, rp.rect.minZ, num4 + 1, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams4, null);
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = new CellRect(rp.rect.minX + num4, rp.rect.minZ, rp.rect.Width - num4, rp.rect.Height);
			BaseGen.symbolStack.Push("basePart_indoors", resolveParams5, null);
		}

		// Token: 0x0400DE98 RID: 56984
		private const int MinLengthAfterSplit = 5;
	}
}
