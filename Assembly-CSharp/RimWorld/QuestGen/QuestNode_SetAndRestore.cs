using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A09 RID: 14857
	public class QuestNode_SetAndRestore : QuestNode
	{
		// Token: 0x060145C7 RID: 83399 RVA: 0x006161AC File Offset: 0x006143AC
		protected override bool TestRunInt(Slate slate)
		{
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo(this.name.GetValue(slate));
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
			return result;
		}

		// Token: 0x060145C8 RID: 83400 RVA: 0x00616214 File Offset: 0x00614414
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo(this.name.GetValue(slate));
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			try
			{
				this.node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x0400E0D7 RID: 57559
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400E0D8 RID: 57560
		public SlateRef<object> value;

		// Token: 0x0400E0D9 RID: 57561
		public QuestNode node;
	}
}
