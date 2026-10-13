using System;

namespace RimWorld
{
	// Token: 0x02001782 RID: 6018
	public class ShipJob_WaitForever : ShipJob_Wait
	{
		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06008D38 RID: 36152 RVA: 0x002A0078 File Offset: 0x0029E278
		protected override bool ShouldEnd
		{
			get
			{
				return this.transportShip.shipThing == null || this.transportShip.shipThing.Destroyed;
			}
		}
	}
}
