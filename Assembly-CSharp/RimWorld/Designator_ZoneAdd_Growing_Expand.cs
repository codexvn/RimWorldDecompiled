using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003103 RID: 12547
	public class Designator_ZoneAdd_Growing_Expand : Designator_ZoneAdd_Growing
	{
		// Token: 0x17002DD2 RID: 11730
		// (get) Token: 0x06011A16 RID: 72214 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06011A17 RID: 72215 RVA: 0x0050F37F File Offset: 0x0050D57F
		public Designator_ZoneAdd_Growing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
