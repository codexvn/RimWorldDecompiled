using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002745 RID: 10053
	public class RitualOutcomeComp_DataThingPresence : RitualOutcomeComp_Data
	{
		// Token: 0x0600DC94 RID: 56468 RVA: 0x0040EB89 File Offset: 0x0040CD89
		public override void Reset()
		{
			this.presentForTicks.Clear();
		}

		// Token: 0x0600DC95 RID: 56469 RVA: 0x0040EB98 File Offset: 0x0040CD98
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Thing, float> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Thing, float>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentThing, ref this.tmpPresentTicks, true, false, false);
		}

		// Token: 0x040094D3 RID: 38099
		public Dictionary<Thing, float> presentForTicks = new Dictionary<Thing, float>();

		// Token: 0x040094D4 RID: 38100
		private List<Thing> tmpPresentThing;

		// Token: 0x040094D5 RID: 38101
		private List<float> tmpPresentTicks;
	}
}
