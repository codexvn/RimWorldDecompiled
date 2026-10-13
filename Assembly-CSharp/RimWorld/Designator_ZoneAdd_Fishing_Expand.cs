using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003106 RID: 12550
	public class Designator_ZoneAdd_Fishing_Expand : Designator_ZoneAdd_Fishing
	{
		// Token: 0x17002DD9 RID: 11737
		// (get) Token: 0x06011A2A RID: 72234 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06011A2B RID: 72235 RVA: 0x0050F91B File Offset: 0x0050DB1B
		public Designator_ZoneAdd_Fishing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
