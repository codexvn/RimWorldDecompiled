using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CBD RID: 3261
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06005370 RID: 21360 RVA: 0x001ADD54 File Offset: 0x001ABF54
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x001ADD70 File Offset: 0x001ABF70
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04003C3C RID: 15420
		protected int ticksUntilSound;
	}
}
