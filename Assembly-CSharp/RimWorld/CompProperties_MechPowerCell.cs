using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D3C RID: 11580
	public class CompProperties_MechPowerCell : CompProperties
	{
		// Token: 0x06010451 RID: 66641 RVA: 0x004B3F3D File Offset: 0x004B213D
		public CompProperties_MechPowerCell()
		{
			this.compClass = typeof(CompMechPowerCell);
		}

		// Token: 0x0400A9F0 RID: 43504
		public int totalPowerTicks = 2500;

		// Token: 0x0400A9F1 RID: 43505
		public bool killWhenDepleted = true;

		// Token: 0x0400A9F2 RID: 43506
		public bool showGizmoOnNonPlayerControlled;

		// Token: 0x0400A9F3 RID: 43507
		[MustTranslate]
		public string labelOverride;

		// Token: 0x0400A9F4 RID: 43508
		[MustTranslate]
		public string tooltipOverride;
	}
}
