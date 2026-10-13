using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B2A RID: 15146
	public class QuestNode_Root_ArchonexusVictory : QuestNode
	{
		// Token: 0x17003359 RID: 13145
		// (get) Token: 0x06014991 RID: 84369 RVA: 0x00625645 File Offset: 0x00623845
		public Faction CivilOutlander
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderCivil);
			}
		}

		// Token: 0x1700335A RID: 13146
		// (get) Token: 0x06014992 RID: 84370 RVA: 0x00625675 File Offset: 0x00623875
		public Faction RoughOutlander
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderRough);
			}
		}

		// Token: 0x1700335B RID: 13147
		// (get) Token: 0x06014993 RID: 84371 RVA: 0x006256A5 File Offset: 0x006238A5
		public Faction RoughTribe
		{
			get
			{
				return Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.TribeRough);
			}
		}

		// Token: 0x06014994 RID: 84372 RVA: 0x006256D8 File Offset: 0x006238D8
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Archonexus victory"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			quest.AddPart(new QuestPart_SubquestGenerator_ArchonexusVictory
			{
				inSignalEnable = slate.Get<string>("inSignal", null, false),
				interval = new IntRange(0, 0),
				maxSuccessfulSubquests = 3,
				maxActiveSubquests = 1,
				civilOutlander = this.CivilOutlander,
				roughOutlander = this.RoughOutlander,
				roughTribe = this.RoughTribe,
				subquestDefs = 
				{
					QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle,
					QuestScriptDefOf.EndGame_ArchonexusVictory_ThirdCycle
				}
			});
		}

		// Token: 0x06014995 RID: 84373 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}
	}
}
