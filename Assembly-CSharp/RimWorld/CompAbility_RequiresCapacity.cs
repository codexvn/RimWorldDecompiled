using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022B1 RID: 8881
	public class CompAbility_RequiresCapacity : AbilityComp
	{
		// Token: 0x17001D93 RID: 7571
		// (get) Token: 0x0600C132 RID: 49458 RVA: 0x0037F196 File Offset: 0x0037D396
		public CompProperties_AbilityRequiresCapacity Props
		{
			get
			{
				return (CompProperties_AbilityRequiresCapacity)this.props;
			}
		}

		// Token: 0x0600C133 RID: 49459 RVA: 0x0037F1A4 File Offset: 0x0037D3A4
		public override bool GizmoDisabled(out string reason)
		{
			if (!this.parent.pawn.health.capacities.CapableOf(this.Props.capacity))
			{
				reason = "AbilityDisabledNoCapacity".Translate(this.parent.pawn, this.Props.capacity.label);
				return true;
			}
			reason = null;
			return false;
		}
	}
}
