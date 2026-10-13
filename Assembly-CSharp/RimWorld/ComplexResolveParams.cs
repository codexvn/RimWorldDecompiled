using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA4 RID: 7844
	public struct ComplexResolveParams
	{
		// Token: 0x040077B7 RID: 30647
		public List<Thing> spawnedThings;

		// Token: 0x040077B8 RID: 30648
		public LayoutRoom room;

		// Token: 0x040077B9 RID: 30649
		public List<LayoutRoom> allRooms;

		// Token: 0x040077BA RID: 30650
		public float points;

		// Token: 0x040077BB RID: 30651
		public Map map;

		// Token: 0x040077BC RID: 30652
		public CellRect complexRect;

		// Token: 0x040077BD RID: 30653
		public Faction hostileFaction;

		// Token: 0x040077BE RID: 30654
		public string triggerSignal;

		// Token: 0x040077BF RID: 30655
		public int? delayTicks;

		// Token: 0x040077C0 RID: 30656
		public bool passive;
	}
}
