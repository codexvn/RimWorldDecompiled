using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002833 RID: 10291
	public class TransportShipManager : IExposable
	{
		// Token: 0x17002257 RID: 8791
		// (get) Token: 0x0600E101 RID: 57601 RVA: 0x004204F4 File Offset: 0x0041E6F4
		public List<TransportShip> AllTransportShips
		{
			get
			{
				return this.ships;
			}
		}

		// Token: 0x0600E102 RID: 57602 RVA: 0x004204FC File Offset: 0x0041E6FC
		public void RegisterShipObject(TransportShip s)
		{
			this.ships.Add(s);
		}

		// Token: 0x0600E103 RID: 57603 RVA: 0x0042050A File Offset: 0x0041E70A
		public void DeregisterShipObject(TransportShip s)
		{
			if (s != null)
			{
				s.EndCurrentJob();
				this.ships.Remove(s);
			}
		}

		// Token: 0x0600E104 RID: 57604 RVA: 0x00420524 File Offset: 0x0041E724
		public void ShipObjectsTick()
		{
			for (int i = this.ships.Count - 1; i >= 0; i--)
			{
				this.ships[i].Tick();
			}
		}

		// Token: 0x0600E105 RID: 57605 RVA: 0x0042055C File Offset: 0x0041E75C
		public void ExposeData()
		{
			Scribe_Collections.Look<TransportShip>(ref this.ships, "ships", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ships.RemoveAll((TransportShip x) => x == null) != 0)
				{
					Log.Error("Removed some null ship objects.");
				}
				this.ships.RemoveAll((TransportShip x) => x.shipThing == null);
			}
		}

		// Token: 0x0400973D RID: 38717
		private List<TransportShip> ships = new List<TransportShip>();
	}
}
