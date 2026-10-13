using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003100 RID: 12544
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		// Token: 0x17002DCC RID: 11724
		// (get) Token: 0x06011A05 RID: 72197 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06011A06 RID: 72198 RVA: 0x0050F09C File Offset: 0x0050D29C
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
