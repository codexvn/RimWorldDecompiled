using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A50 RID: 14928
	public class QuestNode_GetMapOf : QuestNode
	{
		// Token: 0x060146B1 RID: 83633 RVA: 0x006192B1 File Offset: 0x006174B1
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060146B2 RID: 83634 RVA: 0x006192BB File Offset: 0x006174BB
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060146B3 RID: 83635 RVA: 0x006192C8 File Offset: 0x006174C8
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		// Token: 0x0400E1C3 RID: 57795
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x0400E1C4 RID: 57796
		public SlateRef<Thing> mapOf;
	}
}
