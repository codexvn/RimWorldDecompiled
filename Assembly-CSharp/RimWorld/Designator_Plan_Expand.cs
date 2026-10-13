using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030F7 RID: 12535
	public class Designator_Plan_Expand : Designator_Plan_Add
	{
		// Token: 0x17002DC0 RID: 11712
		// (get) Token: 0x060119D8 RID: 72152 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanSelectColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002DC1 RID: 11713
		// (get) Token: 0x060119D9 RID: 72153 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060119DA RID: 72154 RVA: 0x0050E5BF File Offset: 0x0050C7BF
		public Designator_Plan_Expand()
		{
			this.defaultLabel = "DesignatorExpandPlan".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x060119DB RID: 72155 RVA: 0x0050E5E7 File Offset: 0x0050C7E7
		public void Initialize(ColorDef color)
		{
			this.colorDef = color;
		}
	}
}
