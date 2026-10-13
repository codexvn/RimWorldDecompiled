using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AEC RID: 15084
	public class QuestNode_RequirementsToAcceptPlanetLayer : QuestNode
	{
		// Token: 0x060148BD RID: 84157 RVA: 0x00621378 File Offset: 0x0061F578
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptPlanetLayer
			{
				canBeSpace = this.canBeSpace.GetValue(slate),
				layerWhitelist = this.layerWhitelist.GetValue(slate),
				layerBlacklist = this.layerBlacklist.GetValue(slate),
				mapParent = slate.Get<Map>("map", null, false).Parent
			});
		}

		// Token: 0x060148BE RID: 84158 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E404 RID: 58372
		public SlateRef<bool> canBeSpace;

		// Token: 0x0400E405 RID: 58373
		public SlateRef<List<PlanetLayerDef>> layerWhitelist;

		// Token: 0x0400E406 RID: 58374
		public SlateRef<List<PlanetLayerDef>> layerBlacklist;
	}
}
