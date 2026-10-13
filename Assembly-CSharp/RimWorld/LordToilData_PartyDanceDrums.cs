using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173C RID: 5948
	public class LordToilData_PartyDanceDrums : LordToilData_Gathering
	{
		// Token: 0x06008BD7 RID: 35799 RVA: 0x0029A10C File Offset: 0x0029830C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn, Building>(ref this.playedInstruments, "playedInstruments", LookMode.Reference, LookMode.Reference, ref this.tmpPawns, ref this.tmpUsedInstruments, true, false, false);
		}

		// Token: 0x04005ADF RID: 23263
		public Dictionary<Pawn, Building> playedInstruments = new Dictionary<Pawn, Building>();

		// Token: 0x04005AE0 RID: 23264
		private List<Pawn> tmpPawns;

		// Token: 0x04005AE1 RID: 23265
		private List<Building> tmpUsedInstruments;
	}
}
