using System;

namespace RimWorld
{
	// Token: 0x02002F60 RID: 12128
	public class CompProperties_UseEffectGiveQuest : CompProperties_UseEffect
	{
		// Token: 0x0601117F RID: 70015 RVA: 0x004E8836 File Offset: 0x004E6A36
		public CompProperties_UseEffectGiveQuest()
		{
			this.compClass = typeof(CompUseEffect_GiveQuest);
		}

		// Token: 0x0400B170 RID: 45424
		public QuestScriptDef quest;

		// Token: 0x0400B171 RID: 45425
		public bool sendLetterQuestAvailable = true;

		// Token: 0x0400B172 RID: 45426
		public bool skipIfMechlinkAlreadySentMechs;

		// Token: 0x0400B173 RID: 45427
		public bool? discovered;
	}
}
