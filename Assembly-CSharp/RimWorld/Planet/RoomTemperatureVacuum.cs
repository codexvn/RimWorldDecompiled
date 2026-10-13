using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C48 RID: 15432
	public class RoomTemperatureVacuum : IExposable
	{
		// Token: 0x06014F3B RID: 85819 RVA: 0x0064590C File Offset: 0x00643B0C
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.roomCell, "roomCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 0f, false);
			Scribe_Values.Look<float>(ref this.vacuum, "vacuum", 0f, false);
		}

		// Token: 0x0400E976 RID: 59766
		public IntVec3 roomCell;

		// Token: 0x0400E977 RID: 59767
		public float temperature;

		// Token: 0x0400E978 RID: 59768
		public float vacuum;

		// Token: 0x02003C49 RID: 15433
		public struct Data
		{
			// Token: 0x0400E979 RID: 59769
			public IntVec3 local;

			// Token: 0x0400E97A RID: 59770
			public float temperature;

			// Token: 0x0400E97B RID: 59771
			public float vacuum;
		}
	}
}
