using System;

namespace RimWorld
{
	// Token: 0x02002C80 RID: 11392
	public class CompFacilityInactiveWhenElectricityDisabled : CompFacility
	{
		// Token: 0x1700285A RID: 10330
		// (get) Token: 0x0600FFBC RID: 65468 RVA: 0x004A19BE File Offset: 0x0049FBBE
		public override bool CanBeActive
		{
			get
			{
				return base.CanBeActive && !this.parent.Map.gameConditionManager.ElectricityDisabled(this.parent.Map);
			}
		}
	}
}
