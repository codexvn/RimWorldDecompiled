using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEB RID: 7659
	public class IncidentWorker_ObeliskAbductor : IncidentWorker_Obelisk
	{
		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x0600AAAA RID: 43690 RVA: 0x0030CB36 File Offset: 0x0030AD36
		public override ThingDef ObeliskDef
		{
			get
			{
				return ThingDefOf.WarpedObelisk_Abductor;
			}
		}
	}
}
