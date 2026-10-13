using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB0 RID: 11440
	public class CompProperties_GenepackContainer : CompProperties
	{
		// Token: 0x060100EB RID: 65771 RVA: 0x004A5CD3 File Offset: 0x004A3ED3
		public CompProperties_GenepackContainer()
		{
			this.compClass = typeof(CompGenepackContainer);
		}

		// Token: 0x0400A7D1 RID: 42961
		public int maxCapacity;
	}
}
