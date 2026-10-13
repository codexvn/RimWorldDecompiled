using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BFB RID: 7163
	public class QuestPart_VisitColony : QuestPart_MakeLord
	{
		// Token: 0x06009F78 RID: 40824 RVA: 0x002E0948 File Offset: 0x002DEB48
		protected override Lord MakeLord()
		{
			IntVec3 chillSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawns[0], out chillSpot))
			{
				chillSpot = CellFinder.RandomCell(this.pawns[0].Map);
			}
			LordJob_VisitColony lordJob = new LordJob_VisitColony(this.faction ?? this.pawns[0].Faction, chillSpot, this.durationTicks);
			return LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
		}

		// Token: 0x06009F79 RID: 40825 RVA: 0x002E09D8 File Offset: 0x002DEBD8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.durationTicks, "durationTicks", null, false);
		}

		// Token: 0x04006E54 RID: 28244
		public int? durationTicks;
	}
}
