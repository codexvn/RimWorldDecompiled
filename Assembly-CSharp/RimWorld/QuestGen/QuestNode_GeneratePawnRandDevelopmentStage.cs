using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A2D RID: 14893
	public class QuestNode_GeneratePawnRandDevelopmentStage : QuestNode_GeneratePawn
	{
		// Token: 0x0601463D RID: 83517 RVA: 0x00617AE8 File Offset: 0x00615CE8
		protected override DevelopmentalStage GetDevelopmentalStage(Slate slate)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return DevelopmentalStage.Adult;
			}
			float value = this.childChance.GetValue(slate);
			float value2 = this.adultChance.GetValue(slate);
			float num = value / (value + value2);
			if (Rand.Value > num)
			{
				return DevelopmentalStage.Adult;
			}
			return DevelopmentalStage.Child;
		}

		// Token: 0x0400E146 RID: 57670
		public SlateRef<float> childChance;

		// Token: 0x0400E147 RID: 57671
		public SlateRef<float> adultChance;
	}
}
