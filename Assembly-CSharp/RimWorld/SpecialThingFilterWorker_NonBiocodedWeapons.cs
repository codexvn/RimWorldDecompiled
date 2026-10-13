using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036CA RID: 14026
	public class SpecialThingFilterWorker_NonBiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x0601378C RID: 79756 RVA: 0x005C8FCD File Offset: 0x005C71CD
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && (!CompBiocodable.IsBiocoded(t) || t.TryGetComp<CompBladelinkWeapon>() != null);
		}

		// Token: 0x0601378D RID: 79757 RVA: 0x0016121C File Offset: 0x0015F41C
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon;
		}
	}
}
