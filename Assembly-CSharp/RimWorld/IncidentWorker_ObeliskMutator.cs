using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DE9 RID: 7657
	public class IncidentWorker_ObeliskMutator : IncidentWorker_Obelisk
	{
		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x0600AAA6 RID: 43686 RVA: 0x0030CB20 File Offset: 0x0030AD20
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Mutator;
			}
		}
	}
}
