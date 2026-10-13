using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C78 RID: 7288
	public class QuestPart_PlayerWealth : QuestPartActivable
	{
		// Token: 0x0600A275 RID: 41589 RVA: 0x002EB04B File Offset: 0x002E924B
		public override void QuestPartTick()
		{
			if (Find.TickManager.TicksGame % 60 == 0 && WealthUtility.PlayerWealth >= this.playerWealth)
			{
				base.Complete();
			}
		}

		// Token: 0x0600A276 RID: 41590 RVA: 0x002EB06F File Offset: 0x002E926F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.playerWealth, "playerWealth", 0f, false);
		}

		// Token: 0x04007040 RID: 28736
		public float playerWealth = 100000f;

		// Token: 0x04007041 RID: 28737
		public const int CheckInterval = 60;
	}
}
