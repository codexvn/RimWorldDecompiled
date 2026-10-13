using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BEE RID: 7150
	public class QuestPart_BegForItems : QuestPart_MakeLord
	{
		// Token: 0x06009F41 RID: 40769 RVA: 0x002DFB64 File Offset: 0x002DDD64
		protected override Lord MakeLord()
		{
			IntVec3 idleSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out idleSpot))
			{
				idleSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			return LordMaker.MakeNewLord(this.faction, new LordJob_BegForItems(this.faction, idleSpot, this.target, this.thingDef, this.amount, this.outSignalItemsReceived), base.Map, null);
		}

		// Token: 0x06009F42 RID: 40770 RVA: 0x002DFBD3 File Offset: 0x002DDDD3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x04006E1C RID: 28188
		public Pawn target;

		// Token: 0x04006E1D RID: 28189
		public ThingDef thingDef;

		// Token: 0x04006E1E RID: 28190
		public int amount;

		// Token: 0x04006E1F RID: 28191
		public string outSignalItemsReceived;
	}
}
