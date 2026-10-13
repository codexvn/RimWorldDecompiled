using System;

namespace Verse.AI.Group
{
	// Token: 0x02001076 RID: 4214
	public class PsychicRitualToil_Wait : PsychicRitualToil
	{
		// Token: 0x06006728 RID: 26408 RVA: 0x0020AA47 File Offset: 0x00208C47
		public PsychicRitualToil_Wait()
		{
		}

		// Token: 0x06006729 RID: 26409 RVA: 0x0020AA57 File Offset: 0x00208C57
		public PsychicRitualToil_Wait(int durationTicks)
		{
			this.durationTicks = durationTicks;
		}

		// Token: 0x0600672A RID: 26410 RVA: 0x0020AA6E File Offset: 0x00208C6E
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.durationTicks--;
			return this.durationTicks <= 0;
		}

		// Token: 0x0600672B RID: 26411 RVA: 0x0020AA8A File Offset: 0x00208C8A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
		}

		// Token: 0x0400499C RID: 18844
		private int durationTicks = 60;
	}
}
