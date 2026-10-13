using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA1 RID: 15009
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		// Token: 0x060147CF RID: 83919 RVA: 0x0061E06C File Offset: 0x0061C26C
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x060147D0 RID: 83920 RVA: 0x0061E09E File Offset: 0x0061C29E
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x060147D1 RID: 83921 RVA: 0x0061E0CC File Offset: 0x0061C2CC
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			slate.Set<bool>("allowViolentQuests", allowViolentQuests, false);
			if (allowViolentQuests)
			{
				if (this.node != null)
				{
					return func(this.node);
				}
			}
			else if (this.elseNode != null)
			{
				return func(this.elseNode);
			}
			return true;
		}

		// Token: 0x0400E2E7 RID: 58087
		public QuestNode node;

		// Token: 0x0400E2E8 RID: 58088
		public QuestNode elseNode;
	}
}
