using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02002CAF RID: 11439
	public class GatherSpotLister
	{
		// Token: 0x060100E8 RID: 65768 RVA: 0x004A5C87 File Offset: 0x004A3E87
		public void RegisterActivated(CompGatherSpot spot)
		{
			if (!this.activeSpots.Contains(spot))
			{
				this.activeSpots.Add(spot);
			}
		}

		// Token: 0x060100E9 RID: 65769 RVA: 0x004A5CA3 File Offset: 0x004A3EA3
		public void RegisterDeactivated(CompGatherSpot spot)
		{
			if (this.activeSpots.Contains(spot))
			{
				this.activeSpots.Remove(spot);
			}
		}

		// Token: 0x0400A7D0 RID: 42960
		public List<CompGatherSpot> activeSpots = new List<CompGatherSpot>();
	}
}
