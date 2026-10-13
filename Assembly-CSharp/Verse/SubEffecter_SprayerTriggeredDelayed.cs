using System;

namespace Verse
{
	// Token: 0x02000CC4 RID: 3268
	public class SubEffecter_SprayerTriggeredDelayed : SubEffecter_SprayerTriggered
	{
		// Token: 0x06005383 RID: 21379 RVA: 0x001AEB92 File Offset: 0x001ACD92
		public SubEffecter_SprayerTriggeredDelayed(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005384 RID: 21380 RVA: 0x001AEBA3 File Offset: 0x001ACDA3
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			this.ticksLeft = this.def.initialDelayTicks;
		}

		// Token: 0x06005385 RID: 21381 RVA: 0x001AEBB8 File Offset: 0x001ACDB8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			bool flag = this.ticksLeft > 0;
			if (flag)
			{
				this.ticksLeft--;
			}
			if (this.ticksLeft <= 0 && flag)
			{
				base.MakeMote(A, B, -1);
			}
			base.SubEffectTick(A, B);
		}

		// Token: 0x04003C44 RID: 15428
		private int ticksLeft = -1;
	}
}
