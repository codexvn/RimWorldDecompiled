using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E0E RID: 11790
	public class ShipLandingArea
	{
		// Token: 0x17002A76 RID: 10870
		// (get) Token: 0x0601098A RID: 67978 RVA: 0x004C8423 File Offset: 0x004C6623
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		// Token: 0x17002A77 RID: 10871
		// (get) Token: 0x0601098B RID: 67979 RVA: 0x004C8430 File Offset: 0x004C6630
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x17002A78 RID: 10872
		// (get) Token: 0x0601098C RID: 67980 RVA: 0x004C8438 File Offset: 0x004C6638
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof && !this.blockedByTerrainAffordance;
			}
		}

		// Token: 0x17002A79 RID: 10873
		// (get) Token: 0x0601098D RID: 67981 RVA: 0x004C8455 File Offset: 0x004C6655
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		// Token: 0x17002A7A RID: 10874
		// (get) Token: 0x0601098E RID: 67982 RVA: 0x004C845D File Offset: 0x004C665D
		public bool BlockedByTerrainAffordance
		{
			get
			{
				return this.blockedByTerrainAffordance;
			}
		}

		// Token: 0x17002A7B RID: 10875
		// (get) Token: 0x0601098F RID: 67983 RVA: 0x004C8465 File Offset: 0x004C6665
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		// Token: 0x17002A7C RID: 10876
		// (get) Token: 0x06010990 RID: 67984 RVA: 0x004C8470 File Offset: 0x004C6670
		public bool Active
		{
			get
			{
				for (int i = 0; i < this.beacons.Count; i++)
				{
					if (!this.beacons[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06010991 RID: 67985 RVA: 0x004C84A9 File Offset: 0x004C66A9
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		// Token: 0x06010992 RID: 67986 RVA: 0x004C84CC File Offset: 0x004C66CC
		public void RecalculateBlockingThing()
		{
			this.blockedByRoof = false;
			foreach (IntVec3 c in this.rect)
			{
				if (c.Roofed(this.map))
				{
					this.blockedByRoof = true;
					break;
				}
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!(thingList[i] is Pawn) && (thingList[i].def.Fillage != FillCategory.None || thingList[i].def.IsEdifice() || thingList[i] is Skyfaller))
					{
						this.firstBlockingThing = thingList[i];
						return;
					}
				}
				TerrainAffordanceDef landingAreaTerrainSupport = this.beacons[0].Props.landingAreaTerrainSupport;
				if (landingAreaTerrainSupport != null && !c.SupportsStructureType(this.map, landingAreaTerrainSupport))
				{
					this.blockedByTerrainAffordance = true;
					return;
				}
			}
			this.firstBlockingThing = null;
		}

		// Token: 0x0400ACF1 RID: 44273
		private CellRect rect;

		// Token: 0x0400ACF2 RID: 44274
		private Map map;

		// Token: 0x0400ACF3 RID: 44275
		private Thing firstBlockingThing;

		// Token: 0x0400ACF4 RID: 44276
		private bool blockedByRoof;

		// Token: 0x0400ACF5 RID: 44277
		private bool blockedByTerrainAffordance;

		// Token: 0x0400ACF6 RID: 44278
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
