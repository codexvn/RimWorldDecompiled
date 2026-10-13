using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A07 RID: 14855
	public class QuestNode_Sequence : QuestNode
	{
		// Token: 0x060145C0 RID: 83392 RVA: 0x006160BC File Offset: 0x006142BC
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		// Token: 0x060145C1 RID: 83393 RVA: 0x006160F0 File Offset: 0x006142F0
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!this.nodes[i].TestRun(slate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400E0D3 RID: 57555
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
