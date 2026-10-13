using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002753 RID: 10067
	public class RitualOutcomeComp_DestroyConsumableBuilding : RitualOutcomeComp
	{
		// Token: 0x170021CD RID: 8653
		// (get) Token: 0x0600DCD4 RID: 56532 RVA: 0x0040DC61 File Offset: 0x0040BE61
		protected virtual string LabelForDesc
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600DCD5 RID: 56533 RVA: 0x0040FC5F File Offset: 0x0040DE5F
		public override bool Applies(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget.HasThing;
		}
	}
}
