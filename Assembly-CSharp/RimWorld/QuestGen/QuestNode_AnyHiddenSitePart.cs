using System;
using System.Collections.Generic;
using System.Linq;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A92 RID: 14994
	public class QuestNode_AnyHiddenSitePart : QuestNode
	{
		// Token: 0x06014799 RID: 83865 RVA: 0x0061D8D7 File Offset: 0x0061BAD7
		protected override bool TestRunInt(Slate slate)
		{
			if (this.AnyHiddenSitePart(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0601479A RID: 83866 RVA: 0x0061D90F File Offset: 0x0061BB0F
		protected override void RunInt()
		{
			if (this.AnyHiddenSitePart(QuestGen.slate))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x0601479B RID: 83867 RVA: 0x0061D948 File Offset: 0x0061BB48
		private bool AnyHiddenSitePart(Slate slate)
		{
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				return value.Any((SitePartDef x) => x.defaultHidden);
			}
			return false;
		}

		// Token: 0x0400E2BF RID: 58047
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x0400E2C0 RID: 58048
		public QuestNode node;

		// Token: 0x0400E2C1 RID: 58049
		public QuestNode elseNode;
	}
}
