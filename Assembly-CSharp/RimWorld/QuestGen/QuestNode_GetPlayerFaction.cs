using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A6E RID: 14958
	public class QuestNode_GetPlayerFaction : QuestNode
	{
		// Token: 0x0601471E RID: 83742 RVA: 0x0061BA44 File Offset: 0x00619C44
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
		}

		// Token: 0x0601471F RID: 83743 RVA: 0x0061BA6F File Offset: 0x00619C6F
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<Faction>(this.storeAs.GetValue(slate), Faction.OfPlayer, false);
			return true;
		}

		// Token: 0x0400E23E RID: 57918
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
