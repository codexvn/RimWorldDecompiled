using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B2F RID: 15151
	public class QuestNode_Root_ArchonexusVictory_SecondCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700335E RID: 13150
		// (get) Token: 0x060149A7 RID: 84391 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override int ArchonexusCycle
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060149A8 RID: 84392 RVA: 0x00625C6C File Offset: 0x00623E6C
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			base.TryAddStudyRequirement(quest, slate, ThingDefOf.MajorArchotechStructureStudiable);
			quest.DialogWithCloseBehavior("[resolvedQuestDescription]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly, QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd);
			Faction faction = slate.Get<Faction>("roughOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_ThirdArchonexusCycle, SoundDefOf.GameStartSting_SecondArchonexusCycle, 2);
			slate.Set<bool>("factionless", faction == null, false);
		}

		// Token: 0x0400E4BD RID: 58557
		private const int MaxAllowedRelicsToTake = 2;
	}
}
