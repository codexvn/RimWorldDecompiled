using System;
using RimWorld.QuestGen;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BEB RID: 7147
	public class QuestPart_AssaultColony : QuestPart_MakeLord
	{
		// Token: 0x06009F37 RID: 40759 RVA: 0x002DF9DC File Offset: 0x002DDBDC
		protected override Lord MakeLord()
		{
			Faction assaulterFaction = this.faction ?? this.pawns[0].Faction;
			bool flag = this.canTimeoutOrFlee;
			LordJob_AssaultColony lordJob = new LordJob_AssaultColony(assaulterFaction, this.canKidnap, flag, false, false, this.canSteal, false, false);
			Lord lord = LordMaker.MakeNewLord(this.faction ?? this.pawns[0].Faction, lordJob, this.mapParent.Map, null);
			QuestUtility.AddQuestTag(lord, QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.questTag));
			return lord;
		}

		// Token: 0x06009F38 RID: 40760 RVA: 0x002DFA60 File Offset: 0x002DDC60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
		}

		// Token: 0x04006E15 RID: 28181
		public bool canTimeoutOrFlee = true;

		// Token: 0x04006E16 RID: 28182
		public bool canKidnap = true;

		// Token: 0x04006E17 RID: 28183
		public bool canSteal = true;

		// Token: 0x04006E18 RID: 28184
		public string questTag;
	}
}
