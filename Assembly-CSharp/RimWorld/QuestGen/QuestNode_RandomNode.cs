using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A03 RID: 14851
	public class QuestNode_RandomNode : QuestNode
	{
		// Token: 0x060145AE RID: 83374 RVA: 0x00615D64 File Offset: 0x00613F64
		protected override bool TestRunInt(Slate slate)
		{
			QuestNode questNode = this.GetNodesCanRun(slate).FirstOrDefault<QuestNode>();
			if (questNode == null)
			{
				return false;
			}
			questNode.TestRun(slate);
			return true;
		}

		// Token: 0x060145AF RID: 83375 RVA: 0x00615D8C File Offset: 0x00613F8C
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		// Token: 0x060145B0 RID: 83376 RVA: 0x00615DD3 File Offset: 0x00613FD3
		private IEnumerable<QuestNode> GetNodesCanRun(Slate slate)
		{
			int num;
			for (int i = 0; i < this.nodes.Count; i = num + 1)
			{
				if (this.nodes[i].SelectionWeight(slate) > 0f && this.nodes[i].TestRun(slate.DeepCopy()))
				{
					yield return this.nodes[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0400E0C6 RID: 57542
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
