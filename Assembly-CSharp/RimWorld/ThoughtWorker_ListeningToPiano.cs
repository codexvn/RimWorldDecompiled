using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AB RID: 6315
	public class ThoughtWorker_ListeningToPiano : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600905D RID: 36957 RVA: 0x002A9136 File Offset: 0x002A7336
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Piano;
			}
		}
	}
}
