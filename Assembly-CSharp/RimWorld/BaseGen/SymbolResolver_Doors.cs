using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200390C RID: 14604
	public class SymbolResolver_Doors : SymbolResolver
	{
		// Token: 0x06014229 RID: 82473 RVA: 0x005FF510 File Offset: 0x005FD710
		public override void Resolve(ResolveParams rp)
		{
			if (Rand.Chance(0.25f) || (rp.rect.Width >= 10 && rp.rect.Height >= 10 && Rand.Chance(0.8f)))
			{
				BaseGen.symbolStack.Push("extraDoor", rp, null);
			}
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
		}

		// Token: 0x0400DEC0 RID: 57024
		private const float ExtraDoorChance = 0.25f;
	}
}
