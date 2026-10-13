using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B2E RID: 15150
	public class QuestNode_Root_ArchonexusVictory_FirstCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700335D RID: 13149
		// (get) Token: 0x060149A4 RID: 84388 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override int ArchonexusCycle
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060149A5 RID: 84389 RVA: 0x00625C08 File Offset: 0x00623E08
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Faction faction = slate.Get<Faction>("civilOutlander", null, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.PickNewColony(faction, WorldObjectDefOf.Settlement_SecondArchonexusCycle, SoundDefOf.GameStartSting_FirstArchonexusCycle, 1);
			slate.Set<bool>("factionless", faction == null, false);
		}
	}
}
