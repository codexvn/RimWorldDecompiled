using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028AD RID: 10413
	public class ScenPart_CreateQuest : ScenPart
	{
		// Token: 0x0600E3EB RID: 58347 RVA: 0x00429C63 File Offset: 0x00427E63
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
		}

		// Token: 0x0600E3EC RID: 58348 RVA: 0x00429C7C File Offset: 0x00427E7C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			string label;
			if (this.questDef != null)
			{
				label = ((!string.IsNullOrEmpty(this.questDef.label)) ? this.questDef.LabelCap : this.questDef.defName);
			}
			else
			{
				label = "ScenPart_SelectQuestDef".Translate();
			}
			if (Widgets.ButtonText(scenPartRect, label, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
				{
					QuestScriptDef localFd = questScriptDef;
					string label2 = (!string.IsNullOrEmpty(questScriptDef.label)) ? questScriptDef.LabelCap : questScriptDef.defName;
					list.Add(new FloatMenuOption(label2, delegate()
					{
						this.questDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E3ED RID: 58349 RVA: 0x00429DA8 File Offset: 0x00427FA8
		public override void PostGameStart()
		{
			if (this.questDef == null)
			{
				return;
			}
			QuestUtility.GenerateQuestAndMakeAvailable(this.questDef, 0f);
		}

		// Token: 0x0400989A RID: 39066
		private QuestScriptDef questDef;
	}
}
