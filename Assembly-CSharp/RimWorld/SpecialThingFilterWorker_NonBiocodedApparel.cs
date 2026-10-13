using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CC RID: 14028
	public class SpecialThingFilterWorker_NonBiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x06013792 RID: 79762 RVA: 0x005C9024 File Offset: 0x005C7224
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && !CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06013793 RID: 79763 RVA: 0x001520A4 File Offset: 0x001502A4
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel;
		}
	}
}
