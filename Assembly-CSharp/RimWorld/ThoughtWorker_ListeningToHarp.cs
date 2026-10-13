using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A9 RID: 6313
	public class ThoughtWorker_ListeningToHarp : ThoughtWorker_MusicalInstrumentListeningBase
	{
		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06009059 RID: 36953 RVA: 0x002A9120 File Offset: 0x002A7320
		protected override ThingDef InstrumentDef
		{
			get
			{
				return ThingDefOf.Harp;
			}
		}
	}
}
