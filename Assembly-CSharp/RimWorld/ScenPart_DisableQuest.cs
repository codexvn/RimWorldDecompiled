using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028B2 RID: 10418
	public class ScenPart_DisableQuest : ScenPart
	{
		// Token: 0x0600E3FD RID: 58365 RVA: 0x00429F2F File Offset: 0x0042812F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
		}

		// Token: 0x0600E3FE RID: 58366 RVA: 0x00429F48 File Offset: 0x00428148
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

		// Token: 0x040098A0 RID: 39072
		public QuestScriptDef questDef;
	}
}
