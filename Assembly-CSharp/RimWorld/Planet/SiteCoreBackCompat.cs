using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DD6 RID: 15830
	public class SiteCoreBackCompat : IExposable
	{
		// Token: 0x0601597B RID: 88443 RVA: 0x00676E93 File Offset: 0x00675093
		public void ExposeData()
		{
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
			Scribe_Deep.Look<SitePartParams>(ref this.parms, "parms", Array.Empty<object>());
		}

		// Token: 0x0400EFF3 RID: 61427
		public SitePartDef def;

		// Token: 0x0400EFF4 RID: 61428
		public SitePartParams parms;
	}
}
