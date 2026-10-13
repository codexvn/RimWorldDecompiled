using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AA0 RID: 15008
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		// Token: 0x060147CC RID: 83916 RVA: 0x0061E049 File Offset: 0x0061C249
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null && this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		// Token: 0x060147CD RID: 83917 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void RunInt()
		{
		}

		// Token: 0x0400E2E6 RID: 58086
		public SlateRef<Faction> faction;
	}
}
