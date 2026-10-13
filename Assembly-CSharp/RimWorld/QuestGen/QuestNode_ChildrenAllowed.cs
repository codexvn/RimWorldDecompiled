using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A94 RID: 14996
	public class QuestNode_ChildrenAllowed : QuestNode
	{
		// Token: 0x060147A0 RID: 83872 RVA: 0x0061D9A0 File Offset: 0x0061BBA0
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x060147A1 RID: 83873 RVA: 0x0061D9D2 File Offset: 0x0061BBD2
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x060147A2 RID: 83874 RVA: 0x0061DA00 File Offset: 0x0061BC00
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool childrenAllowed = Find.Storyteller.difficulty.ChildrenAllowed;
			slate.Set<bool>("allowChildren", childrenAllowed, false);
			if (childrenAllowed)
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

		// Token: 0x0400E2C4 RID: 58052
		public QuestNode node;

		// Token: 0x0400E2C5 RID: 58053
		public QuestNode elseNode;
	}
}
