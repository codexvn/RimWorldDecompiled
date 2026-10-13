using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BFC RID: 7164
	public class QuestPart_WaitForDurationThenExit : QuestPart_MakeLord
	{
		// Token: 0x06009F7C RID: 40828 RVA: 0x002E0A05 File Offset: 0x002DEC05
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_WaitForDurationThenExit(this.point, this.durationTicks), base.Map, null);
		}

		// Token: 0x06009F7D RID: 40829 RVA: 0x002E0A2C File Offset: 0x002DEC2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x04006E55 RID: 28245
		public IntVec3 point;

		// Token: 0x04006E56 RID: 28246
		public int durationTicks;
	}
}
