using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001744 RID: 5956
	public class LordToilData_Gathering : LordToilData
	{
		// Token: 0x06008BF0 RID: 35824 RVA: 0x0029ABE0 File Offset: 0x00298DE0
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.presentForTicks.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, int>(ref this.presentForTicks, "presentForTicks", LookMode.Reference, LookMode.Value, ref this.tmpPresentPawns, ref this.tmpPresentPawnsTicks, true, false, false);
		}

		// Token: 0x04005AF5 RID: 23285
		public Dictionary<Pawn, int> presentForTicks = new Dictionary<Pawn, int>();

		// Token: 0x04005AF6 RID: 23286
		private List<Pawn> tmpPresentPawns;

		// Token: 0x04005AF7 RID: 23287
		private List<int> tmpPresentPawnsTicks;
	}
}
