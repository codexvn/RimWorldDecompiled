using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AA RID: 6314
	public class ThoughtWorker_ListeningToHarpsichord : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x0600905B RID: 36955 RVA: 0x002A912F File Offset: 0x002A732F
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harpsichord;
			}
		}
	}
}
