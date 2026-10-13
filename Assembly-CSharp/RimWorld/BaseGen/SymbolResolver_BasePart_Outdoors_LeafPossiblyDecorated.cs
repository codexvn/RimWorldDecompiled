using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003904 RID: 14596
	public class SymbolResolver_BasePart_Outdoors_LeafPossiblyDecorated : SymbolResolver
	{
		// Token: 0x06014214 RID: 82452 RVA: 0x005FEC8C File Offset: 0x005FCE8C
		public override void Resolve(ResolveParams rp)
		{
			if (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.25f))
			{
				BaseGen.symbolStack.Push("basePart_outdoors_leafDecorated", rp, null);
				return;
			}
			BaseGen.symbolStack.Push("basePart_outdoors_leaf", rp, null);
		}
	}
}
