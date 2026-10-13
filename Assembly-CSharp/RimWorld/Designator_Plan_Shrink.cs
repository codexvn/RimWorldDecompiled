using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030F9 RID: 12537
	public class Designator_Plan_Shrink : Designator_Plan_Remove
	{
		// Token: 0x060119E1 RID: 72161 RVA: 0x0050E750 File Offset: 0x0050C950
		public Designator_Plan_Shrink()
		{
			this.defaultLabel = "DesignatorPlanShrinkSingular".Translate();
			this.defaultDesc = "DesignatorPlanShrinkDesc".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
