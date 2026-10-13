using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BD3 RID: 11219
	public class CompAncientUplink : ThingComp
	{
		// Token: 0x17002753 RID: 10067
		// (get) Token: 0x0600FAF2 RID: 64242 RVA: 0x0048E8A4 File Offset: 0x0048CAA4
		private List<QuestScriptDef> ScannerQuests
		{
			get
			{
				List<QuestScriptDef> result;
				if ((result = this.scannerQuests) == null)
				{
					result = (this.scannerQuests = QuestUtility.GetGiverQuests(QuestGiverTag.OrbitalScanner).ToList<QuestScriptDef>());
				}
				return result;
			}
		}

		// Token: 0x0600FAF3 RID: 64243 RVA: 0x0048E8D0 File Offset: 0x0048CAD0
		public override void Notify_Hacked(Pawn hacker = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromUplink".Translate(hacker.Named("HACKER")), false);
			QuestScriptDef questScriptDef = this.ScannerQuests.RandomElementByWeight((QuestScriptDef q) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(q, points, Find.World.StoryState));
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
			if (!quest.hidden && questScriptDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
		}

		// Token: 0x0400A4C9 RID: 42185
		private List<QuestScriptDef> scannerQuests;
	}
}
