using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BD8 RID: 7128
	public class QuestPart_ReimplantXenogerm : QuestPart_MakeLord
	{
		// Token: 0x06009EBA RID: 40634 RVA: 0x002DE41E File Offset: 0x002DC61E
		protected override Lord MakeLord()
		{
			if (!ModLister.CheckBiotech("reimplanting reward"))
			{
				return null;
			}
			return LordMaker.MakeNewLord(this.faction, new LordJob_ReimplantXenogerm(this.gatherSpot, this.waitDurationTicks, this.inSignalReimplanted), base.Map, null);
		}

		// Token: 0x06009EBB RID: 40635 RVA: 0x002DE458 File Offset: 0x002DC658
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.gatherSpot, "gatherSpot", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.inSignalReimplanted, "inSignalReimplanted", null, false);
			Scribe_Values.Look<int>(ref this.waitDurationTicks, "waitDurationTicks", 15000, false);
		}

		// Token: 0x04006DD9 RID: 28121
		public IntVec3 gatherSpot;

		// Token: 0x04006DDA RID: 28122
		public string inSignalReimplanted;

		// Token: 0x04006DDB RID: 28123
		public int waitDurationTicks;
	}
}
