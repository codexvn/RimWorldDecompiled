using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CDC RID: 11484
	public class CompProperties_HasPawnSources : CompProperties
	{
		// Token: 0x0601021E RID: 66078 RVA: 0x004AAEA0 File Offset: 0x004A90A0
		public CompProperties_HasPawnSources()
		{
			this.compClass = typeof(CompHasPawnSources);
		}

		// Token: 0x0400A88B RID: 43147
		public bool affectLabel = true;
	}
}
