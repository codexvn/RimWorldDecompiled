using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE9 RID: 15081
	public class QuestNode_RequirementsToAcceptBedroom : QuestNode
	{
		// Token: 0x060148B3 RID: 84147 RVA: 0x00621254 File Offset: 0x0061F454
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			QuestPart_RequirementsToAcceptBedroom questPart_RequirementsToAcceptBedroom = new QuestPart_RequirementsToAcceptBedroom();
			questPart_RequirementsToAcceptBedroom.targetPawns = (from p in this.pawns.GetValue(QuestGen.slate)
			where p.royalty != null && p.royalty.HighestTitleWithBedroomRequirements() != null
			orderby p.royalty.HighestTitleWithBedroomRequirements().def.seniority descending
			select p).ToList<Pawn>();
			questPart_RequirementsToAcceptBedroom.mapParent = slate.Get<Map>("map", null, false).Parent;
			quest.AddPart(questPart_RequirementsToAcceptBedroom);
		}

		// Token: 0x060148B4 RID: 84148 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E3FE RID: 58366
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
