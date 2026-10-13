using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CC5 RID: 3269
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06005386 RID: 21382 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x001AEC04 File Offset: 0x001ACE04
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
					return;
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x04003C45 RID: 15429
		private int age;

		// Token: 0x04003C46 RID: 15430
		private Sustainer sustainer;
	}
}
