using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A6F RID: 14959
	public class QuestNode_GetPopIntentForQuest : QuestNode
	{
		// Token: 0x06014721 RID: 83745 RVA: 0x0061BA8A File Offset: 0x00619C8A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014722 RID: 83746 RVA: 0x0061BA94 File Offset: 0x00619C94
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014723 RID: 83747 RVA: 0x0061BAA4 File Offset: 0x00619CA4
		private void SetVars(Slate slate)
		{
			float populationIntentForQuest = StorytellerUtilityPopulation.PopulationIntentForQuest;
			slate.Set<float>(this.storeAs.GetValue(slate), populationIntentForQuest, false);
		}

		// Token: 0x0400E23F RID: 57919
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
