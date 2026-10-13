using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEA RID: 7658
	public class IncidentWorker_ObeliskDuplicator : IncidentWorker_Obelisk
	{
		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x0600AAA8 RID: 43688 RVA: 0x0030CB2F File Offset: 0x0030AD2F
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Duplicator;
			}
		}
	}
}
