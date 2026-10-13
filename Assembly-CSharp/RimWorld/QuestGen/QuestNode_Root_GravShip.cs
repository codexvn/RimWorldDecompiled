using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B4A RID: 15178
	public class QuestNode_Root_GravShip : QuestNode
	{
		// Token: 0x06014A1B RID: 84507 RVA: 0x00624C29 File Offset: 0x00622E29
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x06014A1C RID: 84508 RVA: 0x006292A0 File Offset: 0x006274A0
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			string inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			QuestPart_SubquestGenerator_Gravcores questPart_SubquestGenerator_Gravcores = new QuestPart_SubquestGenerator_Gravcores
			{
				inSignalEnable = inSignalEnable,
				maxActiveSubquests = 3
			};
			questPart_SubquestGenerator_Gravcores.subquestDefs.AddRange(this.subquestDefs);
			quest.AddPart(questPart_SubquestGenerator_Gravcores);
			if (Faction.OfMechanoids != null)
			{
				quest.Delay(2700000, delegate
				{
					Quest quest = quest;
					LetterDef neutralEvent = LetterDefOf.NeutralEvent;
					string inSignal = null;
					string chosenPawnSignal = null;
					Faction relatedFaction = null;
					MapParent useColonistsOnMap = null;
					bool useColonistsFromCaravanArg = false;
					QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
					IEnumerable<object> lookTargets = null;
					bool filterDeadPawnsFromLookTargets = false;
					string label = "LetterLabelMechhiveWarning".Translate();
					quest.Letter(neutralEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextMechhiveWarning".Translate(), null, label, null, null);
				}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			}
		}

		// Token: 0x0400E51D RID: 58653
		private const int MechhiveWarningDelayTicks = 2700000;

		// Token: 0x0400E51E RID: 58654
		private List<QuestScriptDef> subquestDefs = new List<QuestScriptDef>();
	}
}
