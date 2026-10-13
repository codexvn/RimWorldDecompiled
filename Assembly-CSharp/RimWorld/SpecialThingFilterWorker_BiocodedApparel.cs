using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CB RID: 14027
	public class SpecialThingFilterWorker_BiocodedApparel : SpecialThingFilterWorker
	{
		// Token: 0x0601378F RID: 79759 RVA: 0x005C8FF1 File Offset: 0x005C71F1
		public override bool Matches(Thing t)
		{
			return t.def.IsApparel && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x06013790 RID: 79760 RVA: 0x005C9008 File Offset: 0x005C7208
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsApparel && def.HasComp(typeof(CompBiocodable));
		}
	}
}
