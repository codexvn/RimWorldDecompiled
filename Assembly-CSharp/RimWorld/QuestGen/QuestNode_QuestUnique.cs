using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AE3 RID: 15075
	public class QuestNode_QuestUnique : QuestNode
	{
		// Token: 0x0601489F RID: 84127 RVA: 0x00620EEB File Offset: 0x0061F0EB
		public static string GetProcessedTag(string tag, Faction faction)
		{
			if (faction == null)
			{
				return tag;
			}
			return tag + "_" + faction.Name;
		}

		// Token: 0x060148A0 RID: 84128 RVA: 0x00620F03 File Offset: 0x0061F103
		private string GetProcessedTag(Slate slate)
		{
			return QuestNode_QuestUnique.GetProcessedTag(this.tag.GetValue(slate), this.faction.GetValue(slate));
		}

		// Token: 0x060148A1 RID: 84129 RVA: 0x00620F24 File Offset: 0x0061F124
		protected override void RunInt()
		{
			string processedTag = this.GetProcessedTag(QuestGen.slate);
			QuestUtility.AddQuestTag(ref QuestGen.quest.tags, processedTag);
			if (this.storeProcessedTagAs.GetValue(QuestGen.slate) != null)
			{
				QuestGen.slate.Set<string>(this.storeProcessedTagAs.GetValue(QuestGen.slate), processedTag, false);
			}
		}

		// Token: 0x060148A2 RID: 84130 RVA: 0x00620F7C File Offset: 0x0061F17C
		protected override bool TestRunInt(Slate slate)
		{
			string processedTag = this.GetProcessedTag(slate);
			if (this.storeProcessedTagAs.GetValue(slate) != null)
			{
				slate.Set<string>(this.storeProcessedTagAs.GetValue(slate), processedTag, false);
			}
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (quest.State == QuestState.Ongoing && quest.tags.Contains(processedTag))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400E3EE RID: 58350
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x0400E3EF RID: 58351
		public SlateRef<Faction> faction;

		// Token: 0x0400E3F0 RID: 58352
		[NoTranslate]
		public SlateRef<string> storeProcessedTagAs;
	}
}
