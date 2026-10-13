using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AF8 RID: 15096
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		// Token: 0x060148E8 RID: 84200 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060148E9 RID: 84201 RVA: 0x00621AC0 File Offset: 0x0061FCC0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		// Token: 0x0400E427 RID: 58407
		public SlateRef<Pawn> pawn;

		// Token: 0x0400E428 RID: 58408
		public SlateRef<RoyalTitleDef> royalTitle;

		// Token: 0x0400E429 RID: 58409
		public SlateRef<Faction> faction;
	}
}
