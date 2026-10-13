using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036C9 RID: 14025
	public class SpecialThingFilterWorker_BiocodedWeapons : SpecialThingFilterWorker
	{
		// Token: 0x06013789 RID: 79753 RVA: 0x005C8F90 File Offset: 0x005C7190
		public override bool Matches(Thing t)
		{
			return t.def.IsWeapon && t.TryGetComp<CompBladelinkWeapon>() == null && CompBiocodable.IsBiocoded(t);
		}

		// Token: 0x0601378A RID: 79754 RVA: 0x005C8FB1 File Offset: 0x005C71B1
		public override bool CanEverMatch(ThingDef def)
		{
			return def.IsWeapon && def.HasComp(typeof(CompBiocodable));
		}
	}
}
