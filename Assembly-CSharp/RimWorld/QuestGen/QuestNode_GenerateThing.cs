using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A30 RID: 14896
	public class QuestNode_GenerateThing : QuestNode
	{
		// Token: 0x06014645 RID: 83525 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014646 RID: 83526 RVA: 0x00617DBC File Offset: 0x00615FBC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(this.def.GetValue(slate), null);
			thing.stackCount = (this.stackCount.GetValue(slate) ?? 1);
			if (this.contents.GetValue(slate) != null)
			{
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					thingOwner.TryAddRangeOrTransfer(this.contents.GetValue(slate), true, false);
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(slate, this.addToList.GetValue(slate), thing);
			}
		}

		// Token: 0x0400E15C RID: 57692
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E15D RID: 57693
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x0400E15E RID: 57694
		public SlateRef<ThingDef> def;

		// Token: 0x0400E15F RID: 57695
		public SlateRef<int?> stackCount;

		// Token: 0x0400E160 RID: 57696
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
