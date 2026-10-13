using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EE8 RID: 12008
	public class CompProperties_GravshipFacility : CompProperties_Facility
	{
		// Token: 0x06010F36 RID: 69430 RVA: 0x004E090C File Offset: 0x004DEB0C
		public CompProperties_GravshipFacility()
		{
			this.compClass = typeof(CompGravshipFacility);
		}

		// Token: 0x06010F37 RID: 69431 RVA: 0x004E0924 File Offset: 0x004DEB24
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.fuelSavingsPercent > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_GravshipFuelSavings".Translate(), this.fuelSavingsPercent.ToStringPercentSigned("F0"), "StatsReport_GravshipFuelSavings_Desc".Translate(), 520, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B042 RID: 45122
		public bool providesFuel;

		// Token: 0x0400B043 RID: 45123
		public float fuelSavingsPercent;

		// Token: 0x0400B044 RID: 45124
		public GravshipComponentTypeDef componentTypeDef;
	}
}
