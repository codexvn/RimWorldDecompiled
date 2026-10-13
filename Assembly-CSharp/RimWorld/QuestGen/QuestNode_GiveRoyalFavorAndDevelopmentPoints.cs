using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003ACD RID: 15053
	public class QuestNode_GiveRoyalFavorAndDevelopmentPoints : QuestNode_GiveRoyalFavor
	{
		// Token: 0x0601485E RID: 84062 RVA: 0x0061FC78 File Offset: 0x0061DE78
		protected override void PostProcessRewardChoice(QuestPart_Choice rewardChoice)
		{
			if (ModsConfig.IdeologyActive && Faction.OfPlayer.ideos.FluidIdeo != null)
			{
				for (int i = 0; i < rewardChoice.choices.Count; i++)
				{
					rewardChoice.choices[i].rewards.Add(new Reward_DevelopmentPoints(QuestGen.quest));
				}
			}
		}
	}
}
