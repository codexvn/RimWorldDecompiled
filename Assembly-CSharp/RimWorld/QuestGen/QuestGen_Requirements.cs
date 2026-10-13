using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020039D4 RID: 14804
	public static class QuestGen_Requirements
	{
		// Token: 0x06014503 RID: 83203 RVA: 0x00611C30 File Offset: 0x0060FE30
		public static QuestPart_RequirementsToAcceptFactionRelation RequirementsToAcceptFactionRelation(this Quest quest, Faction faction, FactionRelationKind relationKind, bool acceptIfDefeated = false)
		{
			QuestPart_RequirementsToAcceptFactionRelation questPart_RequirementsToAcceptFactionRelation = new QuestPart_RequirementsToAcceptFactionRelation();
			questPart_RequirementsToAcceptFactionRelation.otherFaction = faction;
			questPart_RequirementsToAcceptFactionRelation.relationKind = relationKind;
			questPart_RequirementsToAcceptFactionRelation.acceptIfDefeated = acceptIfDefeated;
			quest.AddPart(questPart_RequirementsToAcceptFactionRelation);
			return questPart_RequirementsToAcceptFactionRelation;
		}
	}
}
