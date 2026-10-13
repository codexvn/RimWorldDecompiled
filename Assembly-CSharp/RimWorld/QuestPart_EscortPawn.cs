using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF4 RID: 7156
	public class QuestPart_EscortPawn : QuestPart_MakeLord
	{
		// Token: 0x06009F57 RID: 40791 RVA: 0x002E0188 File Offset: 0x002DE388
		protected override Lord MakeLord()
		{
			LordJob_EscortPawn lordJob_EscortPawn = new LordJob_EscortPawn(this.escortee, this.shuttle);
			lordJob_EscortPawn.leavingDangerMessage = this.leavingDangerMessage;
			Lord lord = LordMaker.MakeNewLord(this.faction, lordJob_EscortPawn, base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06009F58 RID: 40792 RVA: 0x002E01D8 File Offset: 0x002DE3D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Values.Look<string>(ref this.leavingDangerMessage, "leavingDangerMessage", null, false);
		}

		// Token: 0x04006E31 RID: 28209
		public Pawn escortee;

		// Token: 0x04006E32 RID: 28210
		public Thing shuttle;

		// Token: 0x04006E33 RID: 28211
		public string questTag;

		// Token: 0x04006E34 RID: 28212
		public string leavingDangerMessage;
	}
}
