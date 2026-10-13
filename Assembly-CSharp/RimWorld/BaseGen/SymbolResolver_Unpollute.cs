using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003942 RID: 14658
	public class SymbolResolver_Unpollute : SymbolResolver
	{
		// Token: 0x060142D9 RID: 82649 RVA: 0x006034C4 File Offset: 0x006016C4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rp.rect)
			{
				if (c.InBounds(map) && c.CanUnpollute(map) && (!rp.rect.IsOnEdge(c) || Rand.Chance(rp.edgeUnpolluteChance)))
				{
					c.Unpollute(map);
				}
			}
		}
	}
}
