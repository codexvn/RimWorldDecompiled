using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C27 RID: 11303
	public class CompProperties_BiosignatureOwner : CompProperties
	{
		// Token: 0x0600FD66 RID: 64870 RVA: 0x004983BD File Offset: 0x004965BD
		public CompProperties_BiosignatureOwner()
		{
			this.compClass = typeof(CompBiosignatureOwner);
		}

		// Token: 0x0400A613 RID: 42515
		public bool requiresAnalysis = true;
	}
}
