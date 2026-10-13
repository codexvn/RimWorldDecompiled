using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF9 RID: 7161
	public class QuestPart_SagnuophageMeeting : QuestPart_MakeLord
	{
		// Token: 0x06009F73 RID: 40819 RVA: 0x002E0798 File Offset: 0x002DE998
		protected override Lord MakeLord()
		{
			Lord lord = LordMaker.MakeNewLord(this.faction, new LordJob_SanguophageMeeting(this.targetCell, this.enemyTargets, this.meetingDurationTicks, this.outSignalMeetingStarted, this.outSignalMeetingCompleted, this.outSignalAllSanguophagesGone, this.inSignalDefend, this.inSignalQuestSuccess, this.inSignalQuestFail), base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06009F74 RID: 40820 RVA: 0x002E0804 File Offset: 0x002DEA04
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			Scribe_Collections.Look<Thing>(ref this.enemyTargets, "enemyTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meetingDurationTicks, "meetingDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDefend, "inSignalDefend", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingStarted, "outSignalMeetingStarted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingCompleted, "outSignalMeetingCompleted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalAllSanguophagesGone, "outSignalAllSanguophagesGone", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestSuccess, "inSignalQuestSuccess", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestFail, "inSignalQuestFail", null, false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
		}

		// Token: 0x04006E46 RID: 28230
		public IntVec3 targetCell;

		// Token: 0x04006E47 RID: 28231
		public List<Thing> enemyTargets = new List<Thing>();

		// Token: 0x04006E48 RID: 28232
		public int meetingDurationTicks;

		// Token: 0x04006E49 RID: 28233
		public string outSignalMeetingStarted;

		// Token: 0x04006E4A RID: 28234
		public string outSignalMeetingCompleted;

		// Token: 0x04006E4B RID: 28235
		public string outSignalAllSanguophagesGone;

		// Token: 0x04006E4C RID: 28236
		public string inSignalDefend;

		// Token: 0x04006E4D RID: 28237
		public string inSignalQuestSuccess;

		// Token: 0x04006E4E RID: 28238
		public string inSignalQuestFail;

		// Token: 0x04006E4F RID: 28239
		public string questTag;
	}
}
